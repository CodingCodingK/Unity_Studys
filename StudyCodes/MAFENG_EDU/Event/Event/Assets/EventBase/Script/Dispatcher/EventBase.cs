using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Dictionary<X, List<Action<P>>>
/// </summary>
/// <typeparam name="T">child self</typeparam>
/// <typeparam name="P">value</typeparam>
/// <typeparam name="X">key</typeparam>
public class EventBase<T,P,X> where T : new() where P : class
{
	// Singleton
	private static T _instance;
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new T();
			}
			return _instance;
		}
	}

	// value = List<Action>
	private Dictionary<X, List<Action<P>>> dic = new Dictionary<X, List<Action<P>>>();

	public void AddEventListener(X key, Action<P> handle)
	{
		if (!dic.ContainsKey(key))
		{
			var actions = new List<Action<P>>(){ handle };
			dic.Add(key, actions);
		}
		else
		{
			dic[key].Add(handle);
		}
	}

	public void RemoveEventListener(X key, Action<P> handle)
	{
		if (dic.ContainsKey(key))
		{
			var actions = dic[key];
			if (actions.Count > 0)
			{
				actions.Remove(handle);
			}

			if (actions.Count == 0)
			{
				dic.Remove(key);
			}
		}
	}

	public void Dispatch(X key,P p)
	{
		if (dic.ContainsKey(key))
		{
			var actions = dic[key];
			if (actions != null && actions.Count > 0)
			{
				foreach (var action in actions)
				{
					action?.Invoke(p);
				}
			}
		}
	}

	public void Dispatch(X key)
	{
		Dispatch(key, null);
	}

}
