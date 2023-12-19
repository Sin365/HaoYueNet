using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoYueNet.ClientNetworkNet4x
{
    public static class BaseData
    {
        public static class HunterNet_S2C
        {
            public static byte[] CreatePkgData(UInt16 CmdID, UInt16 Error, byte[] data)
            {
                byte[] newdata = new byte[2 + 2 + data.Length];
                BitConverter.GetBytes(CmdID).CopyTo(newdata, 0);
                BitConverter.GetBytes(Error).CopyTo(newdata, 2);
                Array.Copy(data, 0, newdata, 4, data.Length);
                return newdata;
            }

            public static void AnalysisPkgData(byte[] srcdata,out UInt16 CmdID,out UInt16 Error,out byte[] data)
            {
                data = new byte[srcdata.Length - 2 - 2];
                CmdID = BitConverter.ToUInt16(srcdata, 0);
                Error = BitConverter.ToUInt16(srcdata, 2);
                Array.Copy(srcdata, 4, data, 0, data.Length);
            }
        }

        public static class HunterNet_C2S
        {
            public static byte[] CreatePkgData(UInt16 CmdID, byte[] data)
            {
                byte[] newdata = new byte[2 + data.Length];
                BitConverter.GetBytes(CmdID).CopyTo(newdata, 0);
                Array.Copy(data, 0, newdata, 2, data.Length);
                return newdata;
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
