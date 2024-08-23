//using HunterProtobufCore;
using System.Buffers;
using System.IO;
using System.Net;
using System.Net.Sockets;
using static HaoYueNet.ClientNetwork.BaseData;

namespace HaoYueNet.ClientNetwork.IOCPMode
{
    public class TcpSaeaClient
    {
        #region 定义属性
        protected int MaxRevIndexNum = 50;//响应倒计时计数最大值
        protected int MaxSendIndexNum = 3;//发送倒计时计数最大值
        protected static int TimerInterval = 3000;//计时器间隔
        protected System.Timers.Timer _heartTimer;//心跳包计数器
        public int m_maxConnectNum;    //最大连接数  
        public int m_revBufferSize;    //最大接收字节数  
        protected BufferManager m_bufferManager;
        protected const int opsToAlloc = 2;
        protected SocketEventPool m_Receivepool;
        protected SocketEventPool m_Sendpool;
        protected TokenMsgPool msg_pool;
        protected int m_clientCount;              //连接的客户端数量  
        protected Semaphore m_maxNumberAcceptedClients;//信号量
        protected Dictionary<Socket, AsyncUserToken> _DictSocketAsyncUserToken = new Dictionary<Socket, AsyncUserToken>();
        List<AsyncUserToken> m_clients; //客户端列表
        public List<AsyncUserToken> ClientList { private set { m_clients = value; } get { return m_clients; } } //获取客户端列表
        #endregion

        #region 定义委托  
        /// <summary>  
        /// 客户端连接数量变化时触发  
        /// </summary>  
        /// <param name="num">当前增加客户的个数(用户退出时为负数,增加时为正数,一般为1)</param>  
        /// <param name="token">增加用户的信息</param>  
        public delegate void OnClientNumberChangeHandler(int num, AsyncUserToken token);
        /// <summary>  
        /// 接收到客户端的数据  
        /// </summary>  
        /// <param name="token">客户端</param>  
        /// <param name="buff">客户端数据</param>  
        public delegate void OnReceiveDataHandler(AsyncUserToken sk, int CMDID, int ErrID, byte[] data);
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="sk"></param>
        public delegate void OnDisconnectHandler(AsyncUserToken sk);
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="sk"></param>
        public delegate void OnNetLogHandler(string msg);
        #endregion

        #region 定义事件  
        /// <summary>  
        /// 客户端连接数量变化事件  
        /// </summary>
        public event OnClientNumberChangeHandler OnClientNumberChange;
        /// <summary>  
        /// 接收到客户端的数据事件  
        /// </summary>
        public event OnReceiveDataHandler OnReceive;
        /// <summary>  
        /// 接收到客户端的断开连接
        /// </summary>
        public event OnDisconnectHandler OnDisconnected;
        /// <summary>  
        /// 网络库内部输出
        /// </summary>
        public event OnNetLogHandler OnNetLog;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="numConnections">最大连接数</param>  
        /// <param name="receiveBufferSize">缓存区大小</param>  
        public TcpSaeaClient(int numConnections, int receiveBufferSize)
        {
            m_clientCount = 0;
            m_maxConnectNum = numConnections;
            m_revBufferSize = receiveBufferSize;
            // allocate buffers such that the maximum number of sockets can have one outstanding read and   
            //write posted to the socket simultaneously
            m_bufferManager = new BufferManager(receiveBufferSize * numConnections * opsToAlloc, receiveBufferSize);

            m_Receivepool = new SocketEventPool(numConnections);
            m_Sendpool = new SocketEventPool(numConnections);

            msg_pool = new TokenMsgPool(numConnections);

            m_maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);
        }

