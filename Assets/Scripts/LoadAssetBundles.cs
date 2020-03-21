using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/// <summary>
/// This script deals with Loading of Assets from AssetBundles, but before loading any assets from AssetBundles we havve to create AssetBundles.
/// </summary>
public class LoadAssetBundles : MonoBehaviour
{
    public AssetBundle myLoadedAssetBundle;//type-AssetBundle instance to be used in this script 
    
    public string localEditorPath;          //this path needds to be specified and has to be same as the path where we save AssetBundle from the editor through the BundleBuilder script
    public string assetName;                //name of the asset to be loaded and to instantiate
    public string assetBundleName;          //AssetBundle Name
    public GameObject parentObject;         //parent object reference where the Asset from AssetBundle needs to be instantiated
    
    void Start()
    {
        string consistentSavePath = Application.persistentDataPath + "/AssetBundles";   //here we are trying to create a folder named AssetBundle in our persistent data path which varies based on target platform. 
        if (!Directory.Exists(consistentSavePath ))
        {
            Debug.Log("PersistentDataPath/AssetBundles Path doesn't exist!");           //indication on console that the folder doesn't exist therefore create one
            Directory.CreateDirectory(consistentSavePath);
        }
        else
            Debug.Log("PersistentDataPath/AssetBundles Path exists!");                  //indication on console that the folder exists
        
        LoadAssetBundle(consistentSavePath + "/" + assetBundleName);                    //This can be used on BUILDS but we have to externally place the AssetBundle the folder through various medium like downloading from web, etc. 
        //LoadAssetBundle(localEditorPath + "/" + assetBundleName);                     //This can be used if you place the AssetBundles in your "C:\Users\<systemUsername>\Desktop\AssetBundles"

        InstantiateObjectFromBundle(assetName, parentObject.transform);                 //once the Assset has been loaded from AssetBundle we can Instantiate it.
    }
    
    void LoadAssetBundle(string bundleURL)
    {
        if (myLoadedAssetBundle != null)
        {
            myLoadedAssetBundle.Unload(false);
        }

        myLoadedAssetBundle = AssetBundle.LoadFromFile(bundleURL);                      //Loading form AssetBundle

        Debug.Log(myLoadedAssetBundle == null ? " Failed to load AssetBundle!" : " AssetBundle successfully loaded!");
    }

    void InstantiateObjectFromBundle(string name, Transform parent)
    {
        var prefab = myLoadedAssetBundle.LoadAsset(name);                               //taking the argument name for loading in the prefab GameObject
        
        Instantiate(prefab, parent);                                                    //Instantiating the Asset in Parent GameObject
    }
}
