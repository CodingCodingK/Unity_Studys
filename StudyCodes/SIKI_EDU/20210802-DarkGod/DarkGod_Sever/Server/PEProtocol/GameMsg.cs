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
        #region 登陆相关
        public ReqLogin reqLogin;
        public RspLogin rspLogin;
        public ReqRename reqRename;
        public RspRename rspRename;
		#endregion

		#region 主城相关
		public ReqGuide reqGuide;
		public RspGuide rspGuide;

		#endregion

	}

	#region 登陆相关

	/// <summary>
	/// 登录请求
	/// </summary>
	[Serializable]
    public class ReqLogin
    {
        public string acct { get; set; }
        public string pass { get; set; }
    }

    /// <summary>
    /// 登录回应
    /// </summary>
    [Serializable]
    public class RspLogin
    {
        public PlayerData playerData;
    }

    /// <summary>
    /// 重命名请求
    /// </summary>
    [Serializable]
    public class ReqRename
    {
        public string name { get; set; }
    }

    /// <summary>
    /// 重命名回应
    /// </summary>
    [Serializable]
    public class RspRename
    {
        public string name { get; set; }
    }
	#endregion

	#region 主城相关


	/// <summary>
	/// 引导请求
	/// </summary>
	[Serializable]
	public class ReqGuide
	{
		public int guideid { get; set; }
	}

	/// <summary>
	/// 引导回应
	/// </summary>
	[Serializable]
	public class RspGuide
	{
		public int guideid { get; set; }
		public int coin { get; set; }
		public int lv { get; set; }
		public int exp { get; set; }
	}

	#endregion


	/// <summary>
	/// 用户信息
	/// </summary>
	[Serializable]
    public class PlayerData
    {
        public int id { get; set; }
        public string name { get; set; }
		public int level { get; set; }
		public int exp { get; set; }
		public int power { get; set; }
		public int coin { get; set; }
		public int diamond { get; set; }
        public int hp { get; set; }
        public int ad { get; set; }
        public int ap { get; set; }
        public int addef { get; set; }
        public int apdef { get; set; }

        /// <summary>
        /// 闪避概率
        /// </summary>
        public int dodge { get; set; }

        /// <summary>
        /// 穿透比率
        /// </summary>
        public int pierce { get; set; }

        /// <summary>
        /// 暴击概率
        /// </summary>
        public int critical { get; set; }

		/// <summary>
		/// 当前自动引导任务ID
		/// </summary>
		public int guideid { get; set; }

		/// <summary>
		/// 不写成属性，手动映射，强化程度
		/// </summary>
		public int[] strongArr;

    }

    /// <summary>
    /// Command协议常数
    /// </summary>
    public enum ErrorCode
    {
        None = 0,

        /// <summary>
        /// 服务端数据异常
        /// </summary>
        ServerDataError,

		/// <summary>
		/// 更新数据库出错
		/// </summary>
		UpdateDBError,

        // 登录相关

        /// <summary>
        /// 账号已登陆
        /// </summary>
        AccountIsOnline,

        /// <summary>
        /// 密码错误
        /// </summary>
        WrongPass,

        /// <summary>
        /// 名字已存在
        /// </summary>
        NameExisted,

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
        ReqRename=103,
        RspRename=104,

		// 主城相关
		ReqGuide = 200,
		RspGuide = 201,
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

