﻿using Google.Protobuf;
using HunterProtobufCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static Google.Protobuf.Reflection.FieldOptions.Types;


namespace HaoYueNet.ServerNetwork
{
    public class SocketManager
    {

        /// <summary>
        /// 心跳包数据
        /// </summary>
        private byte[] HeartbeatData = new byte[5] { 0x05, 0x00, 0x00, 0x00, 0x00 };
        //响应倒计时计数最大值
        //public int MaxRevIndexNum { get; set; } = 5;
        ////发送倒计时计数最大值
        //public int MaxSendIndexNum { get; set; } = 3;

        //响应倒计时计数最大值
        public int MaxRevIndexNum { get; set; } = 50;
        //发送倒计时计数最大值
        public int MaxSendIndexNum { get; set; } = 3;

        //计时器间隔
        private static int TimerInterval = 3000;

        /// <summary>
        /// 心跳包计数器
        /// </summary>
        private System.Timers.Timer _heartTimer;

        public int m_maxConnectNum;    //最大连接数  
        public int m_revBufferSize;    //最大接收字节数  
        public BufferManager m_bufferManager;
        public const int opsToAlloc = 2;
        Socket listenSocket;            //监听Socket  
        public SocketEventPool m_Receivepool;

        public SocketEventPool m_Sendpool;
        public TokenMsgPool msg_pool;
        public int m_clientCount;              //连接的客户端数量  
        public Semaphore m_maxNumberAcceptedClients;//信号量

        List<AsyncUserToken> m_clients; //客户端列表  
        public Dictionary<Socket, AsyncUserToken> _DictSocketAsyncUserToken = new Dictionary<Socket, AsyncUserToken>();



        #region Token管理

        void ClearUserToken()
        {
            lock (_DictSocketAsyncUserToken)
            {
                m_clients.Clear();
                _DictSocketAsyncUserToken.Clear();
            }
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
        #endregion

        #region 定义委托  

        /// <summary>  
        /// 客户端连接数量变化时触发  
        /// </summary>  
        /// <param name="num">当前增加客户的个数(用户退出时为负数,增加时为正数,一般为1)</param>  
        /// <param name="token">增加用户的信息</param>  
        public delegate void OnClientNumberChange(int num, AsyncUserToken token);

        /// <summary>  
        /// 接收到客户端的数据  
        /// </summary>  
        /// <param name="token">客户端</param>  
        /// <param name="buff">客户端数据</param>  
        public delegate void OnReceiveData(AsyncUserToken token, byte[] buff);

        #endregion

        #region 定义事件  
        /// <summary>  
        /// 客户端连接数量变化事件  
        /// </summary>  
        public event OnClientNumberChange ClientNumberChange;

        /// <summary>  
        /// 接收到客户端的数据事件  
        /// </summary>  
        public event OnReceiveData ReceiveClientData;
        #endregion

        #region 定义属性
        /// <summary>  
        /// 获取客户端列表  
        /// </summary>  
        public List<AsyncUserToken> ClientList { get { return m_clients; } }
        #endregion

        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="numConnections">最大连接数</param>  
        /// <param name="receiveBufferSize">缓存区大小</param>  
        public SocketManager(int numConnections, int receiveBufferSize)
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
        }


        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="localEndPoint"></param>
        /// <param name="bReuseAddress">是否端口重用</param>
        /// <returns></returns>
        public bool Start(IPEndPoint localEndPoint,bool bReuseAddress = false)
        {
            try
            {
                ClearUserToken();
                listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                if (bReuseAddress)
                {
                    listenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                }

                listenSocket.Bind(localEndPoint);
                // start the server with a listen backlog of 100 connections  
                listenSocket.Listen(m_maxConnectNum);
                // post accepts on the listening socket  
                StartAccept(null);

                _heartTimer = new System.Timers.Timer();
                _heartTimer.Interval = TimerInterval;
                _heartTimer.Elapsed += CheckUpdatetimer_Elapsed;
                _heartTimer.AutoReset = true;
                _heartTimer.Enabled = true;
                //Console.WriteLine("开启心跳包定时器");

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
                listenSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }

            listenSocket.Close();
            int c_count = m_clients.Count;
            ClearUserToken();

            if (ClientNumberChange != null)
                ClientNumberChange(-c_count, null);
        }


