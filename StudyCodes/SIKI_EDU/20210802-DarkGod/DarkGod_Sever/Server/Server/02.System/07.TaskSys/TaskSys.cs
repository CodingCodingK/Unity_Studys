using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			rspTask = new RspTask()
			{

			}
		};



		pack.session.SendMsg(msg);
	}

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

}