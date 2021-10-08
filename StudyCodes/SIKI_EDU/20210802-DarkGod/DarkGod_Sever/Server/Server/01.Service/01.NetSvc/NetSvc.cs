using System;
using System.Collections;
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

    /// <summary>
    /// 消息处理队列
    /// </summary>
    private Queue<MsgPack> msgPackQue;

    public static readonly string obj = "lock";

    public void Init()
    {
        msgPackQue = new Queue<MsgPack>();

        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        server.StartAsServer(ServerConfig.srvIP, ServerConfig.srvPort);

        
        PECommon.Log("NetSvc Init Done.");
    }

    /// <summary>
    /// 向 消息队列 添加消息
    /// </summary>
    /// <param name="msg"></param>
    public void AddMsgQue(ServerSession session, GameMsg msg)
    {
        lock (obj)
        {
            msgPackQue.Enqueue(new MsgPack(session,msg));
        }
    }

    /// <summary>
    /// 轮询检查 消息队列
    /// </summary>
    public void Update()
    {
        if (msgPackQue?.Count > 0)
        {
            PECommon.Log("PacCount:"+ msgPackQue?.Count);
            lock (obj)
            {
                MsgPack msg = msgPackQue.Dequeue();
                HandOutMsg(msg);
            }
        }
    }

    /// <summary>
    /// 分发处理 消息
    /// </summary>
    private void HandOutMsg(MsgPack msgPack)
    {
        switch ((CMD)msgPack.msg.cmd)
        {
            case CMD.ReqLogin:
                LoginSys.Instance().ReqLogin(msgPack);
                break;
            case CMD.ReqRename:
                LoginSys.Instance().ReqRename(msgPack);
                break;
            case CMD.ReqGuide:
	            GuideSys.Instance().ReqGuide(msgPack);
	            break;
            case CMD.ReqStrong:
	            StrongSys.Instance().ReqStrong(msgPack);
	            break;
            case CMD.SendChat:
	            ChatSys.Instance().SendChat(msgPack);
	            break;
            case CMD.ReqBuy:
	            BuySys.Instance().ReqBuy(msgPack);
	            break;
            case CMD.ReqTask:
	            TaskSys.Instance().ReqTask(msgPack);
	            break;
            case CMD.ReqDungeon:
	            DungeonSys.Instance().ReqDungeon(msgPack);
	            break;

		}
    }


}