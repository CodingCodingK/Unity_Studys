/****************************************************
    文件：ResSvc.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/12 21:50:41
    功能：资源加载服务
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using PEProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;
using LogType = PEProtocol.LogType;
using MyHelper;

public class ResSvc : GameRootMonoSingleton<ResSvc>
{
    // private Action prgCallBack = null;

    /// <summary>
    /// Audio暂存池
    /// </summary>
    private Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();
    
    /// <summary>
    /// Prefab暂存池
    /// </summary>
    private Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
    
    /// <summary>
    /// Sprite暂存池
    /// </summary>
    private Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
    
    
    public void InitSvc()
    {
        Debug.Log("ResSvc Init Completed.");
        InitRDNameConfig(PathDefine.RDNameConfig);
        InitMonsterCfg(PathDefine.MonsterConfig);
        InitMapCfg(PathDefine.MapConfig);
        InitAutoGuideCfg(PathDefine.AutoGuideConfig);
        InitStrongCfg(PathDefine.StrongConfig);
        InitTaskCfg(PathDefine.TaskConfig);
        InitSkillCfg(PathDefine.SkillConfig);
        InitSkillMoveCfg(PathDefine.SkillMoveConfig);
        InitSkillActionCfg(PathDefine.SkillActionConfig);

    }
    
    private Action sceneBPMethod = null;
    public void AsyncLoadScene(string sceneName,Action afterAll)
    {
        StartCoroutine(StartLoading(sceneName,afterAll));
        
        // GameRootResources.Instance().loadingWindow.SetWindowState();
        //
        // AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
        // //sceneAsync.allowSceneActivation = true;
        // prgCB = () => {
        //     float val = sceneAsync.progress;
        //     GameRootResources.Instance().loadingWindow.SetProgress(val);
        //     if (val >= 0.9) {
        //         if (afterAll != null) {
        //             afterAll();
        //         }
        //         prgCB = null;
        //         sceneAsync = null;
        //         GameRootResources.Instance().loadingWindow.SetWindowState(false);
        //     }
        // };
    }

    private void Update()
    {
        if (sceneBPMethod != null)
        {
            sceneBPMethod();
            sceneBPMethod = null;
        }
    }
    
    /// <summary>
    /// 优化进度读取：协程刷新进度
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator StartLoading(string sceneName,Action afterAll)
    {
        var loadingWindow = GameRootResources.Instance().loadingWindow;
        loadingWindow.SetWindowState(true);
        
        int displayProgress = 0;
        int toProgress = 0;
        // 卸载当前场景
        
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName); 
        
        // 不让场景自动跳转，progress也最多只能到90%
        op.allowSceneActivation = false;
        
        while (op.progress < 0.9f)
        {
            toProgress = (int)(op.progress * 100);
            // Debug.Log("below90: " + displayProgress + " , " + op.progress + " , " + toProgress);
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                GameRootResources.Instance().loadingWindow.SetProgress(displayProgress);
                yield return new WaitForEndOfFrame();
            }
        }

        toProgress = 100;
       
        while (displayProgress < toProgress)
        {
            // Debug.Log("over90: " + displayProgress + " , " + op.progress);
            ++displayProgress;
            GameRootResources.Instance().loadingWindow.SetProgress(displayProgress);
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
        
        loadingWindow.SetWindowState(false);
        
        // 赋值回调函数
        sceneBPMethod = afterAll;
        
    }

    /// <summary>
    /// 加载Audio
    /// </summary>
    /// <param name="path"></param>
    /// <param name="isCache">是否需要放进缓存字典中</param>
    /// <returns></returns>
    public AudioClip LoadAudio(string path, bool isCache = true)
    {
        AudioClip au = null;
        if (!audioDic.TryGetValue(path,out au))
        {
            au = Resources.Load<AudioClip>(path);
            if (isCache)
            {
                audioDic[path] = au;
            }
        }
        
        return au;
    }

    public GameObject LoadPrefab(string path, bool isCache = false)
    {
        GameObject prefab = null;
        if (!prefabDic.TryGetValue(path,out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (isCache)
            {
                prefabDic[path] = prefab;
            }
        }
        
        // 再实例化一下
        // GameObject go = null;
        // if (prefab != null)
        // {
        //     go = Instantiate(prefab);
        // }

        return Instantiate(prefab);
    }
    
    public GameObject GetPrefab(string path)
    {
        GameObject prefab = null;
        prefabDic.TryGetValue(path, out prefab);
        return prefab;
    }

    public Sprite LoadSprite(string path,bool cache = false)
    {
        Sprite sp = null;
        if (!spriteDic.TryGetValue(path,out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if (cache)
            {
                spriteDic.Add(path,sp);
            }
        }

        return sp;
    }
    

    #region Configs

    #region 随机名字

    private List<string> surnameList = new List<string>();
    private List<string> manList = new List<string>();
    private List<string> womanList = new List<string>();

    /// <summary>
    /// 读取随机名字配置文件
    /// </summary>
    private void InitRDNameConfig(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:"+ path + "not exist",LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                var eleID = ele.GetAttributeNode("ID");
                if (eleID == null)
                {
                    continue;
                }

                int ID = Convert.ToInt32(eleID.InnerText);
                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "surname":
                            surnameList.Add(e.InnerText);
                            break;
                        case "man":
                            manList.Add(e.InnerText);
                            break;
                        case "woman":
                            womanList.Add(e.InnerText);
                            break;
                    }
                }
            }
        }
    }

    public string GetRDName(bool man = true)
    {
        var surname = surnameList[PETools.RDInt(0, surnameList.Count - 1)];
        var givenName = man
            ? manList[PETools.RDInt(0, manList.Count - 1)]
            : womanList[PETools.RDInt(0, womanList.Count - 1)];
        return surname + givenName;
    }

    #endregion

    #region 配置数据

    #region 地图配置

    private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();
    
     
    public void InitMapCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist",LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            
            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                var eleID = ele.GetAttributeNode("ID");
                if (eleID == null)
                {
                    continue;
                }

                int id = Convert.ToInt32(eleID.InnerText);
                MapCfg dto = new MapCfg
                {
                    ID = id,
                    monsterList = new List<MonsterData>(),
                };

                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mapName":
                            dto.mapName = e.InnerText;
                            break;
                        case "sceneName":
                            dto.sceneName = e.InnerText;
                            break;
                        case "power":
                            dto.power = Int32.Parse(e.InnerText);
                            break;
                        case "mainCamPos":
                            dto.mainCamPos = MapperHelper.ConvertToVector3(e.InnerText);
                            break;
                        case "mainCamRote":
                            dto.mainCamRote = MapperHelper.ConvertToVector3(e.InnerText);
                            break;
                        case "playerBornPos":
                            dto.playerBornPos = MapperHelper.ConvertToVector3(e.InnerText);
                            break;
                        case "playerBornRote":
                            dto.playerBornRote = MapperHelper.ConvertToVector3(e.InnerText);
                            break;
                        case "monsterLst":
                        {
                            var waveList = e.InnerText.Split('#');
                            for (int index = 0;index< waveList.Length;index++)
                            {
                                if (index==0)
                                {
                                    continue;
                                }

                                var monsterList = waveList[index].Split('|');
                                for (var index2 = 0; index2 < monsterList.Length; index2++)
                                {
                                    var monster = monsterList[index2];
                                    if (index2 == 0)
                                    {
                                        continue;
                                    }

                                    var dataString = monster.Split(',');
                                    MonsterData data = new MonsterData()
                                    {
                                        ID = int.Parse(dataString[0]),
                                        mWave = index,
                                        mIndex = index2,
                                        mCfg = GetMonsterCfgData(int.Parse(dataString[0])),
                                        mBornPos = new Vector3(float.Parse(dataString[1]), float.Parse(dataString[2]),
                                            float.Parse(dataString[3])),
                                        mBornRote = new Vector3(0, float.Parse(dataString[4]), 0),
                                        mLevel = int.Parse(dataString[5]),
                                    };
                                    dto.monsterList.Add(data);
                                }
                            }


                        } break;
                           
                    }
                }

                mapCfgDataDic.Add(id,dto);
            }
        }
    }
    
    public MapCfg GetMapCfgData(int id)
    {
        return mapCfgDataDic[id];
    }

    #endregion

    #region 引导任务配置

    private Dictionary<int, AutoGuideCfg> autoGuideCfgDataDic = new Dictionary<int, AutoGuideCfg>();

    public void InitAutoGuideCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist",LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                var eleID = ele.GetAttributeNode("ID");
                if (eleID == null)
                {
                    continue;
                }

                int id = Convert.ToInt32(eleID.InnerText);
                AutoGuideCfg dto = new AutoGuideCfg
                {
                    ID = id,
                };

                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "npcID":
                            dto.npcID = Convert.ToInt32(e.InnerText);
                            break;
                        case "dilogArr":
                            dto.dilogArr = e.InnerText;
                            break;
                        case "actID":
                            dto.actID = Convert.ToInt32(e.InnerText);
                            break;
                        case "coin":
                            dto.coin = Convert.ToInt32(e.InnerText);
                            break;
                        case "exp":
                            dto.exp = Convert.ToInt32(e.InnerText);
                            break;
                       
                    }
                }

                autoGuideCfgDataDic.Add(id,dto);
            }
        }
    }
    
    public AutoGuideCfg GetAutoGuideData(int id)
    {
        autoGuideCfgDataDic.TryGetValue(id, out AutoGuideCfg cfg);
        return cfg;
    }

    #endregion

    #region 强化阶段配置

    private Dictionary<int, Dictionary<int,StrongCfg>> strongCfgDataDic = new Dictionary<int, Dictionary<int,StrongCfg>>();

    public void InitStrongCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist",LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                var eleID = ele.GetAttributeNode("ID");
                if (eleID == null)
                {
                    continue;
                }

                int id = Convert.ToInt32(eleID.InnerText);
                StrongCfg dto = new StrongCfg
                {
                    ID = id,
                };

                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "pos":
                            dto.pos = Convert.ToInt32(e.InnerText);
                            break;
                        case "starlv":
                            dto.starlv = Convert.ToInt32(e.InnerText);
                            break;
                        case "addhp":
                            dto.addhp = Convert.ToInt32(e.InnerText);
                            break;
                        case "addhurt":
                            dto.addhurt = Convert.ToInt32(e.InnerText);
                            break;
                        case "adddef":
                            dto.adddef = Convert.ToInt32(e.InnerText);
                            break;
                        case "minlv":
                            dto.minlv = Convert.ToInt32(e.InnerText);
                            break;
                        case "coin":
                            dto.coin = Convert.ToInt32(e.InnerText);
                            break;
                        case "crystal":
                            dto.crystal = Convert.ToInt32(e.InnerText);
                            break;
                    }
                }

                Dictionary<int, StrongCfg> dic = null;
                if (strongCfgDataDic.TryGetValue(dto.pos,out dic))
                {
                    dic.Add(dto.starlv,dto);
                }
                else
                {
                    dic = new Dictionary<int, StrongCfg>();
                    dic.Add(dto.starlv,dto);
                    strongCfgDataDic.Add(dto.pos,dic);
                }
                
            }
        }
    }
    
    public StrongCfg GetStrongData(int pos,int starlv)
    {
        Dictionary<int, StrongCfg> dic = null;
        if (strongCfgDataDic.TryGetValue(pos, out dic))
        {
            if (dic.ContainsKey(starlv))
            {
                return dic[starlv];
            }
        }
        
        return null;
    }

    public int GetPropStrongAddVal(int pos,int startLv,int type)
    {
        if (strongCfgDataDic.TryGetValue(pos,out Dictionary<int,StrongCfg> list))
        {
            if (list.TryGetValue(startLv,out StrongCfg data))
            {
                switch (type)
                {
                    case 1:
                        return data.addhp;
                    case 2:
                        return data.addhurt;
                    case 3:
                        return data.adddef;
                    
                } 
            }
        }
        return 0;
    }
    
    
    #endregion
    
    #region 任务配置

    private Dictionary<int,TaskCfg> taskDataDic = new Dictionary<int,TaskCfg>();

    public void InitTaskCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist",LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                var eleID = ele.GetAttributeNode("ID");
                if (eleID == null)
                {
                    continue;
                }

                int id = Convert.ToInt32(eleID.InnerText);
                TaskCfg dto = new TaskCfg
                {
                    ID = id,
                };

                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "taskName":
                            dto.taskName = Convert.ToString(e.InnerText);
                            break;
                        case "coin":
                            dto.coin = Convert.ToInt32(e.InnerText);
                            break;
                        case "count":
                            dto.count = Convert.ToInt32(e.InnerText);
                            break;
                        case "exp":
                            dto.exp = Convert.ToInt32(e.InnerText);
                            break;
                    }
                }
                
                taskDataDic.Add(id,dto);
            }
        }
    }
    
    public TaskCfg GetTaskData(int id)
    {
        taskDataDic.TryGetValue(id, out TaskCfg cfg);
        return cfg;
    }

    #endregion
    
    #region 技能配置

    private Dictionary<int, SkillCfg> skillCfgDataDic = new Dictionary<int, SkillCfg>();
    
     
    public void InitSkillCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist",LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                var eleID = ele.GetAttributeNode("ID");
                if (eleID == null)
                {
                    continue;
                }

                int id = Convert.ToInt32(eleID.InnerText);
                SkillCfg dto = new SkillCfg
                {
                    ID = id,
                    skillMoveLst = new List<int>(),
                    skillActionLst = new List<int>(),
                    skillDamageLst = new List<int>(),
                };

                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "skillName":
                            dto.skillName = e.InnerText;
                            break;
                        case "skillTime":
                            dto.skillTime = Int32.Parse(e.InnerText);
                            break;
                        case "cdTime":
                            dto.cdTime = Int32.Parse(e.InnerText);
                            break;
                        case "aniAction":
                            dto.aniAction = Int32.Parse(e.InnerText);
                            break;
                        case "fx":
                            dto.fx = e.InnerText;
                            break;
                        case "skillMoveLst":
                            dto.skillMoveLst = MapperHelper.ConvertToIntList(e.InnerText);
                            break;
                        case "skillActionLst":
                            dto.skillActionLst = MapperHelper.ConvertToIntList(e.InnerText);
                            break;
                        case "skillDamageLst":
                            dto.skillDamageLst = MapperHelper.ConvertToIntList(e.InnerText);
                            break;
                        case "dmgType":
                            dto.dmgType = Int32.Parse(e.InnerText).Equals("1") ? DamageType.AD : DamageType.AP ;
                            break;
                        
                    }
                }

                skillCfgDataDic.Add(id,dto);
            }
        }
    }
    
    public SkillCfg GetSkillCfgData(int id)
    {
        return skillCfgDataDic[id];
    }

    
    private Dictionary<int, SkillMoveCfg> SkillMoveCfgDataDic = new Dictionary<int, SkillMoveCfg>();
    
     
    public void InitSkillMoveCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist",LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                var eleID = ele.GetAttributeNode("ID");
                if (eleID == null)
                {
                    continue;
                }

                int id = Convert.ToInt32(eleID.InnerText);
                SkillMoveCfg dto = new SkillMoveCfg
                {
                    ID = id,
                };

                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "delayTime":
                            dto.delayTime = Int32.Parse(e.InnerText);
                            break;
                        case "moveDis":
                            dto.moveDis = float.Parse(e.InnerText);
                            break;
                        case "moveTime":
                            dto.moveTime = Int32.Parse(e.InnerText);
                            break;
                    }
                }

                SkillMoveCfgDataDic.Add(id,dto);
            }
        }
    }
    
    public SkillMoveCfg GetSkillMoveCfgData(int id)
    {
        return SkillMoveCfgDataDic[id];
    }
    
    private Dictionary<int, SkillActionCfg> SkillActionCfgDataDic = new Dictionary<int, SkillActionCfg>();
    
     
    public void InitSkillActionCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist",LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                var eleID = ele.GetAttributeNode("ID");
                if (eleID == null)
                {
                    continue;
                }

                int id = Convert.ToInt32(eleID.InnerText);
                SkillActionCfg dto = new SkillActionCfg
                {
                    ID = id,
                };

                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "delayTime":
                            dto.delayTime = Int32.Parse(e.InnerText);
                            break;
                        case "angle":
                            dto.angle = float.Parse(e.InnerText);
                            break;
                        case "radius":
                            dto.radius = float.Parse(e.InnerText);
                            break;
                    }
                }

                SkillActionCfgDataDic.Add(id,dto);
            }
        }
    }
    
    public SkillActionCfg GetSkillActionCfgData(int id)
    {
        return SkillActionCfgDataDic[id];
    }
    
    #endregion

    #region Monster
    
    private Dictionary<int, MonsterCfg> MonsterCfgDataDic = new Dictionary<int, MonsterCfg>();
    
    public void InitMonsterCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist",LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                var eleID = ele.GetAttributeNode("ID");
                if (eleID == null)
                {
                    continue;
                }

                int id = Convert.ToInt32(eleID.InnerText);
                MonsterCfg dto = new MonsterCfg
                {
                    ID = id,
                    props = new BattleProps(),
                };

                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mName":
                            dto.mName = e.InnerText;
                            break;
                        case "resPath":
                            dto.resPath = e.InnerText;
                            break;
                        case "hp":
                            dto.props.hp = Int32.Parse(e.InnerText);
                            break;
                        case "ad":
                            dto.props.ad = Int32.Parse(e.InnerText);
                            break;
                        case "ap":
                            dto.props.ap = Int32.Parse(e.InnerText);
                            break;
                        case "addef":
                            dto.props.addef = Int32.Parse(e.InnerText);
                            break;
                        case "apdef":
                            dto.props.apdef = Int32.Parse(e.InnerText);
                            break;
                        case "dodge":
                            dto.props.dodge = Int32.Parse(e.InnerText);
                            break;
                        case "pierce":
                            dto.props.pierce = Int32.Parse(e.InnerText);
                            break;
                        case "critical":
                            dto.props.critical = Int32.Parse(e.InnerText);
                            break;
                    }
                }

                MonsterCfgDataDic.Add(id,dto);
            }
        }
    }
    
    public MonsterCfg GetMonsterCfgData(int id)
    {
        return MonsterCfgDataDic[id];
    }

    #endregion
    
    #endregion
    
    

    #endregion
}
