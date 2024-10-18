using System;
using System.Collections.Generic;
using System.IO;

namespace HaoYueNet.ClientNetwork.IOCPMode
{

    public class MemoryStreamPools
    {
        Stack<MemoryStream> m_pool;

        public MemoryStreamPools(int capacity)
        {
            m_pool = new Stack<MemoryStream>(capacity);
        }

        public void Push(MemoryStream item)
        {
            if (item == null) { throw new ArgumentNullException("Items added to a MemoryStream cannot be null"); }
            lock (m_pool)
            {
                m_pool.Push(item);
            }
        }
        public MemoryStream Pop()
        {
            lock (m_pool)
            {
                return m_pool.Pop();
            }
        }

        public int Count
        {
            get { return m_pool.Count; }
        }

        public void Clear()
        {
            m_pool.Clear();
        }
    }
}
