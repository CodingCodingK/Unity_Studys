using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	public static T Instance
	{
		get
		{
			if(MonoSingletonObject.go == null)
			{
				MonoSingletonObject.go = new GameObject("MonoSingletonObject");
				Object.DontDestroyOnLoad(MonoSingletonObject.go);
			}

			if(_instance == null)
			{
				_instance = MonoSingletonObject.go.AddComponent<T>();
			}

			return _instance;
		}
	}

	// Destory when Scene changed
	public static bool IsDestoryOnLoad;

	public void AddSceneChangeEvent()
	{
		SceneManager.activeSceneChanged += OnSceneChanged;
	}

	private void OnSceneChanged(Scene s1,Scene s2)
	{
		if (IsDestoryOnLoad)
		{
			if (_instance != null)
			{
				GameObject.DestroyImmediate(_instance);
			}
		}
	}

}

// Cache GameObject
public class MonoSingletonObject
{
	public static GameObject go;
}