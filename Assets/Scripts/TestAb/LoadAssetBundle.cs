using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAssetBundle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AssetBundle ab = AssetBundle.LoadFromFile("Assets/AssetBundles/test", 111);
        Debug.LogError(ab.name);
        //AssetBundle ab = AssetBundle.LoadFromFile("Assets/AssetBundles/test");
        //GameObject obj = ab.LoadAsset<GameObject>("test");
        //Instantiate(obj);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