        public void CloseClient(AsyncUserToken token)
        {
            try
            {
                token.Socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }
        }


        // Begins an operation to accept a connection request from the client   
        //  
        // <param name="acceptEventArg">The context object to use when issuing   
        // the accept operation on the server's listening socket</param>  
        public void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                // socket must be cleared since the context object is being reused  
                acceptEventArg.AcceptSocket = null;
            }

            m_maxNumberAcceptedClients.WaitOne();
            if (!listenSocket.AcceptAsync(acceptEventArg))
            {
                ProcessAccept(acceptEventArg);
            }
        }

        // This method is the callback method associated with Socket.AcceptAsync   
        // operations and is invoked when an accept operation is complete  
        //  
        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            try
            {
                Interlocked.Increment(ref m_clientCount);
                // Get the socket for the accepted client connection and put it into the   
                //ReadEventArg object user token  
                SocketAsyncEventArgs readEventArgs = m_Receivepool.Pop();
                AsyncUserToken userToken = (AsyncUserToken)readEventArgs.UserToken;
                userToken.Socket = e.AcceptSocket;
                userToken.ConnectTime = DateTime.Now;
                userToken.Remote = e.AcceptSocket.RemoteEndPoint;
                userToken.IPAddress = ((IPEndPoint)(e.AcceptSocket.RemoteEndPoint)).Address;


                userToken.RevIndex = MaxRevIndexNum;
                userToken.SendIndex = MaxSendIndexNum;

                AddUserToken(userToken);

                ClientNumberChange?.Invoke(1, userToken);
                if (!e.AcceptSocket.ReceiveAsync(readEventArgs))
                {
                    ProcessReceive(readEventArgs);
                }
            }
            catch (Exception me)
            {
                //RuncomLib.Log.LogUtils.Info(me.Message + "\r\n" + me.StackTrace);
            }

            // Accept the next connection request  
            if (e.SocketError == SocketError.OperationAborted) return;
            StartAccept(e);
        }

        void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            // determine which type of operation just completed and call the associated handler  

            switch (e.LastOperation)
            {
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

        // This method is invoked when an asynchronous receive operation completes.   
        // If the remote host closed the connection, then the socket is closed.    
        // If data was received then the data is echoed back to the client.  
        //  
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            try
            {
                // check if the remote host closed the connection  
                AsyncUserToken token = (AsyncUserToken)e.UserToken;
                if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
                {
                    //读取数据  
                    byte[] data = new byte[e.BytesTransferred];
                    Array.Copy(e.Buffer, e.Offset, data, 0, e.BytesTransferred);
                    lock (token.Buffer)
                    {
                        token.Buffer.AddRange(data);
                    }
                    //注意:你一定会问,这里为什么要用do-while循环?   
                    //如果当客户发送大数据流的时候,e.BytesTransferred的大小就会比客户端发送过来的要小,  
                    //需要分多次接收.所以收到包的时候,先判断包头的大小.够一个完整的包再处理.  
                    //如果客户短时间内发送多个小数据包时, 服务器可能会一次性把他们全收了.  
                    //这样如果没有一个循环来控制,那么只会处理第一个包,  
                    //剩下的包全部留在token.Buffer中了,只有等下一个数据包过来后,才会放出一个来.  
                    do
                    {

                        //如果包头不完整
                        if (token.Buffer.Count < 4)
                            break;

                        //判断包的长度  
                        byte[] lenBytes = token.Buffer.GetRange(0, 4).ToArray();
                        int packageLen = BitConverter.ToInt32(lenBytes, 0) - 4;
                        if (packageLen > token.Buffer.Count - 4)
                        {   //长度不够时,退出循环,让程序继续接收  
                            break;
                        }

                        //包够长时,则提取出来,交给后面的程序去处理  
                        byte[] rev = token.Buffer.GetRange(4, packageLen).ToArray();
                        //从数据池中移除这组数据  
                        lock (token.Buffer)
                        {
                            token.Buffer.RemoveRange(0, packageLen + 4);
                        }
                        //将数据包交给后台处理,这里你也可以新开个线程来处理.加快速度.  
                        if (ReceiveClientData != null)
                            ReceiveClientData(token, rev);

                        DataCallBackReady(token, rev);

                        //这里API处理完后,并没有返回结果,当然结果是要返回的,却不是在这里, 这里的代码只管接收.  
                        //若要返回结果,可在API处理中调用此类对象的SendMessage方法,统一打包发送.不要被微软的示例给迷惑了.  
                    } while (token.Buffer.Count > 4);

                    //继续接收. 为什么要这么写,请看Socket.ReceiveAsync方法的说明  
                    if (!token.Socket.ReceiveAsync(e))
                    {
                        this.ProcessReceive(e);
                    }
                }
                else
                {
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

        //关闭客户端  
        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = e.UserToken as AsyncUserToken;

            //调用关闭连接
            OnClose(token);

            RemoveUserToken(token);

            //如果有事件,则调用事件,发送客户端数量变化通知  
            if (ClientNumberChange != null)
                ClientNumberChange(-1, token);
            // close the socket associated with the client  
            try
            {
                token.Socket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception) { }
            token.Socket.Close();
            // decrement the counter keeping track of the total number of clients connected to the server  
            Interlocked.Decrement(ref m_clientCount);
            m_maxNumberAcceptedClients.Release();
            // Free the SocketAsyncEventArg so they can be reused by another client  
            ReleaseSocketAsyncEventArgs(e);
        }

        void ReleaseSocketAsyncEventArgs(SocketAsyncEventArgs saea)
        {
            saea.UserToken = null;//TODO
            saea.SetBuffer(null, 0, 0);
            switch (saea.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    m_Receivepool.Push(saea);
                    break;
                case SocketAsyncOperation.Send:
                    m_Sendpool.Push(saea);
                    break;
            }
        }

        int sendrun = 0;
        public void SendForMsgPool()
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
                        //Console.WriteLine("从信息池取出发送");
                        SendMessage(msg.token, msg.message);
                        msg = null;
                    }
                    catch
                    {
                        Console.WriteLine("==============================================>");
                    }
                }
                sendrun--;
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public void SendMessage(AsyncUserToken token, byte[] message,bool dontNeedHead = false)
        {
            if (token == null || token.Socket == null || !token.Socket.Connected)
                return;
            try
            {
                if (!dontNeedHead)
                {
                    message = SendDataWithHead(message);
                }

                if (m_Sendpool.Count > 0)
                {
                    SocketAsyncEventArgs myreadEventArgs = m_Sendpool.Pop();
                    myreadEventArgs.UserToken = token;
                    myreadEventArgs.AcceptSocket = token.Socket;
                    myreadEventArgs.SetBuffer(message, 0, message.Length);  //将数据放置进去.  

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
                    msg_pool.Enqueue(new TokenWithMsg() { token = token, message = message });
                    //Console.WriteLine("！！！！压入消息发送队列MSG_Pool");
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //拼接长度
        private static byte[] SendDataWithHead(byte[] message)
        {

            MemoryStream memoryStream = new MemoryStream();//创建一个内存流

            byte[] BagHead = BitConverter.GetBytes(message.Length + 4);//往字节数组中写入包头（包头自身的长度和消息体的长度）的长度

            memoryStream.Write(BagHead, 0, BagHead.Length);//将包头写入内存流

            memoryStream.Write(message, 0, message.Length);//将消息体写入内存流

            byte[] HeadAndBody = memoryStream.ToArray();//将内存流中的数据写入字节数组

            memoryStream.Close();//关闭内存
            memoryStream.Dispose();//释放资源

            return HeadAndBody;
        }

        #region

        /// <summary>
        /// 用于调用者回调的虚函数
        /// </summary>
        /// <param name="data"></param>
        public virtual void DataCallBack(AsyncUserToken sk, int CMDID, byte[] data)
        {

        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="sk"></param>
        public virtual void OnClose(AsyncUserToken sk)
        {

        }

        public virtual void OnCloseReady(AsyncUserToken token)
        {
            OnClose(token);

            RemoveUserToken(token);

            //如果有事件,则调用事件,发送客户端数量变化通知  
            if (ClientNumberChange != null)
                ClientNumberChange(-1, token);
            // close the socket associated with the client  
            try
            {
                token.Socket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception) { }
            token.Socket.Close();
            // decrement the counter keeping track of the total number of clients connected to the server  
            Interlocked.Decrement(ref m_clientCount);
            m_maxNumberAcceptedClients.Release();
        }

        /// <summary>
        /// 发送数据并计数
        /// </summary>
        /// <param name="data"></param>
        private void SendWithIndex(AsyncUserToken token, byte[] data)
        {
            try
            {
                //发送数据
                SendMessage(token, data);

                token.SendIndex = MaxSendIndexNum;
            }
            catch
            {
                OnCloseReady(token);
            }
        }

        public AsyncUserToken GetAsyncUserTokenForSocket(Socket sk)
        {
            return _DictSocketAsyncUserToken.ContainsKey(sk) ? _DictSocketAsyncUserToken[sk] : null;
        }

        /// <summary>
        /// 对外暴露的发送消息
        /// </summary>
        /// <param name="CMDID"></param>
        /// <param name="data">序列化之后的数据</param>
        public void SendToSocket(Socket sk, int CMDID, int ERRCODE, byte[] data)
        {
            AsyncUserToken token = GetAsyncUserTokenForSocket(sk);
            HunterNet_S2C _s2cdata = new HunterNet_S2C();
            _s2cdata.HunterNetCoreCmdID = CMDID;
            _s2cdata.HunterNetCoreData = ByteString.CopyFrom(data);
            _s2cdata.HunterNetCoreERRORCode = ERRCODE;
            byte[] _finaldata = Serizlize(_s2cdata);
            SendWithIndex(token, _finaldata);
        }

        /// <summary>
        /// 发送心跳包
        /// </summary>
        /// <param name="sk"></param>
        /// 
        private void SendHeartbeat(AsyncUserToken token)
        {
            if (token == null || token.Socket == null || !token.Socket.Connected)
                return;
            try
            {
                //Console.WriteLine(DateTime.Now.ToString() + "发送心跳包");
                token.SendIndex = MaxSendIndexNum;
                SendMessage(token, HeartbeatData, true);
            }
            catch (Exception e)
            {
                OnCloseReady(token);
            }
        }

        private void DataCallBackReady(AsyncUserToken sk, byte[] data)
        {
            //增加接收计数
            sk.RevIndex = MaxRevIndexNum;

            if (data.Length == 1 && data[0] == 0x00)//心跳包
            {
                //Console.WriteLine("收到心跳包");
                //无处理
            }
            else
            {
                try
                {
                    HunterNet_C2S _s2c = DeSerizlize<HunterNet_C2S>(data);
                    DataCallBack(sk, (int)_s2c.HunterNetCoreCmdID, _s2c.HunterNetCoreData.ToArray());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("数据解析错误");
                }
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
                    OnCloseReady(m_clients[i]);
                    return;
                }

                //发送计数
                m_clients[i].SendIndex--;
                if (m_clients[i].SendIndex <= 0)//需要发送心跳包了
                {
                    //重置倒计时计数
                    m_clients[i].SendIndex = MaxSendIndexNum;
                    SendHeartbeat(m_clients[i]);
                }
            }
        }
        #endregion

        public static byte[] Serizlize(IMessage msg)
        {
            return msg.ToByteArray();
        }

        public static T DeSerizlize<T>(byte[] bytes)
        {
            var msgType = typeof(T);
            object msg = Activator.CreateInstance(msgType);
            ((IMessage)msg).MergeFrom(bytes);
            return (T)msg;
        }
    }
}
