/****************************************************
    文件：MainCityWIndow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/14 21:38:54
    功能：主城窗口
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Data;
using Common;
using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCityWindow : WindowBase
{
    #region UI define

    // 拖拽范围
    public Image imgTouch;
    // 拖拽背景盘
    public Image imgDirBg;
    // 拖拽点
    public Image imgDirPoint;

    public Text txtFight;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;
    
    /// <summary>
    /// 经验进度条
    /// </summary>
    public Transform expPrgTrans;

    /// <summary>
    /// 摇杆用 拖拽起始点
    /// </summary>
    private Vector2 startPos = Vector2.zero;
    
    private Vector2 defaultPos = Vector2.zero;
        
    #endregion

    #region Main Func

    protected override void InitWindow()
    {
        base.InitWindow();

        defaultPos = imgDirBg.transform.position;
        
        RegisterTouchEvents();
        SetActive(imgDirPoint,false);
        RefreshUI();
    }

    /// <summary>
    /// 根据数据库取得的 PlayerData 更新 UI
    /// </summary>
    private void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance().PlayerData;
        SetText(txtFight,PECommon.GetFight(pd));
        SetText(txtPower,$"{pd.power}/{PECommon.GetPowerLimit(pd.level)}");
        imgPowerPrg.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimit(pd.level);
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

    #endregion

    #region Click Event
    
    public void ClickMenuButton()
    {
        // TODO
    }
    
    /// <summary>
    /// 托盘事件
    /// </summary>
    public void RegisterTouchEvents()
    {
        OnClickDown(imgDirBg.gameObject,data =>
        {
            startPos = data.position;
            SetActive(imgDirPoint);
            imgDirBg.transform.position = data.position;
        });
        
        OnClickUp(imgDirBg.gameObject,data =>
        {
            imgDirBg.transform.position = defaultPos;
            SetActive(imgDirBg,false);
            imgDirPoint.transform.localPosition = Vector3.zero;
            // TODO 方向信息传递
        });
        
        OnDrag(imgDirBg.gameObject,data =>
        {
            Vector2 dir = data.position - startPos;
            
        });
    }

    #endregion
    
    
}
