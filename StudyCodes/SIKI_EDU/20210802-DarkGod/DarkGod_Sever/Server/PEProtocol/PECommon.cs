using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PENet;

namespace PEProtocol
{
    /// <summary>
    /// Log级别
    /// </summary>
    public enum LogType
    {
        Log = 0,
        Warn = 1,
        Error = 2,
        Info = 3
    }

    public class PECommon
    {
        /// <summary>
        /// 自包一层的打印log方法
        /// </summary>
        public static void Log(string msg = "", LogType tp = LogType.Log)
        {
            LogLevel lv = (LogLevel) tp;
            PETool.LogMsg(msg, lv);
        }

        /// <summary>
        /// 战斗力计算
        /// </summary>
        public static int GetFight(PlayerData pd)
        {
            return pd.level * 100 + pd.ad + pd.ap + pd.addef + pd.apdef;
        }

        /// <summary>
        /// 体力上限计算
        /// </summary>
        public static int GetPowerLimit(int lv)
        {
            return ((lv - 1) / 10) * 150 + 150;
        }

        /// <summary>
        /// 体力上限计算
        /// </summary>
        public static int GetExpMaxValByLv(int lv)
        {
	        return 100 * (lv^2);
        }

		
		/// 体力增加单位时间（分钟）
		public const int PowerAddSpace = 5;
		/// 体力增加单位数量
		public const int PowerAddCount = 2;
	}
}
