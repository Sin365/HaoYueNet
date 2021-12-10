using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

        /// <summary>  
        /// 数据缓存区  
        /// </summary>  
        public List<byte> Buffer { get; set; }
        
        public AsyncUserToken()
        {
            this.Buffer = new List<byte>();
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
