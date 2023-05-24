using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient
{
    public static class NetBase
    {
        public static byte[] Serizlize(IMessage msg)
        {
            return msg.ToByteArray();
        }

        public static T DeSerizlize<T>(byte[] bytes)
        {
            var msgType = typeof(T);
            object msg = Activator.CreateInstance(msgType);
            ((IMessage)msg).MergeFrom(bytes);
            return (T)msg;
        }
    }

}
