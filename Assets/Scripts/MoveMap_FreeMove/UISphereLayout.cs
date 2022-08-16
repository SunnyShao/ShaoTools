using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISphereLayout : MonoBehaviour
{
    public GameObject spawnObj;
    public int _count;
    [Header("半径")]
    public float _radius;
    [Header("平均份数")]
    public float _average;

    void Start()
    {
        Vector2 zeroPos = spawnObj.GetComponent<RectTransform>().anchoredPosition;
        for (int i = 0; i < _count; i++)
        {
            var obj = Instantiate(spawnObj, transform);
            float rad = 360/ _average * i;
            Debug.LogError(rad);
            float x = zeroPos.x + _radius * Mathf.Cos(rad * Mathf.PI / 180);
            float y = zeroPos.y + _radius * Mathf.Sin(rad * Mathf.PI / 180);
            Debug.Log(x + ":::" + y);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        }
    }

    void Update()
    {

    }
}
