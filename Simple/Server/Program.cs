using ServerCore;
using ServerCore.Manager;

ServerManager.InitServer(23846);

while (true)
{
    string CommandStr = Console.ReadLine();
    string Command = "";
    Command = ((CommandStr.IndexOf(" ") <= 0) ? CommandStr : CommandStr.Substring(0, CommandStr.IndexOf(" ")));
    switch (Command)
    {
        case "list":
            Console.WriteLine("当前在线:" + ServerManager.g_ClientMgr.GetOnlineClient());
            break;
        default:
            Console.WriteLine("未知命令" + CommandStr);
            break;
    }
}