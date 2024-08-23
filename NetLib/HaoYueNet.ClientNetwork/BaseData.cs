using System.Net.Sockets;
namespace HaoYueNet.ClientNetwork
{
    public static class BaseData
    {
        /// <summary>
        /// 心跳包数据
        /// </summary>
        public static byte[] HeartbeatData = new byte[5] { 0x05, 0x00, 0x00, 0x00, 0x00 };
        public static class HunterNet_Heartbeat
        {
            public static void SetDataToSocketAsyncEventArgs(SocketAsyncEventArgs myreadEventArgs)
            {
                myreadEventArgs.SetBuffer(HeartbeatData, 0, HeartbeatData.Length);
            }
        }
        public static class HunterNet_S2C
        {
            public static void SetDataToSocketAsyncEventArgs(SocketAsyncEventArgs myreadEventArgs, UInt16 CmdID, UInt16 Error, byte[] AddonBytes_Data)
            {
                myreadEventArgs.SetBuffer(CreatePkgData(CmdID, Error, AddonBytes_Data));
            }
            public static byte[] CreatePkgData(UInt16 CmdID, UInt16 Error, byte[] AddonBytes_Data)
            {
                //包长度
                int AllLenght = 4 + 2 + 2 + AddonBytes_Data.Length;
                byte[] BufferData = new byte[AllLenght];
                //包长度
                Buffer.BlockCopy(BitConverter.GetBytes(AllLenght), 0, BufferData, 0, sizeof(int));
                //CMDID
                Buffer.BlockCopy(BitConverter.GetBytes(CmdID), 0, BufferData, 4, sizeof(UInt16));
                //ErrID
                Buffer.BlockCopy(BitConverter.GetBytes(Error), 0, BufferData, 4 + 2, sizeof(UInt16));
                //DATA
                Buffer.BlockCopy(AddonBytes_Data, 0, BufferData, 4 + 2 + 2, AddonBytes_Data.Length);
                return BufferData;
            }
            public static void AnalysisPkgData(Span<byte> srcdata, out UInt16 CmdID, out UInt16 Error, out byte[] data)
            {
                CmdID = BitConverter.ToUInt16(srcdata.Slice(0, 2));
                Error = BitConverter.ToUInt16(srcdata.Slice(2, 2));
                data = srcdata.Slice(2 + 2).ToArray();
            }
        }
        public static class HunterNet_C2S
        {
            public static void SetDataToSocketAsyncEventArgs(SocketAsyncEventArgs myreadEventArgs, UInt16 CmdID, byte[] AddonBytes_Data)
            {
                myreadEventArgs.SetBuffer(CreatePkgData(CmdID, AddonBytes_Data));
            }
            public static byte[] CreatePkgData(UInt16 CmdID, byte[] AddonBytes_Data)
            {
                byte[] AddonBytes_CmdID = BitConverter.GetBytes(CmdID);
                int AllLenght = AddonBytes_CmdID.Length + AddonBytes_Data.Length + 4;
                //包长度
                byte[] AddonBytes_Lenght = BitConverter.GetBytes(AllLenght);
                byte[] BufferData = new byte[AllLenght];
                //包长度
                Buffer.BlockCopy(AddonBytes_Lenght, 0, BufferData, 0, AddonBytes_Lenght.Length);
                //CMDID
                Buffer.BlockCopy(AddonBytes_CmdID, 0, BufferData, 4, AddonBytes_CmdID.Length);
                //DATA
                Buffer.BlockCopy(AddonBytes_Data, 0, BufferData, 4 + 2, AddonBytes_Data.Length);
                return BufferData;
            }

            public static void AnalysisPkgData(Span<byte> srcdata, out UInt16 CmdID, out byte[] data)
            {
                CmdID = BitConverter.ToUInt16(srcdata.Slice(0, 2));
                data = srcdata.Slice(2).ToArray();
            }
        }
    }
}
