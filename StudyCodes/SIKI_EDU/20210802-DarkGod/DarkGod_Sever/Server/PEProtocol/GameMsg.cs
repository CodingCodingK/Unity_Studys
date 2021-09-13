using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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



        /// <summary>
        /// 登录请求（客户端服务端共用）
        /// </summary>
        public ReqLogin reqLogin;

        /// <summary>
        /// 登录回应（客户端服务端共用）
        /// </summary>
        public RspLogin rspLogin;
    }

    /// <summary>
    /// 登录请求（客户端服务端共用）
    /// </summary>
    [Serializable]
    public class ReqLogin
    {
        public string acct;
        public string pass;
    }

    /// <summary>
    /// 登录回应（客户端服务端共用）
    /// </summary>
    [Serializable]
    public class RspLogin
    {
        public PlayerData playerData;
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        public int id { get; set; }
		public string acct { get; set; }
		public string pass { get; set; }
		public string name { get; set; }
		public int level { get; set; }
		public int exp { get; set; }
		public int power { get; set; }
		public int coin { get; set; }
		public int diamond { get; set; }
	}

    /// <summary>
    /// Command协议常数
    /// </summary>
    public enum ErrorCode
    {
        None = 0,
        // 登录相关

        /// <summary>
        /// 账号已登陆
        /// </summary>
        AccountIsOnline,

        /// <summary>
        /// 密码错误
        /// </summary>
        WrongPass,

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

