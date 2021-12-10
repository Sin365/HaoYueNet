using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient
{
    public static class NetBase
    {
        //序列化
        public static byte[] Serizlize<T>(T MsgObj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, MsgObj);
                byte[] data1 = ms.ToArray();
                return data1;
            }
        }
        //反序列化
        public static T DeSerizlize<T>(byte[] MsgObj)
        {
            using (MemoryStream ms = new MemoryStream(MsgObj))
            {
                var ds_obj = Serializer.Deserialize<T>(ms);
                return ds_obj;
            }
        }
    }

}
