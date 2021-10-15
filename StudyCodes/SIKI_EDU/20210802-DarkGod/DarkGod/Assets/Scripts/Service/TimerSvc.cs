using System;
using PEProtocol;
using UnityEngine;

/// <summary>
/// 计时服务
/// </summary>
public class TimerSvc : GameRootMonoSingleton<TimerSvc>
{
    public void InitSvc()
    {
        pt = new PETimer();
        // 给PETimer的Log方法添加委托
        pt.SetLog(info =>
        {
            PECommon.Log(info);
        });
        Debug.Log("TimerSvc Init Completed.");
    }

    public void Update()
    {
        pt.Update();
    }

    private PETimer pt;
    
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
        return pt.AddTimeTask(callback,delay,timeUnit,count);
    }

    public double GeyNowTime()
    {
        return pt.GetMillisecondsTime();
    }
    
    
}