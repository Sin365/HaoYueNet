using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace HaoYueNet.ServerNetwork.Standard2
{
    public class TokenWithMsg_SourceMode
    {
        public AsyncUserToken token;
        public byte[] data;
        public bool bHeartbeat;
    }

    public class TokenMsgPool_SourceMode
    {
        //Stack<TokenWithMsg> msg_pool;
        Queue<TokenWithMsg_SourceMode> msg_pool;

        public TokenMsgPool_SourceMode(int capacity)
        {
            msg_pool = new Queue<TokenWithMsg_SourceMode>(capacity);
        }

        /// <summary>
        /// 向 Queue 的末尾添加一个对象。
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(TokenWithMsg_SourceMode item)
        {
            lock (msg_pool)
            {
                msg_pool.Enqueue(item);
            }
        }

        //移除并返回在 Queue 的开头的对象。
        public TokenWithMsg_SourceMode Dequeue()
        {
            lock (msg_pool)
            {
                return msg_pool.Dequeue();
            }
        }

        public int Count
        {
            get { return msg_pool.Count; }
        }

        public void Clear()
        {
            msg_pool.Clear();
        }
    }
}
