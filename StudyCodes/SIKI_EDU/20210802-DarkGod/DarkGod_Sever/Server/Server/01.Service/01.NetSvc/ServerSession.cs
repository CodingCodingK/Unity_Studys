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
	public int sessionId = 0;

    protected override void OnConnected()
    {
	    sessionId = ServerRoot.Instance().GetSessionId();
	    PECommon.Log("SessionId:"+ sessionId + " Client Connect");
       
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("SessionId:" + sessionId + " RcvPack CMD:" + ((CMD)msg.cmd).ToString());

        // 向消息队列添加新的消息处理，等待被轮询执行（执行线程不固定）
        NetSvc.Instance().AddMsgQue(this, msg);
    }

    protected override void OnDisConnected()
    {
	    LoginSys.Instance().ClearOfflineData(this);
	    PECommon.Log("SessionId:" + sessionId + " Client Offline");
    }
}