        #region Client操作
        /// <summary>  
        /// 初始化  
        /// </summary>  
        public void Init()
        {
            // Allocates one large byte buffer which all I/O operations use a piece of.  This gaurds   
            // against memory fragmentation  
            m_bufferManager.InitBuffer();
            m_clients = new List<AsyncUserToken>();
            // preallocate pool of SocketAsyncEventArgs objects  
            SocketAsyncEventArgs readWriteEventArg;

            for (int i = 0; i < m_maxConnectNum; i++)
            {
                readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                readWriteEventArg.UserToken = new AsyncUserToken();
                // assign a byte buffer from the buffer pool to the SocketAsyncEventArg object  
                m_bufferManager.SetBuffer(readWriteEventArg);
                // add SocketAsyncEventArg to the pool  
                m_Receivepool.Push(readWriteEventArg);
            }

            for (int i = 0; i < m_maxConnectNum; i++)
            {
                readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                readWriteEventArg.UserToken = new AsyncUserToken();

                //发送是否需要如此设置 TODO
                m_bufferManager.SetBuffer(readWriteEventArg);

                m_Sendpool.Push(readWriteEventArg);
            }
            OutNetLog("初始化完毕");
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="localEndPoint"></param>
        /// <param name="bReuseAddress">是否端口重用</param>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                ClearUserToken();

                _heartTimer = new System.Timers.Timer();
                _heartTimer.Interval = TimerInterval;
                _heartTimer.Elapsed += CheckUpdatetimer_Elapsed;
                _heartTimer.AutoReset = true;
                _heartTimer.Enabled = true;
                OutNetLog("开启定时心跳包");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>  
        /// 停止服务  
        /// </summary>  
        public void Stop()
        {
            foreach (AsyncUserToken token in m_clients)
            {
                try
                {
                    token.Socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception) { }
            }
            try
            {
                for (int i = 0; i < m_clients.Count; i++)
                {
                    m_clients[i].Socket.Shutdown(SocketShutdown.Both);
                }
                //listenSocket.Shutdown(SocketShutdown.Both);

                for (int i = 0; i < m_clients.Count; i++)
                {
                    m_clients[i].Socket.Close();
                }
            }
            catch (Exception) { }

            //listenSocket.Close();
            int c_count = m_clients.Count;
            ClearUserToken();

            if (OnClientNumberChange != null)
                OnClientNumberChange(-c_count, null);
        }
        public void CloseClient(AsyncUserToken token)
        {
            try { token.Socket.Shutdown(SocketShutdown.Both); }
            catch (Exception) { }
        }
        /// <summary>
        /// 关闭客户端连接
        /// </summary>
        /// <param name="e"></param>
        void CloseClientSocket(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = e.UserToken as AsyncUserToken;
            CloseReady(token);
            // 释放SocketAsyncEventArg，以便其他客户端可以重用它们 
            ReleaseSocketAsyncEventArgs(e);
        }
        void CloseReady(AsyncUserToken token)
        {
            OnDisconnected?.Invoke(token);
            RemoveUserToken(token);
            //如果有事件,则调用事件,发送客户端数量变化通知
            OnClientNumberChange?.Invoke(-1, token);
            // 关闭与客户端关联的套接字 
            try { token.Socket.Shutdown(SocketShutdown.Send); } catch (Exception) { }
            token.Socket.Close();
            // 递减计数器以跟踪连接到服务器的客户端总数
            Interlocked.Decrement(ref m_clientCount);
            m_maxNumberAcceptedClients.Release();
        }
        #endregion

        #region Token管理
        public AsyncUserToken GetAsyncUserTokenForSocket(Socket sk)
        {
            AsyncUserToken result;
            if (_DictSocketAsyncUserToken.TryGetValue(sk, out result))
                return result;
            return null;
            //return _DictSocketAsyncUserToken.ContainsKey(sk) ? _DictSocketAsyncUserToken[sk] : null;
        }
        void AddUserToken(AsyncUserToken userToken)
        {
            lock (_DictSocketAsyncUserToken)
            {
                m_clients.Add(userToken);
                _DictSocketAsyncUserToken.Add(userToken.Socket, userToken);
            }
        }
        void RemoveUserToken(AsyncUserToken userToken)
        {
            lock (_DictSocketAsyncUserToken)
            {
                m_clients.Remove(userToken);
                _DictSocketAsyncUserToken.Remove(userToken.Socket);
            }
        }
        void ClearUserToken()
        {
            lock (_DictSocketAsyncUserToken)
            {
                m_clients.Clear();
                _DictSocketAsyncUserToken.Clear();
            }
        }
        /// <summary>
        /// 回收SocketAsyncEventArgs
        /// </summary>
        /// <param name="saea"></param>
        /// <exception cref="ArgumentException"></exception>
        void ReleaseSocketAsyncEventArgs(SocketAsyncEventArgs saea)
        {
            //saea.UserToken = null;//TODO
            //saea.SetBuffer(null, 0, 0);
            //saea.Dispose();
            //↑ 这里不要自作主张去清东西，否则回收回去不可用

            switch (saea.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    m_Receivepool.Push(saea);
                    break;
                case SocketAsyncOperation.Send:
                    m_Sendpool.Push(saea);
                    break;
                default:
                    throw new ArgumentException("ReleaseSocketAsyncEventArgs > The last operation completed on the socket was not a receive or send");
            }

        }
        #endregion



        #region 连接
        public void StartConnect(string ip, int port, Socket socket, SocketAsyncEventArgs connectEventArg = null)
        {
            if (connectEventArg == null)
            {
                connectEventArg = new SocketAsyncEventArgs();
                connectEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            }
            else
            {
                // socket must be cleared since the context object is being reused  
                connectEventArg.AcceptSocket = null;
                //TODO ??
                //这里其实AcceptSocket是服务端用的，非监听的客户端用，本来就为空，清不清理没什么卵用，但是connectEventArg.ConnectSocket是只读的的
            }

            connectEventArg.RemoteEndPoint = new DnsEndPoint(ip, port);

            m_maxNumberAcceptedClients.WaitOne();

            if (!socket.ConnectAsync(connectEventArg))
            {
                ProcessConnect(connectEventArg);
            }
        }

        private void ProcessConnect(SocketAsyncEventArgs e)
        {
            try
            {
                Interlocked.Increment(ref m_clientCount);

                //确保监听结束时，有连接才抛给数据接收
                if (e.ConnectSocket.RemoteEndPoint != null)
                {
                    // Get the socket for the accepted client connection and put it into the   
                    //ReadEventArg object user token  
                    SocketAsyncEventArgs readEventArgs = m_Receivepool.Pop();
                    //TODO readEventArgs.UserToken这里的 UserToken 有可能是空
                    AsyncUserToken userToken;
                    if (readEventArgs.UserToken == null)
                        readEventArgs.UserToken = new AsyncUserToken();

                    userToken = (AsyncUserToken)readEventArgs.UserToken;
                    userToken.Socket = e.ConnectSocket;
                    userToken.ConnectTime = DateTime.Now;
                    userToken.Remote = e.ConnectSocket.RemoteEndPoint;
                    userToken.IPAddress = ((IPEndPoint)e.ConnectSocket.RemoteEndPoint).Address;

                    userToken.RevIndex = MaxRevIndexNum;
                    userToken.SendIndex = MaxSendIndexNum;

                    AddUserToken(userToken);

                    OnClientNumberChange?.Invoke(1, userToken);
                    if (!e.ConnectSocket.ReceiveAsync(readEventArgs))
                    {
                        ProcessReceive(readEventArgs);
                    }
                }
            }
            catch (Exception me)
            {
                //RuncomLib.Log.LogUtils.Info(me.Message + "\r\n" + me.StackTrace);
            }

            // Accept the next connection request  
            if (e.SocketError == SocketError.OperationAborted) return;
            //StartAccept(e);
        }
        #endregion

        #region 收发IOCP循环
        /// <summary>当异步接收操作完成时，会调用此方法。
        /// 如果远程主机关闭了连接，则套接字关闭。
        /// 如果接收到数据，则将数据回显到客户端。
        /// </summary>
        /// <param name="e"></param>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            try
            {
                AsyncUserToken token = (AsyncUserToken)e.UserToken;
                if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
                {
                    lock (token.memoryStream)
                    {
                        token.memoryStream.Write(e.Buffer, e.Offset, e.BytesTransferred);
                        do
                        {
                            if (token.memoryStream.Length < 4) break;//包头不完整，继续接收
                            long FristBeginPos = token.memoryStream.Position;

                            //从Byte池申请
                            byte[] lenBytes = ArrayPool<byte>.Shared.Rent(4);

                            token.memoryStream.Seek(0, SeekOrigin.Begin);
                            token.memoryStream.Read(lenBytes, 0, 4);
                            int packageLen = BitConverter.ToInt32(lenBytes, 0) - 4;

                            //归还byte[]
                            ArrayPool<byte>.Shared.Return(lenBytes);

                            if (packageLen > token.memoryStream.Length - 4)
                            {
                                token.memoryStream.Seek(FristBeginPos, SeekOrigin.Begin);
                                break;//长度不够时,退出循环,让程序继续接收  
                            }

                            //申请byte池 一定要记得回收!!
                            byte[] rev_fromArrayPool = ArrayPool<byte>.Shared.Rent(packageLen);

                            token.memoryStream.Seek(4, SeekOrigin.Begin);
                            token.memoryStream.Read(rev_fromArrayPool, 0, packageLen);
                            token.memoryStream.Seek(FristBeginPos, SeekOrigin.Begin);
                            //从数据池中移除这组数据
                            lock (token.memoryStream)
                            {
                                int numberOfBytesToRemove = packageLen + 4;
                                byte[] buf = token.memoryStream.GetBuffer();
                                Buffer.BlockCopy(buf, numberOfBytesToRemove, buf, 0, (int)token.memoryStream.Length - numberOfBytesToRemove);
                                token.memoryStream.SetLength(token.memoryStream.Length - numberOfBytesToRemove);
                            }

                            //用Span内存切片，因为来自ArrayPool的byte长度，可能大于本身申请的长度
                            Span<byte> rev_span = rev_fromArrayPool;
                            rev_span = rev_span.Slice(0, packageLen);
                            DataCallBackReady(token, rev_span);

                            //回收（这里依赖DataCallBackReady中，有一次数据拷贝，这个后续还要进一步精进性能优化，否则不能在这里回收，否则影响业务层）
                            ArrayPool<byte>.Shared.Return(rev_fromArrayPool);
                        } while (token.memoryStream.Length > 4);
                    }

                    //如果返回为False则代表此刻已经完成，不必等待完成端口回调，则直接调用ProcessReceive
                    if (!token.Socket.ReceiveAsync(e))
                        this.ProcessReceive(e);
                }
                else
                {
                    //清理数据
                    token.memoryStream.SetLength(0);
                    token.memoryStream.Seek(0, SeekOrigin.Begin);
                    CloseClientSocket(e);
                }
            }
            catch (Exception xe)
            {
                //RuncomLib.Log.LogUtils.Info(xe.Message + "\r\n" + xe.StackTrace);
            }
        }
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                //TODO
            }
            else
            {
                CloseClientSocket(e);
                return;
            }
            ReleaseSocketAsyncEventArgs(e);
            SendForMsgPool();
        }
        void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            // determine which type of operation just completed and call the associated handler  

            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    ProcessConnect(e);
                    break;
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }
        #endregion

        #region 发送
        int sendrun = 0;
        /// <summary>
        /// 对外暴露的发送消息
        /// </summary>
        /// <param name="CMDID"></param>
        /// <param name="data">序列化之后的数据</param>
        public void SendToSocket(Socket sk, int CMDID, byte[] data)
        {
            AsyncUserToken token = GetAsyncUserTokenForSocket(sk);
            SendWithIndex(token, (ushort)CMDID, data);
        }

        void SendForMsgPool()
        {
            //if (flag_SendForMsgPool) return;
            try
            {
                if (sendrun < msg_pool.Count || msg_pool.Count < 1)
                    return;

                sendrun++;
                while (msg_pool.Count > 0)
                {
                    try
                    {
                        TokenWithMsg msg = msg_pool.Dequeue();
                        //OutNetLog("从信息池取出发送");
                        //是心跳包
                        if (msg.bHeartbeat)
                        {
                            SendHeartbeatMessage(msg.token);
                        }
                        else
                        {
                            SendMessage(msg.token, msg.CMDID, msg.data);
                        }
                        msg = null;
                    }
                    catch
                    {
                        OutNetLog("==============================================>");
                    }
                }
                sendrun--;
                OutNetLog("!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }
            catch (Exception ex)
            {
                OutNetLog(ex.ToString());
            }

        }

        /// <summary>
        /// 发送心跳包
        /// </summary>
        /// <param name="token"></param>
        void SendHeartbeatMessage(AsyncUserToken token)
        {
            if (token == null || token.Socket == null || !token.Socket.Connected)
                return;
            try
            {
                if (m_Sendpool.Count > 0)
                {
                    SocketAsyncEventArgs myreadEventArgs = m_Sendpool.Pop();
                    myreadEventArgs.UserToken = token;
                    //TODO ????客户端用这里是否应该直接就不设置
                    //myreadEventArgs.AcceptSocket = token.Socket;

                    //直接写入SocketAsyncEventArgs的Buff
                    HunterNet_Heartbeat.SetDataToSocketAsyncEventArgs(myreadEventArgs);

                    //若不需要等待
                    if (!token.Socket.SendAsync(myreadEventArgs))
                    {
                        m_Sendpool.Push(myreadEventArgs);
                    }
                    return;
                }
                else
                {
                    //先压入队列，等待m_Sendpool回收
                    msg_pool.Enqueue(new TokenWithMsg() { token = token, bHeartbeat = true });
                    //OutNetLog("！！！！压入消息发送队列MSG_Pool");
                    return;
                }
            }
            catch (Exception e)
            {
                OutNetLog(e.ToString());
            }
        }

        /// <summary>
        /// 发送数据并计数
        /// </summary>
        /// <param name="data"></param>
        void SendWithIndex(AsyncUserToken token, ushort CmdID, byte[] data)
        {
            try
            {
                //发送数据
                SendMessage(token, CmdID, data);
                token.SendIndex = MaxSendIndexNum;
            }
            catch
            {
                CloseReady(token);
            }
        }

        void SendMessage(AsyncUserToken token, ushort CmdID, byte[] data)
        {
            if (token == null || token.Socket == null || !token.Socket.Connected)
                return;
            try
            {
                if (m_Sendpool.Count > 0)
                {
                    SocketAsyncEventArgs myreadEventArgs = m_Sendpool.Pop();
                    myreadEventArgs.UserToken = token;
                    myreadEventArgs.AcceptSocket = token.Socket;
                    //myreadEventArgs.SetBuffer(message, 0, message.Length);  //将数据放置进去.  
                    //更换为CMDID和Data直接写入SocketAsyncEventArgs的Buff
                    HunterNet_C2S.SetDataToSocketAsyncEventArgs(myreadEventArgs, CmdID, data);

                    //若不需要等待
                    if (!token.Socket.SendAsync(myreadEventArgs))
                    {
                        m_Sendpool.Push(myreadEventArgs);
                    }
                    return;
                }
                else
                {
                    //先压入队列，等待m_Sendpool回收
                    msg_pool.Enqueue(new TokenWithMsg() { token = token, CMDID = CmdID, data = data });
                    //OutNetLog("！！！！压入消息发送队列MSG_Pool");
                    return;
                }
            }
            catch (Exception e)
            {
                OutNetLog(e.ToString());
            }
        }
        #endregion

        #region 处理前预备
        private void DataCallBackReady(AsyncUserToken sk, Span<byte> data)
        {
            //增加接收计数
            sk.RevIndex = MaxRevIndexNum;

            if (data.Length == 1 && data[0] == 0x00)//心跳包
            {
                //OutNetLog("收到心跳包");
                //无处理
            }
            else
            {
                try
                {
                    //将数据包交给后台处理,这里你也可以新开个线程来处理.加快速度.  
                    HunterNet_S2C.AnalysisPkgData(data, out ushort CmdID, out ushort Error, out byte[] resultdata);
                    OnReceive?.Invoke(sk, CmdID, Error, resultdata);
                }
                catch (Exception ex)
                {
                    OutNetLog("数据解析错误");
                }
            }
        }
        private void OutNetLog(string msg)
        {
            OnNetLog?.Invoke(msg);
        }
        #endregion

        #region 心跳包
        /// <summary>
        /// 发送心跳包
        /// </summary>
        /// <param name="sk"></param>
        /// 
        private void SendHeartbeatWithIndex(AsyncUserToken token)
        {
            if (token == null || token.Socket == null || !token.Socket.Connected)
                return;
            try
            {
                //OutNetLog(DateTime.Now.ToString() + "发送心跳包");
                token.SendIndex = MaxSendIndexNum;
                SendHeartbeatMessage(token);
            }
            catch (Exception e)
            {
                CloseReady(token);
            }
        }
        /// <summary>
        /// 心跳包时钟事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckUpdatetimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            for (int i = 0; i < m_clients.Count(); i++)
            {
                //接收服务器数据计数
                m_clients[i].RevIndex--;
                if (m_clients[i].RevIndex <= 0)
                {
                    //判定掉线
                    CloseReady(m_clients[i]);
                    return;
                }

                //发送计数
                m_clients[i].SendIndex--;
                if (m_clients[i].SendIndex <= 0)//需要发送心跳包了
                {
                    //重置倒计时计数
                    m_clients[i].SendIndex = MaxSendIndexNum;
                    SendHeartbeatWithIndex(m_clients[i]);
                }
            }
        }
        #endregion
    }
}
