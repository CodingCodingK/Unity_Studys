using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocol;

/// <summary>
/// 购买业务
/// </summary>
public class PowerSys : Singleton<PowerSys>
{
	private PowerSys() { }

	private CacheSvc cacheSvc;
	private TimerSvc timerSvc;

	public void Init()
	{
		cacheSvc = CacheSvc.Instance();
		timerSvc = TimerSvc.Instance();

		// 为了测试，把分钟改为秒
		TimerSvc.Instance().AddTimeTask(CalcPowerAdd,PECommon.PowerAddSpace,PETimeUnit.Second,0);

		PECommon.Log("PowerSys Init Done.");
	}

	private void CalcPowerAdd(int tid)
	{
		// 增长体力
		GameMsg msg = new GameMsg()
		{
			cmd = (int) CMD.PushPower,
			pushPower = new PushPower(),
		};


		var clientsAndPDs = cacheSvc.GetAllOnlineClientsAndPD();

		foreach (var item in clientsAndPDs)
		{
			PlayerData pd = item.Value;
			ServerSession clientSession = item.Key;

			int powerMax = PECommon.GetPowerLimit(pd.level);
			if (pd.power >= powerMax)
			{
				continue;
			}
			else
			{
				// 最高只能回复到上限值
				pd.power = pd.power + PECommon.PowerAddCount > powerMax ? powerMax : pd.power + PECommon.PowerAddCount;
				pd.time = timerSvc.GetNowTime();
			}

			if (!cacheSvc.UpdatePlayerData(pd))
			{
				msg.err = (int)ErrorCode.UpdateDBError;
			}

			msg.pushPower.power = pd.power;
			clientSession.SendMsg(msg);
		}

	}

}