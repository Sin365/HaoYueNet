using System.Net;
using System.Net.Sockets;
using static HaoYueNet.ClientNetwork.BaseData;

namespace HaoYueNet.ClientNetwork.OtherMode
{
    public class NetworkHelperCore_ListenerMode
    {
        private Socket serversocket;
        private Socket client;

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

        public void Init(int port)
        {
            LogOut("==>初始化NetworkHelperCore_ListenerMode");
            serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            serversocket.Bind(endPoint);                 // 绑定
            serversocket.Listen(1);
            client = serversocket.Accept();         // 接收客户端连接
            OnConnected?.Invoke(true);
            Console.WriteLine("客户端连接成功 信息： " + client.AddressFamily.ToString());
            Thread revThread = new Thread(Recive);
            revThread.Start(client);
        }

        ~NetworkHelperCore_ListenerMode()
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
        public void SendToClient(byte[] data)
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

        MemoryStream memoryStream = new MemoryStream();//开辟一个内存流
        private void Recive(object o)
        {
            var client = o as Socket;
            //MemoryStream memoryStream = new MemoryStream();//开辟一个内存流

            while (true)
            {
                byte[] buffer = new byte[1024 * 1024 * 2];
                int effective = 0;
                try
                {
                    effective = client.Receive(buffer);
                    if (effective == 0)
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    //远程主机强迫关闭了一个现有的连接
                    OnCloseReady();
                    return;
                    //断开连接
                }
                memoryStream.Write(buffer, 0, effective);//将接受到的数据写入内存流中
                while (true)
                {
                    if (effective > 0)//如果接受到的消息不为0（不为空）
                    {
                        DataCallBackReady(memoryStream.ToArray());
                        //流复用的方式 不用重新new申请
                        memoryStream.Position = 0;
                        memoryStream.SetLength(0);
                    }
                }
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
