using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;
using System.Text;

public class HotfixScript : MonoBehaviour {

	private LuaEnv luaenv;

	// Use this for initialization
	void Start () {
		luaenv = new LuaEnv();
        luaenv.AddLoader(MyLoader);
        luaenv.DoString("require 'fish'");
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnDestory()
    {
        luaenv.Dispose();
    }


    byte[] MyLoader(ref string filepath)
    {
        string path =
            @"D:\github\Unity\StudyCodes\SIKI_EDU\20210627-XLua_JoyFish\XLua_FishingJoy\Assets\PlayerGamePackage\" +
            filepath + ".lua.txt";
        return Encoding.UTF8.GetBytes(File.ReadAllText(path));
    }

    
}
