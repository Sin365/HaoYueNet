﻿using Google.Protobuf;
using HunterProtobufCore;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace HaoYueNet.ClientNetworkNet4x
{
    public class NetworkHelperCore
    {
        private Socket client;

        /// <summary>
        /// 心跳包数据
        /// </summary>
        private byte[] HeartbeatData = new byte[5] { 0x05, 0x00, 0x00, 0x00, 0x00 };

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

        public void Init(string IP, int port)
        {

            LogOut("==>初始化网络核心");

            RevIndex = MaxRevIndexNum;
            SendIndex = MaxSendIndexNum;

            client = new Socket(SocketType.Stream, ProtocolType.Tcp);
            //IPAddress ip = IPAddress.Parse(IP);
            //IPEndPoint point = new IPEndPoint(ip, port);

            LogOut("连接到IP "+ IP + ":"+ port);

            //带回调的
            //client.BeginConnect(ip, port, new AsyncCallback(CallBackMethod) , client);
            try
            {
                client.Connect(IP, port);
                Thread thread = new Thread(Recive);
                thread.IsBackground = true;
                thread.Start(client);
                LogOut("连接成功!");

                _heartTimer = new System.Timers.Timer();
                _heartTimer.Interval = TimerInterval;
                _heartTimer.Elapsed += CheckUpdatetimer_Elapsed;
                _heartTimer.AutoReset = true;
                _heartTimer.Enabled = true;
                LogOut("开启心跳包检测");

                OnConnected(true);
            }
            catch
            {
                OnConnected(false);
            }
        }

        ~NetworkHelperCore()
        {
            client.Close();
        }

        private void SendToSocket(byte[] data)
        {
            data = SendDataWithHead(data);
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
            LogOut("发送消息，消息长度=> "+data.Length);
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
        /// <param name="data">序列化之后的数据</param>
        public void SendToServer(int CMDID,byte[] data)
        {
            LogOut("准备数据 CMDID=> "+CMDID);
            HunterNet_C2S _c2sdata = new HunterNet_C2S();
            _c2sdata.HunterNetCoreCmdID = CMDID;
            _c2sdata.HunterNetCoreData = ByteString.CopyFrom(data);
            byte[] _finaldata = Serizlize(_c2sdata);
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
            
            HunterNet_S2C _c2s = DeSerizlize<HunterNet_S2C>(data);
            
            OnDataCallBack(_c2s.HunterNetCoreCmdID, _c2s.HunterNetCoreERRORCode, _c2s.HunterNetCoreData.ToArray());
        }

        private void Recive(object o)
        {
            var client = o as Socket;
            MemoryStream memoryStream = new MemoryStream();//开辟一个内存流

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
                            memoryStream.Close();//关闭内存流
                            memoryStream.Dispose();//释放内存资源
                            memoryStream = new MemoryStream();//创建新的内存流
                            memoryStream.Write(getData, StartIndex, getData.Length - StartIndex);//从新将接受的消息写入内存流
                            break;
                        }
                        else
                        {
                            //把头去掉，就可以吃了，蛋白质是牛肉的六倍
                            DataCallBackReady(getData.Skip(StartIndex+4).Take(HeadLength-4).ToArray());
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
