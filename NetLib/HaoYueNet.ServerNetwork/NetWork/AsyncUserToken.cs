using System.Net;
using System.Net.Sockets;

namespace HaoYueNet.ServerNetwork
{
    public class AsyncUserToken
    {
        /// <summary>  
        /// 客户端IP地址  
        /// </summary>  
        public IPAddress IPAddress { get; set; }

        /// <summary>  
        /// 远程地址  
        /// </summary>  
        public EndPoint Remote { get; set; }

        /// <summary>  
        /// 通信SOKET  
        /// </summary>  
        public Socket Socket { get; set; }

        /// <summary>  
        /// 连接时间  
        /// </summary>  
        public DateTime ConnectTime { get; set; }

        /// <summary>  
        /// 所属用户信息  
        /// </summary>  
        public object UserInfo { get; set; }

        public MemoryStream memoryStream { get; set; }

        public AsyncUserToken()
        {
            this.memoryStream = new MemoryStream();
        }
        /// <summary>
        /// 响应倒计时计数
        /// </summary>
        public int RevIndex { get; set; } = 0;
        /// <summary>
        /// 发送倒计时计数
        /// </summary>
        public int SendIndex { get; set; } = 0;
    }
}
