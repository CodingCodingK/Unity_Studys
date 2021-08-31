using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PENet;
using PEProtocol;

/// <summary>
/// 网络会话连接
/// </summary>
public class ServerSession : PESession<GameMsg>
{

    protected override void OnConnected()
    {
        PETool.LogMsg("Client Connect");
        SendMsg(new GameMsg
        {
            text = "Welcome to connect"
        });
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PETool.LogMsg("Client Req:" + msg.text);
        SendMsg(new GameMsg
        {
            text = "Server Rsp:" + msg.text
        });
    }

    protected override void OnDisConnected()
    {
        PETool.LogMsg("Client DisConnect");
    }
}