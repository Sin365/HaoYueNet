using AxibugProtobuf;
using ClientCore.Network;
using System.Net.Sockets;
using static ClientCore.Event.DelegateClass;
using static HaoYueNet.ClientNetwork.NetworkHelperCore;

namespace ClientCore.Manager
{
    public class AppChat
    {
        public event dg_Str_Str OnChatMsg;

        public void SendChatMsg(string ChatMsg)
        {
            Protobuf_ChatMsg msg = new Protobuf_ChatMsg()
            {
                ChatMsg = ChatMsg,
            };
            App.networkHelper.SendToServer((int)CommandID.CmdChatmsg, NetBase.Serizlize(msg));
        }

        public void RecvChatMsg(byte[] reqData)
        {
            Protobuf_ChatMsg_RESP msg = NetBase.DeSerizlize<Protobuf_ChatMsg_RESP>(reqData);
            OnChatMsg(msg.NickName, msg.ChatMsg);
        }
    }
}
