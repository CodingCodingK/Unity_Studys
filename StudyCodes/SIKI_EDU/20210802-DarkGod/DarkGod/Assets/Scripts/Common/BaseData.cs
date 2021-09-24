/****************************************************
    文件：Contents.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/12 22:43:0
    功能：配置数据类
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// 配置数据类
public class BaseData<T>
{
    public int ID;
}


/// 地图配置
public class MapCfg : BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;
    
}


/// 自动引导任务配置
public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
    // 触发人物目标NPC索引号
    public int npcID;
    public string dilogArr;
    public int actID;
    public int coin;
    public int exp;
}

/// 装备强化阶段配置
public class StrongCfg : BaseData<StrongCfg>
{
    public int pos;
    public int starlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;

}