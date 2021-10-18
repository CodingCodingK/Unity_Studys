/****************************************************
    文件：Contents.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/12 22:43:0
    功能：常数定数
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TxtColor
{
    Red,
    Green,
    Blue,
    Yellow,
}

public enum DamageType
{
    None,
    AD = 1,
    AP = 2,
}

public enum EntityType
{
    None,
    Player,
    Monster,
}

public enum EntityState
{
    None,
    /// 霸体状态：不可控制，可受伤害
    BaTiState,
    
}

public static class Constants
{
    #region 颜色类

    private const string ColorRed = "<color=#FF0000FF>";
    private const string ColorGreen = "<color=#00FF00FF>";
    private const string ColorBlue = "<color=#00B4FFFF>";
    private const string ColorYellow = "<color=#FFFF00FF>";
    private const string ColorEnd = "</color>";

    public static string ColoredTxt(string str,TxtColor c)
    {
        string result = str;
        switch (c)
        {
            case TxtColor.Red:
                result = ColorRed + str + ColorEnd;
                break;
            case TxtColor.Green:
                result = ColorGreen + str + ColorEnd;
                break;
            case TxtColor.Blue:
                result = ColorBlue + str + ColorEnd;
                break;
            case TxtColor.Yellow:
                result = ColorYellow + str + ColorEnd;
                break;
        }

        return result;
    }
    
    #endregion
    
    #region 场景名称

    public const string SceneLogin = "SceneLogin";
    
    public const string SceneMainCity = "SceneMainCity";
    
    public const int MapID_MainCity = 10000;

    #endregion

    #region 音效名称

    // 登录bgm
    public const string BGLogin = "bgLogin";
    
    // 主城bgm
    public const string BGMainCity = "bgMainCity";
    
    // 主城bgm
    public const string BGHuangYe = "bgHuangYe";
    
    // 登录按钮音效
    public const string UILoginBtn = "uiLoginBtn";
    
    // 常规UI点击音效
    public const string UIClickBtn = "uiClickBtn";
    public const string UIExtenBtn = "uiExtenBtn";
    public const string UIOpenPage = "uiOpenPage";
    
    // 战斗音效
    public const string AssHit = "assassin_Hit";
    
    #endregion

    #region 屏幕标准宽高

    public const int ScreenStandardWidth = 1334;
    public const int ScreenStandardHeight = 750;

    /// 摇杆点标准操作最大距离
    public const int ScreenOPDis = 70;

    #endregion

    #region 角色属性类

    // 移动速度相关
    public const int PlayerMoveSpeed = 8;
    public const int MonsterMoveSpeed = 3;
    /// 运动平滑 最大加速度(单位秒)
    public const float AccelerSpeed = 4;
    // 混合参数
    public const int BlendIdle = 0;
    public const int BlendMove = 1;

    #endregion
    
    #region 自动导航
    public const int NPCWiseMan = 0;
    public const int NPCGeneral = 1;
    public const int NPCArtisan = 2;
    public const int NPCTrader = 3;

    #endregion
    
    #region 技能类

    // Action参数
    public const int ActionDefault = -1;
    public const int ActionBorn = 0;
    public const int ActionDie = 100;
    public const int ActionHit = 101;
    
    public const int DieAniTime = 5000;

    /// 血条平滑 最大加速度(单位秒)
    public const float AccelerHpSpeed = 0.2f;
    
    /// 普攻连招有效时间(单位毫秒)
    public const int ComboSpace = 500;
    
    #endregion

}
