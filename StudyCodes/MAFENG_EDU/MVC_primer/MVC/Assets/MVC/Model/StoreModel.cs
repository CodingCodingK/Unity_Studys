using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreModel : Singleton<StoreModel>
{
	private Dictionary<int, Commodity> dic = new Dictionary<int, Commodity>();

	public void Add(Commodity c)
	{
		if(!dic.ContainsKey(c.id))
		{
			dic.Add(c.id, c);
		}
	}

	public Commodity Get(int id)
	{
		return Instance.dic[id];
	}

}

