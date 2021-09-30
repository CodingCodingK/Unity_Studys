using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocol;

/// <summary>
/// 主城强化业务
/// </summary>
public class StrongSys : Singleton<StrongSys>
{
	private StrongSys() { }

	private CacheSvc cacheSvc;
	private CfgSvc cfgSvc;

	public void Init()
	{
		cacheSvc = CacheSvc.Instance();
		cfgSvc = CfgSvc.Instance();
		PECommon.Log("StrongSys Init Done.");
	}

	/// <summary>
	/// 处理 引导任务请求消息
	/// </summary>
	public void ReqStrong(MsgPack pack)
	{
		ReqStrong data = pack.msg.reqStrong;

		GameMsg msg = new GameMsg()
		{
			cmd = (int)CMD.RspStrong,
		};

		PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
		int curtStartLv = pd.strongArr[data.pos];
		// 数据验证
		var curtData = cfgSvc.GetStrongData(data.pos, curtStartLv);
		var nextData = cfgSvc.GetStrongData(data.pos, curtStartLv+1);
		if (curtData == null || nextData == null)
		{
			msg.err = (int) ErrorCode.ServerDataError;
		}
		else if (pd.level < nextData.minlv)
		{
			msg.err = (int)ErrorCode.LackLevel;
		}
		else if (pd.coin < nextData.coin)
		{
			msg.err = (int)ErrorCode.LackCoin;
		}
		else if (pd.crystal < nextData.crystal)
		{
			msg.err = (int)ErrorCode.LackCrystal;
		}
		else
		{
			// 任务进度更新:强化升级
			TaskSys.Instance().CalcTaskPrgs(pd, 3);

			// 验证通过，开始更新DB数据
			pd.strongArr[data.pos] += 1;
			pd.coin -= nextData.coin;
			pd.crystal -= nextData.crystal;
			pd.hp += nextData.addhp;
			pd.ad += nextData.addhurt;
			pd.ap += nextData.addhurt;
			pd.apdef += nextData.adddef;
			pd.addef += nextData.adddef;

			if (cacheSvc.UpdatePlayerData(pd))
			{
				msg.rspStrong = new RspStrong()
				{
					coin = pd.coin,
					crystal = pd.crystal,
					hp = pd.hp,
					ad = pd.ad,
					ap = pd.ap,
					addef = pd.addef,
					apdef = pd.apdef,
					strongArr = pd.strongArr,
				};
			}
			else
			{
				msg.err = (int)ErrorCode.UpdateDBError;
			}
		}

		pack.session.SendMsg(msg);
	}

}