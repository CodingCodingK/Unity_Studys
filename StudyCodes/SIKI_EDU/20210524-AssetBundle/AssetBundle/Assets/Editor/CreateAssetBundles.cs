using System.IO;
using UnityEditor;

public class CreateAssetBundles
{
	[MenuItem("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles()
	{
		string dir = "AssetBundles";
		if (!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir);
		}

		BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
	}
}
