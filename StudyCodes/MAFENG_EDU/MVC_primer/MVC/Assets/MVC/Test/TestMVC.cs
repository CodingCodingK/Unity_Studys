using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMVC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
	    WindowManager.Instance.OpenWindow(WindowsType.StoreWindow);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
