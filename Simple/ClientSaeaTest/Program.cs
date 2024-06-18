using System.Net.Sockets;

namespace ClientSaeaTest
{
	internal class Program
	{
		static SaeaClient client = new SaeaClient(1024,1024);
		static void Main(string[] args)
		{
			client.Init();
			client.Start();
			Socket socket =  new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 
			client.Connect("139.186.160.243",1000);

			Console.WriteLine("Hello, World!");
			Console.ReadLine();
		}
	}
}
