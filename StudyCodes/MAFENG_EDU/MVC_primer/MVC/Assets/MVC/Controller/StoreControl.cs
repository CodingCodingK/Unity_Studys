using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreCtrl : Singleton<StoreCtrl>
{
	//
	public void SaveCommodity(Commodity commodity)
	{
		StoreModel.Instance.Add(commodity);
	}

	public Commodity GetCommodity(int id)
	{
		return StoreModel.Instance.Get(id);
	}

}
