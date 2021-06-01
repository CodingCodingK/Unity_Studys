using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadFromFileExample : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
	    string path = @"AssetBundles/scene/wall.unity3d";
        GameObject go;
		// method 1
		//go = AssetBundle.LoadFromFile(path).LoadAsset<GameObject>("Wallet");

		// method 2
		//AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(path));
		//yield return request;
		//AssetBundle assetBundle = request.assetBundle;
		//go = assetBundle.LoadAsset<GameObject>("Wallet");

		// method 3
		//while (!Caching.ready)
		//{
		// yield return null;
		//}

		//using (var www = WWW.LoadFromCacheOrDownload(
		// @"file://C:\Github\Unity\StudyCodes\SIKI_EDU\20210524-AssetBundle\AssetBundle\AssetBundles\scene\wall.unity3d",
		// 1))
		//{
		//    yield return www;
		//    if (!string.IsNullOrEmpty(www.error))
		//    {
		//     Debug.Log(www.error);
		//        yield return null;
		//    }

		//    go = www.assetBundle.LoadAsset<GameObject>("Wallet");
		//}

		// method 4
		string uri = "http://localhost/AssetBundles/env.unity3d";
		using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(uri))//ÐÂ·½·¨
		{
			yield return uwr.SendWebRequest();

			if (uwr.result != UnityWebRequest.Result.Success)
			{
				Debug.Log(uwr.error);
			}
			else
			{
				// Get downloaded asset bundle
				AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
				go = bundle.LoadAsset<GameObject>("Cube");
				Instantiate(go);
			}
		}

		// load method 1
		var manifestAB = AssetBundle.LoadFromFile(@"AssetBundles/AssetBundles");
		AssetBundleManifest manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

		foreach (string name in manifest.GetAllAssetBundles())
		{
			Debug.Log(name);
		}

		string[] strs = manifest.GetAllDependencies("env.unity3d");
		foreach (string name in strs)
		{
			Debug.Log(name);
			AssetBundle.LoadFromFile($"AssetBundles/{name}");
		}

	}
}
