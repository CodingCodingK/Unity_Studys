/****************************************************
    文件：LoginWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/23
    功能：任务界面
*****************************************************/


using System;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class TaskWindow : WindowBase
{
    #region define
    // UI
    public Transform scrollerTrans;
    
    // others
    private PlayerData pd;
    private List<TaskData> taskList = new List<TaskData>();
   

    #endregion
    

    protected override void InitWindow()
    {
        base.InitWindow();
        pd = GameRoot.Instance().PlayerData;
        
        
        RegClickEvents();
        RefreshUI();
    }

    public void RefreshUI()
    {
        pd = GameRoot.Instance().PlayerData;
        taskList.Clear();

        var doingList = new List<TaskData>();
        var doneList = new List<TaskData>();

        for (int i=0;i<scrollerTrans.childCount;i++)
        {
            Destroy(scrollerTrans.GetChild(i).gameObject);
        }

        foreach(var data in pd.taskArr)
        {
            string[] taskInfo = data.Split('|');
            var td = new TaskData()
            {
                ID = int.Parse(taskInfo[0]),
                prgs = int.Parse(taskInfo[1]),
                isTaken = taskInfo[2].Equals("1"),
            };

            if (td.isTaken)
            {
                doneList.Add(td);
            }
            else
            {
                doingList.Add(td);
            }
        }
        
        // 前后排序
        taskList.AddRange(doingList);
        taskList.AddRange(doneList);

        for (int i = 0; i < taskList.Count; i++)
        {
            var go = resSvc.LoadPrefab(PathDefine.TaskItemPrefab);
            go.transform.SetParent(scrollerTrans);
            go.name = "taskItem_" + i;
            TaskData td = taskList[i];
            TaskCfg tcfg = resSvc.GetTaskData(td.ID);
            SetText(GetTrans(go, "txtName"),tcfg.taskName);
            SetText(GetTrans(go, "txtExp"),tcfg.exp);
            SetText(GetTrans(go, "txtCoin"),tcfg.coin);
            SetText(GetTrans(go, "txtPrg"),$"{td.prgs}/{tcfg.count}");
            Image imgPrg = GetTrans(go, "prgBar/imgPowerPrg").GetComponent<Image>();
            var prgVal = td.prgs * 1.0f / tcfg.count;
            imgPrg.fillAmount = prgVal;

            Button btnTake = GetTrans(go, "btnTake").GetComponent<Button>();
            btnTake.onClick.AddListener(() =>
            {
                ClickTakeButton(go.name);
            });

            var transComp = GetTrans(go, "iconCompleted");
            if (td.isTaken)
            {
                btnTake.interactable = false;
                SetActive(transComp);
            }
            else
            {
                SetActive(transComp,false);
                if (td.prgs.Equals(tcfg.count))
                {
                    btnTake.interactable = true;
                }
                else
                {
                    btnTake.interactable = false;
                }
            }
        }
    }

   
    #region Click Events

    private void RegClickEvents()
    {
        
    }

    private void ClickTakeButton(string name)
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        
        var index = int.Parse(name.Substring(9));
        Debug.Log("index:"+name + "sd:"+index);
        var task = taskList[index];
        var taskCfg = resSvc.GetTaskData(task.ID);
        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.ReqTask,
            reqTask = new ReqTask()
            {
                rid = task.ID,
            }
        };
        
        netSvc.SendMsg(msg);
        GameRootResources.Instance().ShowTips(Constants.ColoredTxt("获得奖励：金币 "+taskCfg.coin+"，经验 "+taskCfg.exp,TxtColor.Blue));
    }

    public void ClickCloseButton()
    {
        SetWindowState(false);
    }

    #endregion
   
    
}