using AxibugProtobuf;
using HaoYueNet.ServerNetwork;
using ServerCore.Manager;
using System.Net.Sockets;

namespace ServerCore.NetWork
{
    public class IOCPNetWork : SocketManager
    {
        public IOCPNetWork(int numConnections, int receiveBufferSize)
            : base(numConnections, receiveBufferSize)
        {
            m_clientCount = 0;
            m_maxConnectNum = numConnections;
            m_revBufferSize = receiveBufferSize;
            // allocate buffers such that the maximum number of sockets can have one outstanding read and   
            //write posted to the socket simultaneously    
            m_bufferManager = new BufferManager(receiveBufferSize * numConnections * opsToAlloc, receiveBufferSize);

            m_pool = new SocketEventPool(numConnections);
            m_maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);


            ClientNumberChange += IOCPNetWork_ClientNumberChange;

        }

        private void IOCPNetWork_ClientNumberChange(int num, AsyncUserToken token)
        {
            Console.WriteLine("Client数发生变化");
        }

        /// <summary>
        /// 接受包回调
        /// </summary>
        /// <param name="CMDID">协议ID</param>
        /// <param name="ERRCODE">错误编号</param>
        /// <param name="data">业务数据</param>
        public override void DataCallBack(AsyncUserToken token, int CMDID, byte[] data)
        {
            DataCallBackToOld(token.Socket, CMDID, data);
        }

        public void DataCallBackToOld(Socket sk, int CMDID, byte[] data)
        {
            ServerManager.g_Log.Debug("收到消息 CMDID =>" + CMDID + " 数据长度=>" + data.Length);
            try
            {
                switch ((CommandID)CMDID)
                {
                    case CommandID.CmdLogin: ServerManager.g_Login.UserLogin(sk, data); break;
                    case CommandID.CmdChatmsg: ServerManager.g_Chat.RecvPlayerChatMsg(sk, data); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("逻辑处理错误：" + ex.ToString());
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="sk"></param>
        public override void OnClose(AsyncUserToken token)
        {
            OnCloseToOld(token.Socket);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="sk"></param>
        public void OnCloseToOld(Socket sk)
        {
            Console.WriteLine("断开连接");
            ServerManager.g_ClientMgr.SetClientOfflineForSocket(sk);
        }

    }
}
