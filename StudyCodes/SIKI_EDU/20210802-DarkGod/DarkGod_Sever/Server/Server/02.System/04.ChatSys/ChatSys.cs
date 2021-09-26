using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocol;

/// <summary>
/// 聊天业务
/// </summary>
public class ChatSys : Singleton<ChatSys>
{
	private ChatSys() { }

	private CacheSvc cacheSvc;

	public void Init()
	{
		cacheSvc = CacheSvc.Instance();
		PECommon.Log("ChatSys Init Done.");
	}

	/// <summary>
	/// 处理 请求消息
	/// </summary>
	public void SendChat(MsgPack pack)
	{
		SendChat data = pack.msg.sendChat;
		PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
		GameMsg msg = new GameMsg()
		{
			cmd = (int)CMD.PushChat,
			pushChat = new PushChat()
			{
				name = pd.name,
				chat = data.chat,
			}
		};

		// 广播所有在线客户端
		var onlineClients = cacheSvc.GetAllOnlineClients();
		// 网络优化
		var msgBytes = PENet.PETool.PackNetMsg(msg);
		foreach (var onlineClient in onlineClients)
		{
			onlineClient.SendMsg(msgBytes);
		}

	}

}