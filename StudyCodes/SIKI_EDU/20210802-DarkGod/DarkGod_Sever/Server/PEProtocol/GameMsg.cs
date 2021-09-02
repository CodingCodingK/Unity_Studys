using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PENet;

namespace PEProtocol
{
    /// <summary>
    /// 网络通信协议（客户端服务端共用）
    /// </summary>
    [Serializable]
    public class GameMsg : PEMsg
    {
        public ReqLogin reqLogin;
    }

    /// <summary>
    /// 登录信息（客户端服务端共用）
    /// </summary>
    [Serializable]
    public class ReqLogin
    {
        public string acct;
        public string pass;
    }

    /// <summary>
    /// Command协议常数
    /// </summary>
    public enum CMD
    {
        None=0,
        // 登录相关
        ReqLogin=101,
        RspLogin=102,
    }

    /// <summary>
    /// 端口号常数
    /// </summary>
    public class ServerConfig
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}

