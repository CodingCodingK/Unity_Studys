using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBHelper;
using PEProtocol;

/// <summary>
/// 副本业务
/// </summary>
public class DungeonSys : Singleton<DungeonSys>
{
	private DungeonSys() { }

	private CacheSvc cacheSvc;
	private CfgSvc cfgSvc;

	public void Init()
	{
		cacheSvc = CacheSvc.Instance();
		cfgSvc = CfgSvc.Instance();
		PECommon.Log("DungeonSys Init Done.");
	}

	/// <summary>
	/// 处理 请求消息
	/// </summary>
	public void ReqDungeon(MsgPack pack)
	{
		int dgId = pack.msg.reqDungeon.dgId;
		int costPower = cfgSvc.GetMapData(dgId).power;
		PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

		GameMsg msg = new GameMsg()
		{
			cmd = (int)CMD.RspDungeon,

		};

		// 如果存在跳关
		if (pd.dg < dgId)
		{
			msg.err = (int) ErrorCode.ClientDataError;
		}
		// 如果体力不足
		else if (pd.power < costPower)
		{
			msg.err = (int)ErrorCode.LackPower;
		}
		else
		{
			pd.power -= costPower;
			if (cacheSvc.UpdatePlayerData(pd))
			{
				msg.rspDungeon = new RspDungeon()
				{
					dgId = dgId,
					power = pd.power,
				};
			}
		}

		pack.session.SendMsg(msg);
	}

}