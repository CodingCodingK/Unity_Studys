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
        public string text;
    }

    public class ServerConfig
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}

