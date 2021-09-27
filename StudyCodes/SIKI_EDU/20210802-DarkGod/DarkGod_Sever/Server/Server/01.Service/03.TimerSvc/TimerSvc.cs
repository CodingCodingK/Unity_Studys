using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MySql.Data;
using PEProtocol;

/// <summary>
/// 配置数据服务
/// </summary>
public class TimerSvc : Singleton<TimerSvc>
{
	private TimerSvc() { }

	public void Init()
	{
		pt = new PETimer(100);
		tpQue = new Queue<TaskPack>();

		// 给PETimer的Log方法添加委托
		pt.SetLog(info =>
		{
			PECommon.Log(info);
		});

		// 放在独立线程计时，检测插入Task到主线程执行的队列中
		pt.SetHandle((Action<int> cb, int tid) =>
		{
			if (cb!=null)
			{
				lock (tpQueLock)
				{
					tpQue.Enqueue(new TaskPack(tid,cb));
				}
			}
		});


		PECommon.Log("TimerSvc Init Done.");
	}

	private PETimer pt;
	private Queue<TaskPack> tpQue;
	private static readonly string tpQueLock = "tpQueLock";

	// 放在主线程执行Task
	public void Update()
	{
		while (tpQue.Count > 0)
		{
			TaskPack tp = null;
			lock (tpQueLock)
			{
				tp = tpQue.Dequeue();
			}

			if (tp!=null)
			{
				tp.cb(tp.tid);
			}
		}
	}

	/// <summary>
	/// 添加定时任务
	/// </summary>
	/// <param name="callback">回调函数</param>
	/// <param name="delay">回调函数执行前等待秒数</param>
	/// <param name="timeUnit">delay的单位</param>
	/// <param name="count">任务执行次数，0就是一直循环</param>
	/// <returns></returns>
	public int AddTimeTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
	{
		return pt.AddTimeTask(callback, delay, timeUnit, count);
	}

	public long GetNowTime()
	{
		return (long)pt.GetMillisecondsTime();
	}
}

public class TaskPack
{
	public int tid;
	public Action<int> cb;

	public TaskPack(int tid,Action<int> cb)
	{
		this.tid = tid;
		this.cb = cb;
	}
}