using AxibugProtobuf;
using HaoYueNet.ClientNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCore.Network
{
    /// <summary>
    /// 继承网络库，以支持网络功能
    /// </summary>
    public class NetworkHelper : NetworkHelperCore
    {
        public NetworkHelper()
        {
            //指定接收服务器数据事件
            OnReceiveData += GetDataCallBack;
            //断开连接
            OnClose += OnConnectClose;
            OnConnected += NetworkConnected;
            //网络库调试信息输出事件，用于打印网络内容
            OnLogOut += NetworkDeBugLog;
        }

        public void NetworkConnected(bool IsConnect)
        {
            if (IsConnect)
                NetworkDeBugLog("服务器连接成功");
            else
            {
                NetworkDeBugLog("服务器连接失败");
                //to do 重连逻辑
            }
        }

        public void NetworkDeBugLog(string str)
        {
            //用于Unity内的输出
            //Debug.Log("NetCoreDebug >> "+str);

            Console.WriteLine("NetCoreDebug >> " + str);
        }

        /// <summary>
        /// 接受包回调
        /// </summary>
        /// <param name="CMDID">协议ID</param>
        /// <param name="ERRCODE">错误编号</param>
        /// <param name="data">业务数据</param>
        public void GetDataCallBack(int CMDID, int ERRCODE, byte[] data)
        {
            NetworkDeBugLog("收到消息 CMDID =>" + CMDID + " ERRCODE =>" + ERRCODE + " 数据长度=>" + data.Length);
            try
            {
                //抛出网络数据
                NetMsg.Instance.PostNetMsgEvent(CMDID, data);
            }
            catch (Exception ex)
            {
                NetworkDeBugLog("逻辑处理错误：" + ex.ToString());
            }

        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void OnConnectClose()
        {
            NetworkDeBugLog("OnConnectClose");
        }
    }
}
