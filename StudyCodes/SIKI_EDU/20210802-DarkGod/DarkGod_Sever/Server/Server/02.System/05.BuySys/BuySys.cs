using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocol;

/// <summary>
/// 购买业务
/// </summary>
public class BuySys : Singleton<BuySys>
{
	private BuySys() { }

	private CacheSvc cacheSvc;

	public void Init()
	{
		cacheSvc = CacheSvc.Instance();
		PECommon.Log("BuySys Init Done.");
	}

	/// <summary>
	/// 处理 请求消息
	/// </summary>
	public void ReqBuy(MsgPack pack)
	{
		ReqBuy data = pack.msg.reqBuy;
		PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
		GameMsg msg = new GameMsg()
		{
			cmd = (int)CMD.RspBuy,
			rspBuy = new RspBuy()
			{
				type = data.type,
			}
		};

		if (pd.diamond < data.cost)
		{
			msg.err = (int)ErrorCode.LackDiamond;
		}
		else
		{
			pd.diamond -= data.cost;

			switch (data.type)
			{
				case 0:
					// 任务进度更新:购买体力
					TaskSys.Instance().CalcTaskPrgs(pd, 4);
					pd.power += 100;
					break;
				case 1:
					// 任务进度更新:铸造金币
					TaskSys.Instance().CalcTaskPrgs(pd, 5);
					pd.coin += 1000;
					break;
			}

			if (!cacheSvc.UpdatePlayerData(pd))
			{
				msg.err = (int)ErrorCode.UpdateDBError;
			}
			else
			{
				msg.rspBuy.power = pd.power;
				msg.rspBuy.coin = pd.coin;
				msg.rspBuy.diamond = pd.diamond;
			}
		}

		pack.session.SendMsg(msg);
	}

}