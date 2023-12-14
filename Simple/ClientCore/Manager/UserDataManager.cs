using AxibugProtobuf;

namespace ClientCore.Manager
{
    public class UserDataBase
    {
        public long UID { get; set; }
        public string Account { get; set; }
    }

    public class MainUserDataBase : UserDataBase
    {
        public bool IsLoggedIn { get; set; } = false;
    }

    public class UserDataManager
    {
        public UserDataManager()
        {
            //注册重连成功事件，以便后续自动登录
            App.networkHelper.OnReConnected += OnReConnected;
        }

        MainUserDataBase user = new MainUserDataBase();
        public bool IsLoggedIn => user.IsLoggedIn;

        public void InitMainUserData(string UName)
        {
            user.Account = UName;
            user.IsLoggedIn = true;
            //以及其他数据初始化
            //...
        }

        /// <summary>
        /// 登出
        /// </summary>
        public void LoginOutData()
        {
            user.IsLoggedIn = false;
            //以及其他数据清理
            //...
        }

        /// <summary>
        /// 当重连成功
        /// </summary>
        public void OnReConnected()
        {
            //如果之前已登录，则重新登录
            if (user.IsLoggedIn)
            {
                App.login.Login(user.Account);
            }
        }
    }
}
