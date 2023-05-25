using ClientCore;
App.Init("127.0.0.1", 23846);

//注册
App.chat.OnChatMsg += OnChatMsg;

while (true)
{
    string CommandStr = Console.ReadLine();
    string Command = "";
    Command = ((CommandStr.IndexOf(" ") <= 0) ? CommandStr : CommandStr.Substring(0, CommandStr.IndexOf(" ")));
    string[] CmdArr = CommandStr.Split(' ');
    switch (Command)
    {
        case "login":
        case "l":
            if (CmdArr.Length < 2)
            {
                Console.WriteLine("缺省用户名");
                return;
            }
            App.login.Login(CmdArr[1]);
            break;
        case "say":
            if (CmdArr.Length < 2)
            {
                Console.WriteLine("缺省参数");
                return;
            }
            App.chat.SendChatMsg(CmdArr[1]);
            break;
        default:
            Console.WriteLine("未知命令" + CommandStr);
            break;
    }
}

void OnChatMsg(string str1, string str2)
{
    Console.WriteLine($"[Chat]{str1}:{str2}");
}