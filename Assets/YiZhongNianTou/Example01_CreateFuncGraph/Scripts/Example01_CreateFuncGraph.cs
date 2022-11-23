using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example01_CreateFuncGraph : MonoBehaviour
{
    [Header("构建视图的预制体")]
    public Transform pointPrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            Transform trans = Instantiate(pointPrefab);
            trans.localPosition = Vector3.right * (i / 5f - 1f);
            trans.localScale = Vector3.one / 5;
        }

    }
}