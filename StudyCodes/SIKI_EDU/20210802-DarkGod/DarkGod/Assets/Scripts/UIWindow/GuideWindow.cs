/****************************************************
    文件：GuideWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/22
    功能：对话窗口
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class GuideWindow : WindowBase
{
    #region define
    //UI
    public Text txtName;
    public Text txtTalk;
    public Image imgIcon;

    // others
    private AutoGuideCfg curtTaskData;
    private string[] dialogArr;
    private int index;
    private PlayerData pd;

    #endregion
  
    
    protected override void InitWindow()
    {
        base.InitWindow();

        pd = GameRoot.Instance().PlayerData;
        curtTaskData = MainCitySys.Instance.GetCurTaskData();
        dialogArr = curtTaskData.dilogArr.Split('#');
        index = 1;

        SetTalk();
    }

    /*
     data like below
     #0|智者您好，晚辈$name,前来拜会。
     #1|漫漫人生路，你我得以相遇也是一种缘分。我看你骨骼精奇，眉宇间正气凛然，将来定能成就一番事业。
     #0|智者过誉了，晚辈阅历尚浅，学识浅薄，空有满腔热血，还请前辈多多教导。
     */
    private void SetTalk()
    {
        string[] talkArr = dialogArr[index].Split('|');
        
        // imgIcon,txtName
        if (talkArr[0] == "0")
        {
            //自己
            SetSprite(imgIcon,PathDefine.SelfIcon);
            SetText(txtName,pd.name);
        }
        else
        {
            switch (curtTaskData.npcID)
            {
                case Constants.NPCWiseMan:
                    SetSprite(imgIcon,PathDefine.WiseManIcon);
                    SetText(txtName,"智者");
                    break;
                case Constants.NPCGeneral:
                    SetSprite(imgIcon,PathDefine.GeneralIcon);
                    SetText(txtName,"将军");
                    break;
                case Constants.NPCArtisan:
                    SetSprite(imgIcon,PathDefine.ArtisanIcon);
                    SetText(txtName,"工匠");
                    break;
                case Constants.NPCTrader:
                    SetSprite(imgIcon,PathDefine.TraderIcon);
                    SetText(txtName,"商人");
                    break;
                default:
                    SetSprite(imgIcon,PathDefine.GuideIcon);
                    SetText(txtName,"赛丽亚");
                    break;
            }
        }

        SetText(txtTalk,talkArr[1].Replace("$name",pd.name));
    }

    public void ClickNextBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        
        index += 1;
        if (dialogArr.Length >= index)
        {
            // 返回服务端完成任务
            GameMsg msg = new GameMsg()
            {
                cmd = (int)CMD.ReqGuide,
                reqGuide = new ReqGuide()
                {
                    guideid = curtTaskData.ID,
                }
            };
            netSvc.SendMsg(msg);
            
            SetWindowState(false);
            return;
        }
        SetTalk();
    }
}
