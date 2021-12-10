using System.Net.Sockets;
using System.Timers;

namespace SimpleServer
{
    public class ClientInfo
    {
        public long UID { get; set; }
        public long RID { get; set; }
        public Socket _socket { get; set; }
        public bool IsOffline { get; set; } = false;
        public DateTime LogOutDT { get; set; }
    }

    public class ClientManager
    {
        public List<ClientInfo> ClientList = new List<ClientInfo>();
        public Dictionary<long?, ClientInfo> _DictRIDClient = new Dictionary<long?, ClientInfo>();
        public Dictionary<Socket, ClientInfo> _DictSocketClient = new Dictionary<Socket, ClientInfo>();
        public Dictionary<long?, ClientInfo> _DictUIDClient = new Dictionary<long?, ClientInfo>();
        
        private System.Timers.Timer _ClientCheckTimer;
        private long _RemoveOfflineCacheMin;
        public void Init(long ticktime,long RemoveOfflineCacheMin)
        {
            //换算成毫秒
            _RemoveOfflineCacheMin = RemoveOfflineCacheMin * 1000;
            _ClientCheckTimer = new System.Timers.Timer();
            _ClientCheckTimer.Interval = ticktime;
            _ClientCheckTimer.AutoReset = true;
            _ClientCheckTimer.Elapsed += new ElapsedEventHandler(ClientCheckClearOffline_Elapsed);
            _ClientCheckTimer.Enabled = true;
        }
        
        private void ClientCheckClearOffline_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime CheckDT = DateTime.Now.AddMinutes(-1 * _RemoveOfflineCacheMin);
            ClientInfo[] OfflineClientlist = ClientList.Where(w => w.IsOffline == true && w.LogOutDT < CheckDT).ToArray();

            Console.WriteLine("开始清理离线过久的玩家的缓存");
            for (int i = 0; i < OfflineClientlist.Length; i++)
            {
                //to do 掉线处理
                RemoveClient(OfflineClientlist[i]);
            }
            GC.Collect();
        }
        

        //通用处理
        #region clientlist 处理

        /// <summary>
        /// 增加用户
        /// </summary>
        /// <param name="client"></param>
        public void AddClient(ClientInfo clientInfo)
        {
            try
            {
                Console.WriteLine("追加连接玩家 RID=>" + clientInfo.RID + "  UID=>" + clientInfo.UID);
                lock (ClientList)
                {
                    _DictUIDClient.Add(clientInfo.UID, clientInfo);
                    _DictRIDClient.Add(clientInfo.RID, clientInfo);
                    _DictSocketClient.Add(clientInfo._socket, clientInfo);
                    ClientList.Add(clientInfo);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        
        /// <summary>
        /// 清理连接
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(ClientInfo client)
        {
            lock (ClientList)
            {
                if(_DictUIDClient.ContainsKey(client.UID))
                    _DictUIDClient.Remove(client.UID);

                if (_DictRIDClient.ContainsKey(client.RID))
                    _DictRIDClient.Remove(client.RID);
                
                if (_DictSocketClient.ContainsKey(client._socket))
                    _DictSocketClient.Remove(client._socket);
                
                ClientList.Remove(client);
            }
        }

        public ClientInfo GetClientForRoleID(int RoleID)
        {
            return _DictRIDClient.ContainsKey(RoleID) ? _DictRIDClient[RoleID] : null;
        }

        public ClientInfo GetClientForSocket(Socket sk)
        {
            return _DictSocketClient.ContainsKey(sk) ? _DictSocketClient[sk] : null;
        }

        /// <summary>
        /// 获取在线玩家
        /// </summary>
        /// <returns></returns>
        public List<ClientInfo> GetOnlineClientList()
        {
            return ClientList.Where(w => w.IsOffline == false).ToList();
        }


        /// <summary>
        /// 设置玩家离线
        /// </summary>
        /// <param name="sk"></param>
        public void SetClientOfflineForSocket(Socket sk)
        {
            if (!_DictSocketClient.ContainsKey(sk))
                return;

            Console.WriteLine("标记玩家RID"+ _DictSocketClient[sk].RID+ "为离线");
            _DictSocketClient[sk].IsOffline = true;
            _DictSocketClient[sk].LogOutDT = DateTime.Now;
        }

        public void RemoveClientForSocket(Socket sk)
        {
            if (!_DictSocketClient.ContainsKey(sk))
                return;

            RemoveClient(_DictSocketClient[sk]);
            //ClientList.Remove(GetClientForSocket(sk));
        }

        #endregion

        //public void AddClient_JoinGame(Socket sk)
        //{
        //    var c = new ClientInfo();
        //    c = AutoRoleID;
        //    AddClient(c);
        //}

        /// <summary>
        /// 给一组用户发送数据
        /// </summary>
        /// <param name="_toclientlist"></param>
        /// <param name="CMDID"></param>
        /// <param name="ERRCODE"></param>
        /// <param name="data"></param>
        public void ClientSend(List<ClientInfo> _toclientlist, int CMDID, int ERRCODE, byte[] data)
        {
            for (int i = 0; i < _toclientlist.Count();i++)
            {
                if (_toclientlist[i] == null || _toclientlist[i].IsOffline)
                    continue;
                ServerManager.g_SocketMgr.SendToSocket(_toclientlist[i]._socket, CMDID,ERRCODE,data);
            }
        }

        public void ClientSend(Socket _socket, int CMDID, int ERRCODE, byte[] data)
        {
            //Console.WriteLine("发送数据 CMDID->"+ CMDID);
            ServerManager.g_SocketMgr.SendToSocket(_socket, CMDID, ERRCODE, data);
        }

        /// <summary>
        /// 给一个连接发送数据
        /// </summary>
        /// <param name="_c"></param>
        /// <param name="CMDID"></param>
        /// <param name="ERRCODE"></param>
        /// <param name="data"></param>
        public void ClientSend(ClientInfo _c, int CMDID, int ERRCODE, byte[] data)
        {
            if (_c == null || _c.IsOffline)
                return;
            ServerManager.g_SocketMgr.SendToSocket(_c._socket, CMDID, ERRCODE, data);
        }
    }
}
