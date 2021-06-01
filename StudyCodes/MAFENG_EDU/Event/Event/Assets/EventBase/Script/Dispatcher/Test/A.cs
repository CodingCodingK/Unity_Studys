using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetEvent.Instance.AddEventListener(1001, Login);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnDestroy()
	{
		NetEvent.Instance.RemoveEventListener(1001, Login);
    }

	void Login(string name)
    {
        Debug.Log($"AccessLog:{name} µÇÂ½ÁË");
    }
}
