using System;
using System.Net.Sockets;

namespace HaoYueNet.ServerNetworkNet4x
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
            //buf[offset++] = (byte)(255 & value >>> 24);
            buf[offset++] = (byte)(255 & value >> 24);
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
                byte[] data = CreatePkgData(CmdID, Error, AddonBytes_Data);
                myreadEventArgs.SetBuffer(data, 0, data.Length);
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
                writeUInt16(BufferData, 4 + 2, CmdID);

                //DATA
                Buffer.BlockCopy(AddonBytes_Data, 0, BufferData, 4 + 2 + 2, AddonBytes_Data.Length);

                return BufferData;
            }

            public static void AnalysisPkgData(byte[] srcdata, out UInt16 CmdID, out UInt16 Error, out byte[] data)
            {
                CmdID = BitConverter.ToUInt16(srcdata, 0);
                Error = BitConverter.ToUInt16(srcdata, 2);
                data = new byte[srcdata.Length - 2 - 2];
                Array.Copy(srcdata, 4, data, 0, data.Length);

            }
        }

        public static class HunterNet_C2S
        {
            public static void SetDataToSocketAsyncEventArgs(SocketAsyncEventArgs myreadEventArgs, UInt16 CmdID, byte[] AddonBytes_Data)
            {
                byte[] data = CreatePkgData(CmdID, AddonBytes_Data);
                myreadEventArgs.SetBuffer(data, 0, data.Length);
            }

            public static byte[] CreatePkgData(UInt16 CmdID, byte[] AddonBytes_Data)
            {
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

            public static void AnalysisPkgData(byte[] srcdata, out UInt16 CmdID, out byte[] data)
            {
                data = new byte[srcdata.Length - 2];
                CmdID = BitConverter.ToUInt16(srcdata, 0);
                Array.Copy(srcdata, 2, data, 0, data.Length);
            }
        }
    }
}
