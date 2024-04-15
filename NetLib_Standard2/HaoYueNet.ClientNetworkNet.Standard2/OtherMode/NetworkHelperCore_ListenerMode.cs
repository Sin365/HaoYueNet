using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HaoYueNet.ClientNetworkNet.Standard2.OtherMode
{
    public class NetworkHelperCore_ListenerMode
    {
        private Socket serversocket;
        private Dictionary<IntPtr, Socket> mDictHandleClient;

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
            mDictHandleClient = new Dictionary<IntPtr, Socket>();

            LogOut("==>初始化NetworkHelperCore_ListenerMode");
            serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            serversocket.Bind(endPoint);                 // 绑定
            serversocket.Listen(1);
            //client = serversocket.Accept();         // 接收客户端连接
            //OnConnected?.Invoke(true);
            //Console.WriteLine("客户端连接成功 信息： " + client.AddressFamily.ToString());
            //Thread revThread = new Thread(Recive);
            //revThread.Start(client);

            Task task = new Task(() =>
            {
                while (true)
                {
                    Socket newclient;
                    try
                    {
                        newclient = serversocket.Accept();         // 接收客户端连接
                    }
                    catch
                    {
                        break;
                    }
                    AddDictSocket(newclient);
                    OnConnected?.Invoke(newclient);
                    Console.WriteLine("客户端连接成功 信息： " + newclient.AddressFamily.ToString());
                    Thread revThread = new Thread(Recive);
                    revThread.Start(newclient);
                }
            });
            task.Start();
        }

#region

        /// <summary>
        /// 追加Socket返回下标
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public void AddDictSocket(Socket socket)
        {
            if (socket == null)
                return;
            lock (mDictHandleClient)
            {
                mDictHandleClient[socket.Handle] = socket;
            }
        }

        public void RemoveDictSocket(Socket socket)
        {
            if (socket == null)
                return;
            lock (mDictHandleClient)
            {
                if (!mDictHandleClient.ContainsKey(socket.Handle))
                    return;
                mDictHandleClient.Remove(socket.Handle);
            }
        }
#endregion

        ~NetworkHelperCore_ListenerMode()
        {
            IntPtr[] keys = mDictHandleClient.Keys.ToArray();
            for (uint i = 0; i < keys.Length; i++) 
            {
                mDictHandleClient[keys[i]].Close();
            }
            mDictHandleClient.Clear();
        }

        private void SendToSocket(Socket socket, byte[] data)
        {
            //已拼接包长度，这里不再需要拼接长度
            //data = SendDataWithHead(data);
            try
            {
                SendWithIndex(socket,data);
            }
            catch (Exception ex)
            {
                //连接断开
                OnCloseReady(socket);
                return;
            }
            //LogOut("发送消息，消息长度=> "+data.Length);
        }

        /// <summary>
        /// 发送数据并计数
        /// </summary>
        /// <param name="data"></param>
        private void SendWithIndex(Socket socket,byte[] data)
        {
            //增加发送计数
            SendIndex = MaxSendIndexNum;
            //发送数据
            socket.Send(data);
        }

        /// <summary>
        /// 供外部调用 发送消息
        /// </summary>
        /// <param name="CMDID"></param>
        /// <param name="data">序列化之后的数据</param>
        public void SendToClient(Socket socket, byte[] data)
        {
            //LogOut("准备数据 data=> "+data);
            SendToSocket(socket, data);
        }

#region 事件定义
        public delegate void OnConnectedHandler(Socket socket);

        public delegate void OnReceiveDataHandler(Socket sk, byte[] data);

        public delegate void OnDisconnectHandler(Socket sk);

        public delegate void OnNetLogHandler(string msg);
#endregion

        public event OnConnectedHandler OnConnected;

        public event OnReceiveDataHandler OnReceive;

        public event OnDisconnectHandler OnDisconnected;

        public event OnNetLogHandler OnNetLog;

        /// <summary>
        /// 做好处理的连接管理
        /// </summary>
        private void OnCloseReady(Socket socket)
        {
            LogOut("关闭连接");
            //关闭Socket连接
            socket.Close();
            RemoveDictSocket(socket);
            OnDisconnected?.Invoke(socket);
        }

        /// <summary>
        /// 主动关闭连接
        /// </summary>
        public void CloseConntect(Socket socket)
        {
            OnCloseReady(socket);
        }

        private void DataCallBackReady(Socket socket,byte[] data)
        {
            //增加接收计数
            RevIndex = MaxRevIndexNum;
            OnReceive(socket,data);
        }

        private void Recive(object o)
        {
            MemoryStream memoryStream = new MemoryStream();//开辟一个内存流
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
                    OnCloseReady(client);
                    return;
                    //断开连接
                }
                if (effective > 0)//如果接受到的消息不为0（不为空）
                {
                    memoryStream.Write(buffer, 0, effective);//将接受到的数据写入内存流中
                    DataCallBackReady(client, memoryStream.ToArray());
                    //流复用的方式 不用重新new申请
                    memoryStream.Position = 0;
                    memoryStream.SetLength(0);
                }
            }
        }

        public void LogOut(string Msg)
        {
            //Console.WriteLine(Msg);
            OnNetLog?.Invoke(Msg);
        }

    }
}
