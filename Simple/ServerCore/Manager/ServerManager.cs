﻿using ServerCore.NetWork;
using System.Net;

namespace ServerCore.Manager
{
    public static class ServerManager
    {
        public static ClientManager g_ClientMgr;
        public static LogManager g_Log;
        public static LoginManager g_Login;
        public static ChatManager g_Chat;
        public static IOCPNetWork g_SocketMgr;

        public static void InitServer(int port)
        {
            g_ClientMgr = new ClientManager();
            g_ClientMgr.Init(45000, 120);
            g_Log = new LogManager();
            g_Login = new LoginManager();
            g_Chat = new ChatManager();
            g_SocketMgr = new IOCPNetWork(1024, 1024);
            g_SocketMgr.Init();
            g_SocketMgr.Start(new IPEndPoint(IPAddress.Any.Address, port));
            Console.WriteLine("Succeed!");
        }
    }
}