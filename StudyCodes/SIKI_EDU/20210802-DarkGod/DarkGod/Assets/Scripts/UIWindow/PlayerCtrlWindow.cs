/****************************************************
    文件：PlayerCtrlWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/23
    功能：玩家控制界面
*****************************************************/


using System;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCtrlWindow : WindowBase
{
    #region define
    // UI
    
    // 拖拽范围
    public Image imgTouch;
    // 拖拽背景盘
    public Image imgDirBg;
    // 拖拽点
    public Image imgDirPoint;
    
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;
    
    /// <summary>
    /// 经验进度条
    /// </summary>
    public Transform expPrgTrans;

    
    // others
    
    /// <summary>
    /// 摇杆用 拖拽起始点
    /// </summary>
    private Vector2 startPos = Vector2.zero;
    
    /// <summary>
    /// 摇杆用 操控盘归零位置
    /// </summary>
    private Vector2 defaultPos = Vector2.zero;

    /// <summary>
    /// 摇杆范围 自适应用
    /// </summary>
    private float pointDis;
    
    
    #endregion
    
    protected override void InitWindow()
    {
        base.InitWindow();
        
        defaultPos = imgDirBg.transform.position;
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        
        RegClickEvents();
        RefreshUI();
    }

    private void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance().PlayerData;
        SetText(txtLevel,pd.level);
        SetText(txtName,pd.name);
        
        //expPrg text
        int expPrgVal = (int)(pd.exp * 1.0f / PECommon.GetExpMaxValByLv(pd.level) * 100);
        SetText(txtExpPrg,expPrgVal + "%");

        //expPrg img
        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        float globalRate = Screen.height * 1.0f / Constants.ScreenStandardHeight;
        float screenWidth = Screen.width * globalRate;
        float width = (screenWidth - 180) / 10;
        
        // 高固定，宽计算适配屏幕整体的UGUI宽度自适应
        grid.cellSize = new Vector2(width, 7);
        
        // 遍历修改经验条的10个区块
        int index = expPrgVal / 10;
        for (int i = 0;i < expPrgTrans.childCount;i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }
            else if (i == index)
            {
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0;
            }
        }
        
        
    }

    #region Click Events

    private void RegClickEvents()
    {
        OnClickDown(imgDirBg.gameObject,(PointerEventData data) =>
        {
            startPos = data.position;
            SetActive(imgDirPoint);
            imgDirBg.transform.position = data.position;
        });
        
        OnClickUp(imgDirBg.gameObject,(PointerEventData data) =>
        {
            imgDirBg.transform.position = defaultPos;
            SetActive(imgDirPoint,false);
            imgDirPoint.transform.localPosition = Vector3.zero;
            // TODO 方向信息传递
            BattleSys.Instance.SetMoveDir(Vector2.zero);
        });
        
        OnDrag(imgDirBg.gameObject,(PointerEventData data) =>
        {
            Vector2 dir = data.position - startPos;
            float len = dir.magnitude;
            if (len > pointDis)
            {
                // 把方向向量限制到maxLength的长度
                Vector2 clampDir = Vector2.ClampMagnitude(dir,pointDis);
                imgDirPoint.transform.position = startPos + clampDir;
            }
            else
            {
                imgDirPoint.transform.position = data.position;
            }
            // TODO 方向信息传递
            BattleSys.Instance.SetMoveDir(dir.normalized);
        });
    }
    
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWindowState(false);
    }
    
    public void ClickNormalAtk()
    {
        BattleSys.Instance.ReqReleaseSkill(0);
    }
    
    public void ClickSkill1()
    {
        BattleSys.Instance.ReqReleaseSkill(1);
    }

    public void ClickSkill2()
    {
        BattleSys.Instance.ReqReleaseSkill(2);
    }
    
    public void ClickSkill3()
    {
        BattleSys.Instance.ReqReleaseSkill(3);
    }
    
    #endregion
   
    
}