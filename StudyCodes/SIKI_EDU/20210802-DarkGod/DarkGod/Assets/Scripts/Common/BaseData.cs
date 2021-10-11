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
    public int power;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;
    public List<MonsterData> monsterList;
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

public class TaskCfg : BaseData<TaskCfg>
{
    public string taskName;
    public int count;
    public int exp;
    public int coin;
}

public class TaskData : BaseData<TaskData>
{
    // 进度
    public int prgs;
    public bool isTaken;
}

public class SkillCfg : BaseData<SkillCfg>
{
    public string skillName;
    public int skillTime;
    public int aniAction;
    public string fx;
    /// skillmove配置表对应的ID
    public List<int> skillMoveLst;
}

public class SkillMoveCfg : BaseData<SkillMoveCfg>
{
    public int delayTime;
    public int moveTime;
    public float moveDis;
   
}

public class MonsterCfg : BaseData<MonsterCfg>
{
    public string mName;
    public string resPath;
}

/// <summary>
/// 怪物模型
/// </summary>
public class MonsterData : BaseData<MonsterData>
{
    /// 出现批次
    public int mWave;
    /// 个体编号
    public int mIndex;
    /// 模型信息
    public MonsterCfg mCfg;

    public Vector3 mBornPos;
    public Vector3 mBornRote;
}
