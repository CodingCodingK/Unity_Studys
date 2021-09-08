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

        // 向消息队列添加新的消息处理，等待被轮询执行（执行线程不固定）
        NetSvc.Instance().AddMsgQue(this, msg);

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