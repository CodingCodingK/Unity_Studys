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

	/// <summary>
	/// 处理 请求消息
	/// </summary>
	public void ReqDungeonEnd(MsgPack pack)
	{
		ReqDungeonEnd data = pack.msg.reqDungeonEnd;
		PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

		GameMsg msg = new GameMsg()
		{
			cmd = (int)CMD.RspDungeonEnd,
		};

		// 校验战斗是否合法
		if (data.win)
		{
			if (data.costTime > 0 && data.restHp > 0)
			{
				// 根据副本id获取相应奖励
				MapCfg rd = cfgSvc.GetMapData(data.dgId);
				pd.coin += rd.coin;
				pd.crystal += rd.crystal;
				PECommon.CalcExp(pd, rd.exp);

				if (pd.dg == data.dgId)
				{
					pd.dg++;
				}

				if (cacheSvc.UpdatePlayerData(pd))
				{
					msg.rspDungeonEnd = new RspDungeonEnd()
					{
						win = data.win,
						dgId = data.dgId,
						costTime = data.costTime,
						restHp = data.restHp,
						coin = pd.coin,
						lv=pd.level,
						crystal = pd.crystal,
						dg = pd.dg
					};
				}
				else
				{
					msg.err = (int)ErrorCode.UpdateDBError;
				}
				
			}
		}
		else
		{
			msg.err = (int) ErrorCode.ClientDataError;
		}

		pack.session.SendMsg(msg);
	}

}