using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;
using System.Text;

public class HotfixScript : MonoBehaviour
{
    private LuaEnv luaenv;

    private static Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        luaenv = new LuaEnv();
        luaenv.AddLoader(MyLoader);
        luaenv.DoString("require 'fish'");
    }

    // Use this for initialization
    void Start()
    {
        // luaenv = new LuaEnv();
        //       luaenv.AddLoader(MyLoader);
        //       luaenv.DoString("require 'fish'");
    }

    // Update is called once per frame
    void Update()
    {
    }


    void OnDestroy()
    {
        luaenv.DoString("require 'fishDispose'");
        luaenv.Dispose();
    }


    byte[] MyLoader(ref string filepath)
    {
        string path =
            @"D:\github\Unity\StudyCodes\SIKI_EDU\20210627-XLua_JoyFish\XLua_FishingJoy\Assets\PlayerGamePackage\" +
            filepath + ".lua.txt";
        return Encoding.UTF8.GetBytes(File.ReadAllText(path));
    }

    [LuaCallCSharp]
    public static void LoadResource(string resName, string filePath)
    {
        AssetBundle ab = AssetBundle.LoadFromFile(
            @"D:\github\Unity\StudyCodes\SIKI_EDU\20210627-XLua_JoyFish\XLua_FishingJoy\AssetBundles\" + filePath);
        GameObject go = ab.LoadAsset<GameObject>(resName);
        prefabDict.Add(resName,go);
    }

    [LuaCallCSharp]
    public static GameObject GetGameObject(string goName)
    {
        return prefabDict[goName];
    }
}