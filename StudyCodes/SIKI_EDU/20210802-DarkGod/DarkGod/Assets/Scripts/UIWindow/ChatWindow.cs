/****************************************************
    文件：LoginWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/23
    功能：聊天界面
*****************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindow : WindowBase
{
    #region define
    // UI
    public Image imgWrold;
    public Image imgGuild;
    public Image imgFriend;
    public Text txtChat;
    public InputField input;

    
    // others
    private int chatType;
    private List<string> chatList = new List<string>();
    private bool canSend = true;
    
    #endregion
    

    protected override void InitWindow()
    {
        base.InitWindow();

        chatType = 0;

        RegClickEvents();
        RefreshUI();
    }

    private void RefreshUI()
    {
        string chatMsg = string.Empty;
        if (chatType == 0)
        {
            SetSprite(imgWrold,PathDefine.chatBtn1);
            SetSprite(imgGuild,PathDefine.chatBtn2);
            SetSprite(imgFriend,PathDefine.chatBtn2);
            
            
            for (int i = 0; i < chatList.Count; i++)
            {
                chatMsg += chatList[i] + "\n";
            }
        }
        else if (chatType == 1)
        {
            chatMsg = "暂未公会信息";
            SetSprite(imgWrold,PathDefine.chatBtn2);
            SetSprite(imgGuild,PathDefine.chatBtn1);
            SetSprite(imgFriend,PathDefine.chatBtn2);
        }
        else if (chatType == 2)
        {
            chatMsg = "暂未好友信息";
            SetSprite(imgWrold,PathDefine.chatBtn2);
            SetSprite(imgGuild,PathDefine.chatBtn2);
            SetSprite(imgFriend,PathDefine.chatBtn1);
        }

        SetText(txtChat,chatMsg);
    }

    public void AddChatMsg(string name,string chat)
    {
        chatList.Add(Constants.ColoredTxt(name + "：",TxtColor.Blue) + chat);
        if (chatList.Count > 13)
        {
            chatList.RemoveAt(0);
        }
        RefreshUI();
    }
    
    #region Click Events

    private void RegClickEvents()
    {
        
    }
    
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWindowState(false);
    }
    
    public void ClickWorldBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        RefreshUI();
    }
    public void ClickGuildBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 1;
        RefreshUI();
    }
    public void ClickFriendBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 2;
        RefreshUI();
    }
    
    public void ClickSendBtn()
    {
        if (!canSend)
        {
            GameRootResources.Instance().ShowTips("每5秒只能发送一次消息");
            return;
        }
        
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        if (!string.IsNullOrEmpty(input.text))
        {
            if (input.text.Length > 21)
            {
                GameRootResources.Instance().ShowTips("聊天限制最长21字！");
            }
            else
            {
                // 发送网络消息
                GameMsg msg = new GameMsg()
                {
                    cmd = (int) CMD.SendChat,
                    sendChat = new SendChat()
                    {
                        chat = input.text
                    }
                };
                input.text = string.Empty;
                netSvc.SendMsg(msg);
                canSend = false;
                timerSvc.AddTimeTask(i =>
                {
                    canSend = true;
                }, 5, PETimeUnit.Second, 1);
            }
        }
        else
        {
            GameRootResources.Instance().ShowTips("尚未输入聊天信息");
        }
    }

    
    #endregion

    // private IEnumerator MsgTimer()
    // {
    //     yield return new WaitForSeconds(5.0f);
    //     canSend = true;
    // }
    
}