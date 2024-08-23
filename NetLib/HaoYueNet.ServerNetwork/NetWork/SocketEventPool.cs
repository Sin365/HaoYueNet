using System.Net.Sockets;

namespace HaoYueNet.ServerNetwork
{
    public class SocketEventPool
    {
        Stack<SocketAsyncEventArgs> m_pool;

        public SocketEventPool(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null) { throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null"); }
            lock (m_pool)
            {
                m_pool.Push(item);
            }
        }

        // Removes a SocketAsyncEventArgs instance from the pool  
        // and returns the object removed from the pool  
        public SocketAsyncEventArgs Pop()
        {
            lock (m_pool)
            {
                return m_pool.Pop();
            }
        }

        // The number of SocketAsyncEventArgs instances in the pool  
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
