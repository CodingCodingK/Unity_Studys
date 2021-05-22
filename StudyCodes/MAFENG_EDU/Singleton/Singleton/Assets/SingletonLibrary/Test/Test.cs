using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // No MonoBehaviour Singleton Test :
        SingletonTest.Instance.MyMethod();

        // MonoBehaviour Singleton Test :
        MonoSingletonTest.Instance.MyMethod();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.A))
		{
            SceneManager.LoadScene("02");
		}
    }
}

public class SingletonTest : Singleton<SingletonTest>
{
    public void MyMethod()
	{
        Debug.Log("Singleton Test...");
	}
}

public class MonoSingletonTest : MonoSingleton<MonoSingletonTest>
{
    public MonoSingletonTest()
	{
        IsDestoryOnLoad = true;
    }

    public void MyMethod()
    {
        Debug.Log("MonoSingleton Test...");
    }

	void Start()
	{
        AddSceneChangeEvent();
    }

	void Update()
	{
		
	}
}
