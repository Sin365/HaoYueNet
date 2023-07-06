# HaoYueNet

.Net 7 的，自建基于IOCP的TCP的高性能网络库
使用Protobuff作为基础协议

包含服务端和客户端双端库，可直接用于各类.Net程序或Unity程序，做TCP通讯底层库。

并包含心跳包等检测、连接管理、Protobuff解析，优化后的高性能收发等等。

不用关心网络底层，直接引用或继承，即可便捷使用。

#使用基础事件回调即可:

OnClientNumberChange//连接数发生变化

OnDisconnected//断开连接

OnNetLog//来自网络库的日志信息

OnReceive//收到网络数据

#Simple目录下，有实例客户端和实例服务端
示例中，使用本网络库，您可以继续示例项目写，也可以参照示例代码。

实现了：

事件机制，

客户端基本框架（连接管理，数据管理，消息收发，指定用户发送）

服务端基本框架（连接管理，用户管理，消息收发，指定用户发送，广播等）

简单无OAuth登录，

用户列表，

基础的Protobuff设计，

基础聊天功能，

整合Protobuff生成。

#最简接入示例（服务端和客户端）
若您的应用相对简单，您甚至可以基于Simple增加功能，快速达成目标.

Server:

```
TcpSaeaServer Srv = new TcpSaeaServer(1024, 1024);//实例化，最大连接数和最大接收字节数
Srv.OnClientNumberChange += (int num, AsyncUserToken client) => { /* 连接数发生变化*/};
Srv.OnDisconnected += (AsyncUserToken client) => { /* 断开连接 */};
Srv.OnNetLog += (string msg) => { /* 来自网络库的日志信息 */};
Srv.OnReceive += (AsyncUserToken client, int CMDID, byte[] data) => {
    /* 收到网络消息 CMDID和数据 */
    Srv.SendMessage(client, new byte[1] { 0x00 });//给指定连接发送数据
};
Srv.Init();//初始化
Srv.Start(new IPEndPoint(IPAddress.Any.Address, 6000));//启动
```


Client:

```
NetworkHelperCore network = new NetworkHelperCore();
network.OnClose += ()=> { /* 断开连接 */}; 
network.OnConnected += (bool IsConnect) => { /* 连接回到，成功或失败 */};
network.OnLogOut += (string msg) => { /* 来自网络库的日志信息 */};
//指定接收服务器数据事件
network.OnReceiveData += (int CMDID, int ERRCODE, byte[] data) => {
    /* 收到网络消息 CMDID和数据 */
    network.SendToServer(CMDID, new byte[1] { 0x00 });//给服务器发送数据
};
network.Init("127.0.0.1", 6000);//连接服务器
```


#引用姿势

方式1.直接解决方案引用项目

方式2.直接引用dll文件

服务端

HaoYueNet.ServerNetwork.dll 

客户端

HaoYueNet.ClientNetwork.dll (.net7 推荐跨平台.net程序使用)

HaoYueNet.ClientNetworkNet4x.dll (传统.Net4.X版本,用于传统.NetFX程序或Unity游戏，或Mono程序)
