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
		PECommon.Log("CfgSvc Init Done.");
	}

	private Dictionary<int, AutoGuideCfg> autoGuideCfgDataDic = new Dictionary<int, AutoGuideCfg>();

	private void InitAutoGuideCfg()
	{
		XmlDocument doc = new XmlDocument();

		DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory);
		dir = dir.Parent.Parent.Parent.Parent.Parent;
		var path =  Path.Combine(dir.FullName, "DarkGod", "Assets", "Resources", "ResCfgs", "guide.xml");
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

}



public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
	public int coin;
	public int exp;
}

public class BaseData<T>
{
	public int ID;
}