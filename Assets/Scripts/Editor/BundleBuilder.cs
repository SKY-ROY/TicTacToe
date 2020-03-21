using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
/// <summary>
/// This script is basically used for packing prefabs into asset-bundles, which can later be placed on persistentDataPath so that the application can access it during run time
/// </summary>
public class BundleBuilder : Editor
{
    [MenuItem("Assets/ Build AssetBundles")]
    
    static void BuildAllAssetBundles()
    {
        string systemUserName = "akash";

        string consistentSavePath = Application.persistentDataPath + "/AssetBundles";//persistentDataPath retuens a string that redirects to the a directory path which varies based on target platform


        BuildPipeline.BuildAssetBundles(consistentSavePath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);//Save or Place AssetBundle in persistent DataPath based on your target platform though any means
        BuildPipeline.BuildAssetBundles(@"C:\Users\" + systemUserName + @"\Desktop\AssetBundles", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);//Save it from editor on "C:\Users\<systemUsername>\Desktop\AssetBundles"
    }
}
