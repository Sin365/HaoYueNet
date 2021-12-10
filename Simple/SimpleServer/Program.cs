// See https://aka.ms/new-console-template for more information
using SimpleServer;
using System.Net;

Console.WriteLine("Hello, World!");

ServerManager.g_ClientMgr = new ClientManager();
ServerManager.g_SocketMgr = new IOCPNetWork(1024, 1024);
ServerManager.g_SocketMgr.Init();
ServerManager.g_SocketMgr.Start(new IPEndPoint(IPAddress.Any.Address, 23846));
Console.WriteLine("监听:" + 23846);
Console.WriteLine("Succeed!");
while (true)
{
	string CommandStr = Console.ReadLine();
	string Command = "";
	Command = ((CommandStr.IndexOf(" ") <= 0) ? CommandStr : CommandStr.Substring(0, CommandStr.IndexOf(" ")));
	switch(Command)
	{
		case "list":
			Console.WriteLine("当前在线:" + ServerManager.g_ClientMgr.ClientList.Count());
			break;
		default:
			Console.WriteLine("未知命令"+CommandStr);
			break;
	}
}