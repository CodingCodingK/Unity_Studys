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
    public int coin;
    public int exp;
    public int crystal;
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
    public int cdTime;
    public int aniAction;
    public string fx;
    public bool isCombo;
    public bool isCollide;
    public bool isBreak;
    public DamageType dmgType;
    /// skillmove配置表对应的ID List
    public List<int> skillMoveLst;
    /// skillaction配置表对应的ID List
    public List<int> skillActionLst;
    /// skillaction配置表对应的list的每一段伤害值
    public List<int> skillDamageLst;
}

public class SkillMoveCfg : BaseData<SkillMoveCfg>
{
    public int delayTime;
    public int moveTime;
    public float moveDis;
   
}

public class SkillActionCfg : BaseData<SkillActionCfg>
{
    public int delayTime;
    public float radius;
    public float angle;
   
}

public class MonsterCfg : BaseData<MonsterCfg>
{
    public string mName;
    public string resPath;
    /// 1：普通怪物 2：boss怪物
    public MonsterType mType;
    public bool isStop;
    public int skillID;
    public float atkDis;
    public BattleProps props;
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
    public int mLevel;

    public Vector3 mBornPos;
    public Vector3 mBornRote;
}

public class BattleProps
{
    public int hp;
    public int ad;
    public int ap;
    public int addef;
    public int apdef;
    public int dodge;
    public int pierce;
    public int critical;
}

