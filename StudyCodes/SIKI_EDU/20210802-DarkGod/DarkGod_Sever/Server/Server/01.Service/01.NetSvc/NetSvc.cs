using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PENet;
using PEProtocol;

/// <summary>
/// 网络服务
/// </summary>
public class NetSvc : Singleton<NetSvc>
{
    private NetSvc() { }

    public void Init()
    {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        server.StartAsServer(ServerConfig.srvIP, ServerConfig.srvPort);

        
        PECommon.Log("NetSvc Init Done.");
    }
}