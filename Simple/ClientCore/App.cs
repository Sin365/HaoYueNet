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
        public static LogManager log;
        public static NetworkHelper networkHelper;
        public static AppLogin login;
        public static AppChat chat;
        public static UserDataManager user;

        public static void Init(string IP, int port)
        {
            log = new LogManager();
            networkHelper = new NetworkHelper();
            login = new AppLogin();
            chat = new AppChat();
            user = new UserDataManager();
            networkHelper.Init(IP, port);
        }
    }


}