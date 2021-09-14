/****************************************************
    文件：CreateWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/24 9:46:37
    功能：创建人物界面
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class CreateWindow : WindowBase
{
    public InputField inputName;
    
    protected override void InitWindow()
    {
        base.InitWindow();
        setRandomName();
    }

    public void ClickRandomButton()
    {
        setRandomName();
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        GameRootResources.Instance().ShowTips("生成随机昵称成功");
    }

    public void ClickEnterButton()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        if (inputName.text != null)
        {
            // 发送网络请求
            GameMsg msg = new GameMsg
            {
                cmd = (int) CMD.ReqRename,
                reqRename = new ReqRename() {name = inputName.text}
            };
            netSvc.SendMsg(msg);
            
            GameRootResources.Instance().ShowTips("创建成功");
            
            // TODO 跳转场景到主城
            MainCitySys.Instance.EnterMainCity();
            
            // 关闭窗口
            SetWindowState(false);
        }
        else
        {
            GameRootResources.Instance().ShowTips("当前输入昵称不符合规范");
        }
    }
    
    /// <summary>
    /// 生成一个随机姓名并赋值
    /// </summary>
    private void setRandomName()
    {
        var rd = new System.Random();
        var isMan = rd.Next(0, 2) == 1;
        inputName.text = ResSvc.Instance().GetRDName(isMan);
    }
}
