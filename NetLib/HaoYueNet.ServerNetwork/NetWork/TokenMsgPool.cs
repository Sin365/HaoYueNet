using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace HaoYueNet.ServerNetwork
{
    public class TokenWithMsg
    {
        public AsyncUserToken token;
        public byte[] message;
    }

    public class TokenMsgPool
    {
        //Stack<TokenWithMsg> msg_pool;
        Queue<TokenWithMsg> msg_pool;

        public TokenMsgPool(int capacity)
        {
            //msg_pool = new Stack<TokenWithMsg>(capacity);
            msg_pool = new Queue<TokenWithMsg>(capacity);
        }

        //public void Push(TokenWithMsg item)
        //{
        //    if (item == null) { throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null"); }
        //    lock (msg_pool)
        //    {
        //        msg_pool.Push(item);
        //    }
        //}

        /// <summary>
        /// 向 Queue 的末尾添加一个对象。
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(TokenWithMsg item)
        {
            lock (msg_pool)
            {
                msg_pool.Enqueue(item);
            }
        }

        //移除并返回在 Queue 的开头的对象。
        public TokenWithMsg Dequeue()
        {
            lock (msg_pool)
            {
                return msg_pool.Dequeue();
            }
        }

        //// Removes a SocketAsyncEventArgs instance from the pool  
        //// and returns the object removed from the pool  
        //public TokenWithMsg Pop()
        //{
        //    lock (msg_pool)
        //    {
        //        return msg_pool.Pop();
        //    }
        //}

        // The number of SocketAsyncEventArgs instances in the pool  
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
