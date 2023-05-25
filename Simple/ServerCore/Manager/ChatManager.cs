using AxibugProtobuf;
using ServerCore.Common;
using ServerCore.NetWork;
using System.Net.Sockets;

namespace ServerCore.Manager
{
    public class ChatManager
    {
        public void RecvPlayerChatMsg(Socket sk, byte[] reqData)
        {
            ClientInfo _c = ServerManager.g_ClientMgr.GetClientForSocket(sk);
            ServerManager.g_Log.Debug("收到新的登录请求");
            Protobuf_ChatMsg msg = NetBase.DeSerizlize<Protobuf_ChatMsg>(reqData);
            byte[] respData = NetBase.Serizlize(new Protobuf_ChatMsg_RESP()
            {
                ChatMsg = msg.ChatMsg,
                NickName = _c.Account,
                Date = Helper.GetNowTimeStamp()
            });
            ServerManager.g_ClientMgr.ClientSendALL((int)CommandID.CmdChatmsg, (int)ErrorCode.ErrorOk, respData);
        }
    }
}