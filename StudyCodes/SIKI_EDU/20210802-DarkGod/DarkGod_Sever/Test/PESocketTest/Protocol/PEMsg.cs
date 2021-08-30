using System;
using PENet;

namespace Protocol
{
    [Serializable]
    public class NetMsg : PEMsg
    {
        public string text;
    }

    public class IpCfg
    {
        /// <summary>
        /// 本地端口（固定）
        /// </summary>
        public const string svrIP = "127.0.0.1";
        public const int svrPort = 17666;
    }
}