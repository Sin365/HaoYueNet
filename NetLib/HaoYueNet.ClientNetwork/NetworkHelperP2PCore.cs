using System.Net;
using System.Net.Sockets;
using static HaoYueNet.ClientNetwork.BaseData;

namespace HaoYueNet.ClientNetwork
{
    public class NetworkHelperP2PCore
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
        private static int RevIndex=0;
        //发送倒计时计数
        private static int SendIndex=0;

        //计时器间隔
        private static int TimerInterval = 3000;

        private System.Timers.Timer _heartTimer;

        public void Init(string IP, int port, bool bBindReuseAddress = false,int bBindport = 0)
        {

            LogOut("==>初始化网络核心");

            RevIndex = MaxRevIndexNum;
            SendIndex = MaxSendIndexNum;

            client = new Socket(SocketType.Stream, ProtocolType.Tcp);
            if (bBindReuseAddress)
            {
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                IPEndPoint ipe = new IPEndPoint(IPAddress.Any, Convert.ToInt32(bBindport));
                client.Bind(ipe);
            }
            Connect(IP, port);
        }

        public bool Connect(string IP, int port)
        {
            //带回调的
            try
            {
                LogOut("连接到远程IP " + IP + ":" + port);
                client.Connect(IP, port);
                Thread thread = new Thread(Recive);
                thread.IsBackground = true;
                thread.Start(client);
                int localport = ((IPEndPoint)client.LocalEndPoint).Port;
                LogOut($"连接成功!连接到远程IP->{IP}:{port} | 本地端口->{localport}");

                if (_heartTimer == null)
                {
                    _heartTimer = new System.Timers.Timer();
                }
                _heartTimer.Interval = TimerInterval;
                _heartTimer.Elapsed += CheckUpdatetimer_Elapsed;
                _heartTimer.AutoReset = true;
                _heartTimer.Enabled = true;
                LogOut("开启心跳包检测");

                OnConnected(true);
                return true;
            }
            catch (Exception ex)
            {
                LogOut("连接失败：" + ex.ToString());
                OnConnected(false);
                return false;
            }
        }

        ~NetworkHelperP2PCore()
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
                SendWithIndex(BaseData.HeartbeatData);
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

        //拼接头长度
        private byte[] SendDataWithHead(byte[] message)
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

        /// <summary>
        /// 供外部调用 发送消息
        /// </summary>
        /// <param name="CMDID"></param>
        /// <param name="ERRCODE"></param>
        /// <param name="data"></param>
        public void SendToSocket(int CMDID, int ERRCODE, byte[] data)
        {
            //LogOut("准备数据 CMDID=> "+CMDID);
            /*HunterNet_S2C _s2sdata = new HunterNet_S2C();
            _s2sdata.HunterNetCoreCmdID = CMDID;
            _s2sdata.HunterNetCoreERRORCode = ERRCODE;
            _s2sdata.HunterNetCoreData = ByteString.CopyFrom(data);
            byte[] _finaldata = Serizlize(_s2sdata);*/

            byte[] _finaldata = HunterNet_S2C.CreatePkgData((ushort)CMDID, (ushort)ERRCODE, data);
            SendToSocket(_finaldata);
        }

        public delegate void OnDataCallBack_Data(int CMDID, int ERRCODE, byte[] data);

        public event OnDataCallBack_Data OnDataCallBack;

        public delegate void delegate_NoData();

        public delegate void delegate_Bool(bool IsConnected);

        public event delegate_NoData OnClose;

        public event delegate_Bool OnConnected;

        public delegate void delegate_str(string Msg);

        /// <summary>
        /// 网络库调试日志输出
        /// </summary>
        public event delegate_str OnLogOut;

        ///// <summary>
        ///// 用于调用者回调的虚函数
        ///// </summary>
        ///// <param name="data"></param>
        //public virtual void DataCallBack(int CMDID,int ERRCODE,byte[] data)
        //{

        //}

        ///// <summary>
        ///// 断开连接
        ///// </summary>
        ///// <param name="sk"></param>
        //public virtual void OnClose()
        //{

        //}

