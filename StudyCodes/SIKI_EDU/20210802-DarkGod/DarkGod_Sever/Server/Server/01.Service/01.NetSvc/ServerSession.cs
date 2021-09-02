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
        PECommon.Log("Client Connect");
        //SendMsg(new GameMsg
        //{
        //    text = "Welcome to connect"
        //});
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("RcvPack CMD:" + ((CMD)msg.cmd).ToString());
        //SendMsg(new GameMsg
        //{
        //    text = "Server Rsp:" + msg.text
        //});
    }

    protected override void OnDisConnected()
    {
        PECommon.Log("Client DisConnect");
    }
}