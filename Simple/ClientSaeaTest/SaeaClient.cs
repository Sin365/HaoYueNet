using HaoYueNet.ClientNetwork.IOCPMode;
using System.Net.Sockets;

namespace ClientSaeaTest
{
    internal class SaeaClient : TcpSaeaClient
	{
		public SaeaClient(int numConnections, int receiveBufferSize)
			: base(numConnections, receiveBufferSize)
		{
			OnClientNumberChange += ClientNumberChange;
			OnReceive += ReceiveData;
			OnDisconnected += OnDisconnect;
			OnNetLog += OnShowNetLog;
		}


		private void ClientNumberChange(int num, AsyncUserToken token)
		{
			Console.WriteLine("Client数发生变化");
		}

		public void Connect(string ip, int port)
		{
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			base.StartConnect(ip, port, socket);
		}

		/// <summary>
		/// 接受包回调
		/// </summary>
		/// <param name="CMDID">协议ID</param>
		/// <param name="ERRCODE">错误编号</param>
		/// <param name="data">业务数据</param>
		private void ReceiveData(AsyncUserToken token, int CMDID,int Error, byte[] data)
		{
			DataCallBack(token.Socket, CMDID, Error,data);
		}

		public void DataCallBack(Socket sk, int CMDID, int Error, byte[] data)
		{
			Console.WriteLine("收到消息 CMDID =>" + CMDID + " 数据长度=>" + data.Length);
			try
			{
				//抛出网络数据
				//NetMsg.Instance.PostNetMsgEvent(CMDID, sk, data);
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
		public void OnDisconnect(AsyncUserToken token)
		{
			Console.WriteLine("断开连接");
			//ServerManager.g_ClientMgr.SetClientOfflineForSocket(token.Socket);
		}

		public void OnShowNetLog(string msg)
		{
			//ServerManager.g_Log.Debug(msg);
		}
	}
}