        /// <summary>
        /// 做好处理的连接管理
        /// </summary>
        private void OnCloseReady()
        {
            LogOut("关闭心跳包计数");
            _heartTimer.Enabled = false;
            _heartTimer.Elapsed -= CheckUpdatetimer_Elapsed;
            LogOut("关闭连接");
            //关闭Socket连接
            client.Close();
            OnClose();
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

            //不处理心跳包
            if (data.Length == 1 && data[0] == 0x00)
            {
                //LogOut("收到心跳包");
                return;
            }

            /*
            HunterNet_S2C _c2s = DeSerizlize<HunterNet_S2C>(data);
            OnDataCallBack(_c2s.HunterNetCoreCmdID, _c2s.HunterNetCoreERRORCode, _c2s.HunterNetCoreData.ToArray());
            */
            HunterNet_S2C.AnalysisPkgData(data, out ushort CmdID, out ushort Error, out byte[] resultdata);
            OnDataCallBack(CmdID, Error, resultdata);
        }

        MemoryStream memoryStream = new MemoryStream();//开辟一个内存流
        private void Recive(object o)
        {
            var client = o as Socket;
            //MemoryStream memoryStream = new MemoryStream();//开辟一个内存流

            while (true)
            {
                byte[] buffer = new byte[1024 * 1024 * 2];
                int effective=0;
                try
                {
                    effective = client.Receive(buffer);
                    if (effective == 0)
                    {
                        continue;
                    }
                }
                catch(Exception ex)
                {
                    //远程主机强迫关闭了一个现有的连接
                    OnCloseReady();
                    return;
                    //断开连接
                }


                memoryStream.Write(buffer, 0, effective);//将接受到的数据写入内存流中
                byte[] getData = memoryStream.ToArray();//将内存流中的消息体写入字节数组
                int StartIndex = 0;//设置一个读取数据的起始下标

                while (true)
                {


                    if (effective > 0)//如果接受到的消息不为0（不为空）
                    {
                        int HeadLength = 0;//包头长度（包头+包体）
                        if (getData.Length - StartIndex < 4)//包头接受不完整
                        {
                            HeadLength = -1;
                        }
                        else
                        {
                            //如果包头接受完整  转换成int类型的数值
                            HeadLength = BitConverter.ToInt32(getData, StartIndex);
                        }
                        //包头接受完整但是消息体不完整              //包头接受不完整
                        //↓↓↓↓↓↓↓↓                            ↓↓↓
                        if (getData.Length - StartIndex < HeadLength || HeadLength == -1)
                        {
                            /* 一种清空流的方式
                            memoryStream.Close();//关闭内存流
                            memoryStream.Dispose();//释放内存资源
                            memoryStream = new MemoryStream();//创建新的内存流
                            */

                            //流复用的方式 不用重新new申请
                            memoryStream.Position = 0;
                            memoryStream.SetLength(0);

                            memoryStream.Write(getData, StartIndex, getData.Length - StartIndex);//从新将接受的消息写入内存流
                            break;
                        }
                        else
                        {
                            //把头去掉，就可以吃了，蛋白质是牛肉的六倍
                            //DataCallBackReady(getData.Skip(StartIndex+4).Take(HeadLength-4).ToArray());

                            int CoreLenght = HeadLength - 4;
                            //改为Array.Copy 提升效率
                            //byte[] retData = new byte[CoreLenght];
                            //Array.Copy(getData, StartIndex + 4, retData, 0, CoreLenght);
                            //DataCallBackReady(retData);

                            //用Span
                            Span<byte> getData_span = getData;
                            getData_span = getData_span.Slice(StartIndex + 4, CoreLenght);
                            DataCallBackReady(getData_span.ToArray());

                            StartIndex += HeadLength;//当读取一条完整的数据后，读取数据的起始下标应为当前接受到的消息体的长度（当前数据的尾部或下一条消息的首部）
                        }
                    }
                }
            }
        }

        private void CheckUpdatetimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //接收服务器数据计数
            RevIndex--;
            if (RevIndex <= 0)
            {
                //判定掉线
                OnCloseReady();
                return;
            }

            //发送计数
            SendIndex--;
            if (SendIndex <= 0)//需要发送心跳包了
            {
                //重置倒计时计数
                SendIndex = MaxSendIndexNum;

                SendHeartbeat();
            }
        }

        public void LogOut(string Msg)
        {
            //Console.WriteLine(Msg);
            OnLogOut(Msg);
        }

        public Socket GetClientSocket()
        {
            return client;
        }
    }
}
