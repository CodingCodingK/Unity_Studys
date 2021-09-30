using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBHelper;
using PEProtocol;

/// <summary>
/// 任务业务
/// </summary>
public class TaskSys : Singleton<TaskSys>
{
	private TaskSys() { }

	private CacheSvc cacheSvc;
	private CfgSvc cfgSvc;

	public void Init()
	{
		cacheSvc = CacheSvc.Instance();
		cfgSvc = CfgSvc.Instance();
		PECommon.Log("TaskSys Init Done.");
	}

	/// <summary>
	/// 处理 请求消息
	/// </summary>
	public void ReqTask(MsgPack pack)
	{
		ReqTask data = pack.msg.reqTask;
		PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
		TaskCfg cfg = cfgSvc.GetTaskData(data.rid);
		TaskData task = CalcTaskData(pd, data.rid);
		GameMsg msg = new GameMsg()
		{
			cmd = (int)CMD.RspTask,
			
		};
		if (task != null && task.prgs == cfg.count && !task.isTaken)
		{
			task.isTaken = true;
			pd = CalcTaskArr(pd, task);

			if (cacheSvc.UpdatePlayerData(pd))
			{
				msg.rspTask = new RspTask()
				{
					coin = pd.coin,
					exp = pd.exp,
					lv = pd.level,
					taskArr = pd.taskArr,
				};
			}
			else
			{
				msg.err = (int) ErrorCode.UpdateDBError;
			}

		}
		else
		{
			msg.err = (int) ErrorCode.ClientDataError;
		}

		pack.session.SendMsg(msg);
	}

	/// <summary>
	/// 获取某个player的某任务进度
	/// </summary>
	public TaskData CalcTaskData(PlayerData pd,int rid)
	{
		TaskData task = null;
		for (int i = 0; i < pd.taskArr.Length; i++)
		{
			var taskList = pd.taskArr[i].Split('|');
			if (int.Parse(taskList[0]) == rid)
			{
				task = new TaskData()
				{
					ID = int.Parse(taskList[0]),
					prgs = int.Parse(taskList[1]),
					isTaken = taskList[2].Equals("1"),
				};
			}
		}

		return task;
	}

	public PlayerData CalcTaskArr(PlayerData pd, TaskData task)
	{
		TaskCfg cfg = cfgSvc.GetTaskData(task.ID);
		pd.coin += cfg.coin;
		pd = PECommon.CalcExp(pd, cfg.exp);
		for (int i = 0; i < pd.taskArr.Length; i++)
		{
			var intList = pd.taskArr[i].ToIntArr('|');
			if (intList[0] == task.ID)
			{
				pd.taskArr[i] = intList[0] + "|" + task.prgs + "|" + (task.isTaken?1:0);
			}
		}

		return pd;
	}

	/// <summary>
	/// 增加任务完成进度 +1
	/// </summary>
	public void CalcTaskPrgs(PlayerData pd, int tID)
	{
		TaskCfg cfg = cfgSvc.GetTaskData(tID);
		TaskData task = CalcTaskData(pd, tID);
		//如果未完成则+1，已经到达标准则不推送
		if (task.prgs < cfg.count)
		{
			task.prgs += 1;
			pd = CalcTaskArr(pd,task);
			cacheSvc.UpdatePlayerData(pd);
			var session = cacheSvc.GetSessionByPlayerID(pd.id);
			if (session != null)
			{
				session.SendMsg(new GameMsg()
				{
					cmd = (int)CMD.PushTaskPrgs,
					pushTaskPrgs = new PushTaskPrgs()
					{
						taskArr = pd.taskArr,
					}
				});
			}
		}
	}


}