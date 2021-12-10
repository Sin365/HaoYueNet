// See https://aka.ms/new-console-template for more information

using SimpleClient;

Console.WriteLine("Hello, World!");

StaticComm.networkHelper = new NetworkHelper();

StaticComm.networkHelper.Init("127.0.0.1", 23846);

while (true)
{
	string CommandStr = Console.ReadLine();
	string Command = "";
	Command = ((CommandStr.IndexOf(" ") <= 0) ? CommandStr : CommandStr.Substring(0, CommandStr.IndexOf(" ")));
	switch (Command)
	{
		default:
			Console.WriteLine("未知命令" + CommandStr);
			break;
	}
}