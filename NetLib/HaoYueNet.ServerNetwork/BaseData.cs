using System.Net.Sockets;

namespace HaoYueNet.ServerNetwork
{
    public static class BaseData
    {
        /// <summary>
        /// 心跳包数据
        /// </summary>
        public static byte[] HeartbeatData = new byte[5] { 0x05, 0x00, 0x00, 0x00, 0x00 };

        public static void writeInt(byte[] buf, int offset, int value)
        {
            buf[offset++] = (byte)(255 & value);
            buf[offset++] = (byte)(255 & value >> 8);
            buf[offset++] = (byte)(255 & value >> 16);
            buf[offset++] = (byte)(255 & value >>> 24);
        }

        public static void writeUInt16(byte[] buf, int offset, int value)
        {
            buf[offset++] = (byte)(255 & value);
            buf[offset++] = (byte)(255 & value >> 8);
        }

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
                //用Buffer.BlockCopy拷贝
                //包长度
                int AllLenght = 4 + 2 + 2 + AddonBytes_Data.Length;
                byte[] BufferData = new byte[AllLenght];

                //包长度
                writeInt(BufferData, 0, AllLenght);

                //CMDID
                writeUInt16(BufferData, 4, CmdID);

                //Error
                writeUInt16(BufferData, 4 + 2, Error);

                //DATA
                Buffer.BlockCopy(AddonBytes_Data, 0, BufferData, 4 + 2 + 2, AddonBytes_Data.Length);

                return BufferData;
            }

            public static void AnalysisPkgData(Span<byte> srcdata, out UInt16 CmdID, out UInt16 Error, out byte[] data)
            {
                //CmdID = BitConverter.ToUInt16(srcdata, 0);
                //Error = BitConverter.ToUInt16(srcdata, 2);
                //data = new byte[srcdata.Length - 2 - 2];
                //Array.Copy(srcdata, 4, data, 0, data.Length);

                //CmdID = BitConverter.ToUInt16(srcdata, 0);
                //Error = BitConverter.ToUInt16(srcdata, 2);
                //Span<byte> span_srcdata = srcdata;
                //data = span_srcdata.Slice(2 + 2).ToArray();

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
                //byte[] AddonBytes_CmdID = BitConverter.GetBytes(CmdID);
                //int AllLenght = AddonBytes_CmdID.Length + AddonBytes_Data.Length + 4;
                //int LastIndex = 0;
                ////包长度
                //byte[] AddonBytes_Lenght = BitConverter.GetBytes(AllLenght);

                //byte[] BufferData = new byte[AllLenght];

                ////包长度
                //AddonBytes_Lenght.CopyTo(BufferData, LastIndex);
                //LastIndex += AddonBytes_Lenght.Length;

                ////CMDID
                //AddonBytes_CmdID.CopyTo(BufferData, LastIndex);
                //LastIndex += AddonBytes_CmdID.Length;

                ////DATA
                //AddonBytes_Data.CopyTo(BufferData, LastIndex);
                //LastIndex += AddonBytes_Data.Length;

                //myreadEventArgs.SetBuffer(BufferData, 0, BufferData.Length);
                //return BufferData;

                //用Buffer.BlockCopy拷贝

                byte[] AddonBytes_CmdID = BitConverter.GetBytes(CmdID);
                int AllLenght = AddonBytes_CmdID.Length + AddonBytes_Data.Length + 4;
                int LastIndex = 0;
                //包长度
                byte[] AddonBytes_Lenght = BitConverter.GetBytes(AllLenght);

                byte[] BufferData = new byte[AllLenght];
                //包长度
                Buffer.BlockCopy(AddonBytes_Lenght, 0, BufferData, LastIndex, AddonBytes_Lenght.Length);
                LastIndex += AddonBytes_Lenght.Length;

                //CMDID
                Buffer.BlockCopy(AddonBytes_CmdID, 0, BufferData, LastIndex, AddonBytes_CmdID.Length);
                LastIndex += AddonBytes_CmdID.Length;

                //DATA
                Buffer.BlockCopy(AddonBytes_Data, 0, BufferData, LastIndex, AddonBytes_Data.Length);
                LastIndex += AddonBytes_Data.Length;
                return BufferData;
            }

            public static void AnalysisPkgData(Span<byte> srcdata, out UInt16 CmdID, out byte[] data)
            {
                //data = new byte[srcdata.Length - 2];
                //CmdID = BitConverter.ToUInt16(srcdata, 0);
                //Array.Copy(srcdata, 2, data, 0, data.Length);

                //CmdID = BitConverter.ToUInt16(srcdata, 0);
                //Span<byte> span_srcdata = srcdata;
                //data = span_srcdata.Slice(2).ToArray();

                CmdID = BitConverter.ToUInt16(srcdata.Slice(0, 2));
                data = srcdata.Slice(2).ToArray();
            }
        }
    }
}
