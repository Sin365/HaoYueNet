using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientCore.Manager;
using ClientCore.Network;

namespace ClientCore
{
    public class App
    {
        public static string TokenStr;
        public static long RID = -1;
        public static string IP;
        public static int Port;
        public static NetworkHelper networkHelper;
        public static AppLogin login;
        public static AppChat chat;

        public static void Init(string IP, int port)
        {
            networkHelper = new NetworkHelper();
            login = new AppLogin();
            chat = new AppChat();
            networkHelper.Init(IP, port);
        }
    }


}