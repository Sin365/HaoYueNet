using System.Buffers;
using System.Net.Sockets;
namespace HaoYueNet.ClientNetwork
{
    public static class BytesArrayPool
    {
        public static byte[] RentBuffer(int minSize)
        {
            return ArrayPool<byte>.Shared.Rent(minSize);
        }

        public static void ReturnBuffer(byte[] buffer)
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    public static class BaseData
    {

#if NETSTANDARD2_0_OR_GREATER
        static class BitConverter
        {
            /// <summary>
            /// 将short值以小端序写入字节数组
            /// </summary>
            /// <param name="buffer">目标字节数组</param>
            /// <param name="value">要写入的short值</param>
            /// <param name="offset">写入起始位置</param>
            public static void TryWriteBytes(byte[] buffer, short value, int offset = 0)
            {
                // 小端序：低位在前，高位在后
                buffer[offset] = (byte)value;            // 低位字节
                buffer[offset + 1] = (byte)(value >> 8); // 高位字节
            }

            /// <summary>
            /// 将ushort值以小端序写入字节数组
            /// </summary>
            public static void TryWriteBytes(byte[] buffer, ushort value, int offset = 0)
            {
                buffer[offset] = (byte)value;            // 低位字节
                buffer[offset + 1] = (byte)(value >> 8); // 高位字节
            }

            /// <summary>
            /// 将uint值以小端序写入字节数组
            /// </summary>
            public static void TryWriteBytes(byte[] buffer, uint value, int offset = 0)
            {
                buffer[offset] = (byte)value;            // 字节0 (最低位)
                buffer[offset + 1] = (byte)(value >> 8);  // 字节1
                buffer[offset + 2] = (byte)(value >> 16); // 字节2
                buffer[offset + 3] = (byte)(value >> 24); // 字节3 (最高位)
            }

            /// <summary>
            /// 将int值以小端序写入字节数组
            /// </summary>
            public static void TryWriteBytes(byte[] buffer, int value, int offset = 0)
            {
                buffer[offset] = (byte)value;            // 字节0 (最低位)
                buffer[offset + 1] = (byte)(value >> 8);  // 字节1
                buffer[offset + 2] = (byte)(value >> 16); // 字节2
                buffer[offset + 3] = (byte)(value >> 24); // 字节3 (最高位)
            }

            public static ushort ToUInt16(Span<byte> span)
            {
                return System.BitConverter.ToUInt16(span.ToArray(), 0);
            }
        }
#endif
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
            const int LenghtDataLength = sizeof(UInt32);
            const int CmdDataLength = sizeof(UInt16);
            const int ErrDataLength = sizeof(UInt16);
            public static int GetAllLength(byte[] bodyData)
            {
                return CmdDataLength + ErrDataLength + LenghtDataLength + bodyData.Length;
            }
            public static int GetBodyLength(byte[] allData)
            {
                return allData.Length - CmdDataLength - ErrDataLength;
            }
            public static void SetDataToSocketAsyncEventArgs(SocketAsyncEventArgs myreadEventArgs, UInt16 CmdID, UInt16 Error, byte[] AddonBytes_Data)
            {
                int allDataLength = GetAllLength(AddonBytes_Data);
                byte[] finalData = BytesArrayPool.RentBuffer(allDataLength);
                CreatePkgData(CmdID, Error, AddonBytes_Data, ref finalData, out int usefulLenght);
                myreadEventArgs.SetBuffer(finalData, 0, usefulLenght);
                BytesArrayPool.ReturnBuffer(finalData);

                //myreadEventArgs.SetBuffer(CreatePkgData(CmdID, Error, AddonBytes_Data));
            }
            public static void CreatePkgData(UInt16 CmdID, UInt16 Error, byte[] AddonBytes_Data, ref byte[] finalData, out int usefulLenght)
            {
                int allLength = GetAllLength(AddonBytes_Data);
                byte[] bytes_allLenght = BytesArrayPool.RentBuffer(LenghtDataLength);
                BitConverter.TryWriteBytes(bytes_allLenght, allLength);//包长度
                byte[] bytes_cmdid = BytesArrayPool.RentBuffer(CmdDataLength);
                BitConverter.TryWriteBytes(bytes_cmdid, CmdID);
                byte[] bytes_errid = BytesArrayPool.RentBuffer(ErrDataLength);
                BitConverter.TryWriteBytes(bytes_errid, Error);

                Buffer.BlockCopy(bytes_allLenght, 0, finalData, 0, LenghtDataLength);
                //CMDID
                Buffer.BlockCopy(bytes_cmdid, 0, finalData, LenghtDataLength, CmdDataLength);
                //ErrID
                Buffer.BlockCopy(bytes_errid, 0, finalData, LenghtDataLength + CmdDataLength, ErrDataLength);
                //DATA
                Buffer.BlockCopy(AddonBytes_Data, 0, finalData, LenghtDataLength + CmdDataLength + ErrDataLength, AddonBytes_Data.Length);

                BytesArrayPool.ReturnBuffer(bytes_allLenght);
                BytesArrayPool.ReturnBuffer(bytes_cmdid);
                BytesArrayPool.ReturnBuffer(bytes_errid);

                usefulLenght = allLength;

                /*
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
                return BufferData;*/
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
            const int LenghtDataLength = sizeof(UInt32);
            const int CmdDataLength = sizeof(UInt16);
            public static int GetAllLengthByBody(byte[] bodyData)
            {
                return LenghtDataLength + CmdDataLength + bodyData.Length;
            }
            public static int GetAllLengthByUsedLen(int UsedLength)
            {
                return LenghtDataLength + CmdDataLength + UsedLength;
            }
            public static int GetBodyLength(byte[] allData)
            {
                return allData.Length - CmdDataLength;
            }

            public static void SetDataToSocketAsyncEventArgs(SocketAsyncEventArgs myreadEventArgs, UInt16 CmdID, byte[] AddonBytes_Data)
            {
                int allDataLength = GetAllLengthByBody(AddonBytes_Data);
                byte[] finalData = BytesArrayPool.RentBuffer(allDataLength);
                CreatePkgData(CmdID, AddonBytes_Data, ref finalData, out int usefulLenght);
                myreadEventArgs.SetBuffer(finalData, 0, usefulLenght);
                BytesArrayPool.ReturnBuffer(finalData);
                //myreadEventArgs.SetBuffer(CreatePkgData(CmdID, AddonBytes_Data));
            }

            public static void CreatePkgData(UInt16 CmdID, byte[] bytesdata, ref byte[] finalData, out int usefulLenght)
            {
                CreatePkgData(CmdID, bytesdata, bytesdata.Length, ref finalData, out usefulLenght);
            }

            public static void CreatePkgData(UInt16 CmdID, byte[] bytesdata, int bytesdata_useLenght, ref byte[] finalData, out int usefulLenght)
            {
                int allLength = GetAllLengthByUsedLen(bytesdata_useLenght);
                byte[] bytes_allLenght = BytesArrayPool.RentBuffer(LenghtDataLength);
                BitConverter.TryWriteBytes(bytes_allLenght, allLength);
                byte[] bytes_cmdid = BytesArrayPool.RentBuffer(CmdDataLength);
                BitConverter.TryWriteBytes(bytes_cmdid, CmdID);

                //包长度
                Buffer.BlockCopy(bytes_allLenght, 0, finalData, 0, LenghtDataLength);
                //CMDID
                Buffer.BlockCopy(bytes_cmdid, 0, finalData, LenghtDataLength, CmdDataLength);
                //DATA
                Buffer.BlockCopy(bytesdata, 0, finalData, LenghtDataLength + CmdDataLength, bytesdata_useLenght);

                BytesArrayPool.ReturnBuffer(bytes_allLenght);
                BytesArrayPool.ReturnBuffer(bytes_cmdid);

                usefulLenght = allLength;

                /*
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
                return BufferData;*/
            }

            public static void AnalysisPkgData(Span<byte> srcdata, out UInt16 CmdID, out byte[] data)
            {
                CmdID = BitConverter.ToUInt16(srcdata.Slice(0, 2));
                data = srcdata.Slice(2).ToArray();
            }
        }
    }
}