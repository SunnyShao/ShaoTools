using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundle
{
    
    [MenuItem("Tools/Create AssetBunle")]
    private static void BuildAssetBundle()
    {
        string abPath = "Assets/AssetBundles";

        if(!Directory.Exists(abPath))
        {
            Directory.CreateDirectory(abPath);
        }

        BuildPipeline.BuildAssetBundles(abPath, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

}
