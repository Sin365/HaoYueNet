using System.Net;
using System.Net.Sockets;
using static HaoYueNet.ClientNetwork.BaseData;

namespace HaoYueNet.ClientNetwork.OtherMode
{
    public class NetworkHelperCore_SourceMode
    {
        private Socket client;

        ////响应倒计时计数最大值
        //private static int MaxRevIndexNum = 6;

        ////发送倒计时计数最大值
        //private static int MaxSendIndexNum = 3;

        //响应倒计时计数最大值
        private static int MaxRevIndexNum = 50;

        //发送倒计时计数最大值
        private static int MaxSendIndexNum = 3;

        //响应倒计时计数
        private static int RevIndex = 0;
        //发送倒计时计数
        private static int SendIndex = 0;

        //计时器间隔
        private static int TimerInterval = 3000;

        public static string LastConnectIP;
        public static int LastConnectPort;
        public bool bDetailedLog = false;

        public bool Init(string IP, int port, bool isHadDetailedLog = true, bool bBindReuseAddress = false, int bBindport = 0)
        {
            LogOut("==>初始化网络核心");

            bDetailedLog = isHadDetailedLog;
            RevIndex = MaxRevIndexNum;
            SendIndex = MaxSendIndexNum;

            client = new Socket(SocketType.Stream, ProtocolType.Tcp);
            if (bBindReuseAddress)
            {
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                IPEndPoint ipe = new IPEndPoint(IPAddress.Any, Convert.ToInt32(bBindport));
                client.Bind(ipe);
            }
            LastConnectIP = IP;
            LastConnectPort = port;
            return Connect(IP, port);
        }

        bool Connect(string IP, int port)
        {
            //带回调的
            try
            {
                if (bDetailedLog)
                    LogOut("连接到远程IP " + IP + ":" + port);
                else
                    LogOut("连接到远程服务");

                client.Connect(IP, port);
                Thread thread = new Thread(Recive);
                thread.IsBackground = true;
                thread.Start(client);
                int localport = ((IPEndPoint)client.LocalEndPoint).Port;

                if (bDetailedLog)
                    LogOut($"连接成功!连接到远程IP->{IP}:{port} | 本地端口->{localport}");
                else
                    LogOut("连接成功!");

                if (bDetailedLog)
                    LogOut("开启心跳包检测");

                OnConnected?.Invoke(true);
                return true;
            }
            catch (Exception ex)
            {
                if (bDetailedLog)
                    LogOut("连接失败：" + ex.ToString());
                else
                    LogOut("连接失败");

                OnConnected?.Invoke(false);
                return false;
            }
        }

        ~NetworkHelperCore_SourceMode()
        {
            client.Close();
        }

        private void SendToSocket(byte[] data)
        {
            //已拼接包长度，这里不再需要拼接长度
            //data = SendDataWithHead(data);
            try
            {
                SendWithIndex(data);
            }
            catch (Exception ex)
            {
                //连接断开
                OnCloseReady();
                return;
            }
            //LogOut("发送消息，消息长度=> "+data.Length);
        }

        private void SendHeartbeat()
        {
            try
            {
                SendWithIndex(HeartbeatData);
            }
            catch (Exception ex)
            {
                //连接断开
                OnCloseReady();
                return;
            }
            //LogOut("发送心跳包");
        }

        /// <summary>
        /// 发送数据并计数
        /// </summary>
        /// <param name="data"></param>
        private void SendWithIndex(byte[] data)
        {
            //增加发送计数
            SendIndex = MaxSendIndexNum;
            //发送数据
            client.Send(data);
        }

        /// <summary>
        /// 供外部调用 发送消息
        /// </summary>
        /// <param name="CMDID"></param>
        /// <param name="data">序列化之后的数据</param>
        public void SendToServer(byte[] data)
        {
            //LogOut("准备数据 data=> "+data);
            SendToSocket(data);
        }

        #region 事件定义
        public delegate void OnReceiveDataHandler(byte[] data);
        public delegate void OnConnectedHandler(bool IsConnected);
        public delegate void OnCloseHandler();
        public delegate void OnLogOutHandler(string Msg);
        #endregion

        public event OnConnectedHandler OnConnected;
        public event OnReceiveDataHandler OnReceiveData;
        public event OnCloseHandler OnClose;
        /// <summary>
        /// 网络库调试日志输出
        /// </summary>
        public event OnLogOutHandler OnLogOut;

        /// <summary>
        /// 做好处理的连接管理
        /// </summary>
        private void OnCloseReady()
        {
            LogOut("关闭连接");
            //关闭Socket连接
            client.Close();
            OnClose?.Invoke();
        }

        /// <summary>
        /// 主动关闭连接
        /// </summary>
        public void CloseConntect()
        {
            OnCloseReady();
        }

        private void DataCallBackReady(byte[] data)
        {
            //增加接收计数
            RevIndex = MaxRevIndexNum;
            OnReceiveData(data);
        }

        MemoryStream reciveMemoryStream = new MemoryStream();//开辟一个内存流
        byte[] reciveBuffer = new byte[1024 * 1024 * 2];
        private void Recive(object o)
        {
            var client = o as Socket;
            while (true)
            {
                int effective = 0;
                try
                {
                    effective = client.Receive(reciveBuffer);
                    if (effective == 0)//为0表示已经断开连接，放到后面处理
                    {
                        //尝试性，清理数据
                        reciveMemoryStream.SetLength(0);
                        reciveMemoryStream.Seek(0, SeekOrigin.Begin);
                        //远程主机强迫关闭了一个现有的连接
                        OnCloseReady();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    //尝试性，清理数据
                    reciveMemoryStream.SetLength(0);
                    reciveMemoryStream.Seek(0, SeekOrigin.Begin);

                    //断开连接
                    //远程主机强迫关闭了一个现有的连接
                    OnCloseReady();
                    return;
                }

                reciveMemoryStream.Write(reciveBuffer, 0, effective);//将接受到的数据写入内存流中
                DataCallBackReady(reciveMemoryStream.ToArray());
                //流复用的方式 不用重新new申请
                reciveMemoryStream.Position = 0;
                reciveMemoryStream.SetLength(0);
            }
        }


        public void LogOut(string Msg)
        {
            //Console.WriteLine(Msg);
            OnLogOut?.Invoke(Msg);
        }

        public Socket GetClientSocket()
        {
            return client;
        }
    }
}
