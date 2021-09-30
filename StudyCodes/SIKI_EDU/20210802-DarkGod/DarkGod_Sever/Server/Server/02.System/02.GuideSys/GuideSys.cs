using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocol;

/// <summary>
/// 任务引导业务
/// </summary>
public class GuideSys : Singleton<GuideSys>
{
	private GuideSys() { }

	private CacheSvc cacheSvc;
	private CfgSvc cfgSvc;
	private TaskSys taskSys;

	public void Init()
	{
		cacheSvc = CacheSvc.Instance();
		cfgSvc = CfgSvc.Instance();
		taskSys = TaskSys.Instance();
		PECommon.Log("GuideSys Init Done.");
	}

	/// <summary>
	/// 处理 引导任务请求消息
	/// </summary>
	public void ReqGuide(MsgPack pack)
	{
		ReqGuide data = pack.msg.reqGuide;

		GameMsg msg = new GameMsg()
		{
			cmd = (int)CMD.RspGuide,
		};

		PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
		AutoGuideCfg gc = cfgSvc.GetAutoGuideData(data.guideid);
	
		// 数据验证
		if (pd.guideid == data.guideid)
		{
			// 任务进度更新:智者点拨
			if (pd.guideid == 1001)
			{
				taskSys.CalcTaskPrgs(pd,1);
			}

			// 更新引导ID
			pd.guideid += 1;

			// 更新玩家数据
			pd.coin += gc.coin;
			pd = PECommon.CalcExp(pd,gc.exp);

			if (!cacheSvc.UpdatePlayerData(pd))
			{
				msg.err = (int) ErrorCode.UpdateDBError;
			}
			else
			{
				msg.rspGuide = new RspGuide()
				{
					guideid = pd.guideid,
					coin = pd.coin,
					lv = pd.level,
					exp = pd.exp
				};
			}
		}
		else
		{
			msg.err = (int) ErrorCode.ServerDataError;
		}

		pack.session.SendMsg(msg);
	}

	
}