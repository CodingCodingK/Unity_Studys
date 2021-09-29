using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MySql.Data;
using PEProtocol;

/// <summary>
/// 配置数据服务
/// </summary>
public class CfgSvc : Singleton<CfgSvc>
{
	private CfgSvc() { }

	public void Init()
	{
		InitAutoGuideCfg();
		InitStrongCfg();
		InitTaskCfg();
		PECommon.Log("CfgSvc Init Done.");
	}

	#region GuideCfg

	private Dictionary<int, AutoGuideCfg> autoGuideCfgDataDic = new Dictionary<int, AutoGuideCfg>();

	private void InitAutoGuideCfg()
	{
		XmlDocument doc = new XmlDocument();

		DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory);
		dir = dir.Parent.Parent.Parent.Parent.Parent;
		var path = Path.Combine(dir.FullName, "DarkGod", "Assets", "Resources", "ResCfgs", "guide.xml");
		doc.Load(path);

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
					case "coin":
						dto.coin = Convert.ToInt32(e.InnerText);
						break;
					case "exp":
						dto.exp = Convert.ToInt32(e.InnerText);
						break;

				}
			}

			autoGuideCfgDataDic.Add(id, dto);
		}

	}

	public AutoGuideCfg GetAutoGuideData(int id)
	{
		return autoGuideCfgDataDic[id];
	}

	#endregion

	#region StrongCfg

	private Dictionary<int, Dictionary<int, StrongCfg>> strongCfgDataDic = new Dictionary<int, Dictionary<int, StrongCfg>>();

	public void InitStrongCfg()
	{
		XmlDocument doc = new XmlDocument();

		DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory);
		dir = dir.Parent.Parent.Parent.Parent.Parent;
		var path = Path.Combine(dir.FullName, "DarkGod", "Assets", "Resources", "ResCfgs", "strong.xml");
		doc.Load(path);
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
			if (strongCfgDataDic.TryGetValue(dto.pos, out dic))
			{
				dic.Add(dto.starlv, dto);
			}
			else
			{
				dic = new Dictionary<int, StrongCfg>();
				dic.Add(dto.starlv, dto);
				strongCfgDataDic.Add(dto.pos, dic);
			}
		}
	}

	public StrongCfg GetStrongData(int pos, int starlv)
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

	#endregion

	#region TaskCfg

	private Dictionary<int, TaskCfg> taskCfgDataDic = new Dictionary<int, TaskCfg>();

	private void InitTaskCfg()
	{
		XmlDocument doc = new XmlDocument();

		DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory);
		dir = dir.Parent.Parent.Parent.Parent.Parent;
		var path = Path.Combine(dir.FullName, "DarkGod", "Assets", "Resources", "ResCfgs", "taskreward.xml");
		doc.Load(path);

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
					case "count":
						dto.count = Convert.ToInt32(e.InnerText);
						break;
					case "exp":
						dto.exp = Convert.ToInt32(e.InnerText);
						break;
					case "coin":
						dto.coin = Convert.ToInt32(e.InnerText);
						break;

				}
			}

			taskCfgDataDic.Add(id, dto);
		}

	}

	public TaskCfg GetTaskData(int id)
	{
		return taskCfgDataDic[id];
	}
	#endregion
}


public class BaseData<T>
{
	public int ID;
}

public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
	public int coin;
	public int exp;
}

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