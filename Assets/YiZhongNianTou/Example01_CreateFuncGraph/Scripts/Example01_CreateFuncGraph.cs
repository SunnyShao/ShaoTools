using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example01_CreateFuncGraph : MonoBehaviour
{
    [Header("构建视图的预制体")]
    public Transform pointPrefab;

    [Header("分辨率滑块")]
    [Range(10, 100)]
    public int resolution = 50;

    private Transform[] points;

    // Start is called before the first frame update
    void Start()
    {
        points = new Transform[resolution];
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 pos;
        pos.z = 0f;
        pos.y = 0f;

        for (int i = 0; i < points.Length; i++)
        {
            Transform trans = Instantiate(pointPrefab);
            pos.x = (i + 0.5f) * step - 1f;
            trans.localPosition = pos;
            trans.localScale = scale;
            trans.SetParent(transform , false);
            points[i] = trans;
        }

        //Y = X
        //Vector3 scale = Vector3.one / 5f;
        //Vector3 pos;
        //pos.z = 0f;
        //for (int i = 0; i < 10; i++)
        //{
        //    Transform trans = Instantiate(pointPrefab);
        //    pos.x = (i + 0.5f) / 5f - 1f;
        //    pos.y = pos.x;
        //    trans.localPosition = pos;
        //    trans.localScale = scale;
        //}


        //Y = X * X (抛物线)
        //Vector3 scale = Vector3.one / 5f;
        //Vector3 pos;
        //pos.z = 0f;
        //for (int i = 0; i < 10; i++)
        //{
        //    Transform trans = Instantiate(pointPrefab);
        //    pos.x = (i + 0.5f) / 5f - 1f;
        //    pos.y = pos.x * pos.x;
        //    trans.localPosition = pos;
        //    trans.localScale = scale;
        //}
    }

    private void Update()
    {
        //若要让此函数动起来，可以在计算正弦函数之前将当前游戏时间添加到X上。如果我们也通过π缩放时间，这个函数将每两秒重复一次。所以使用f(x，t)=sin(π(x+t))，其中t是经过的游戏时间。这将推动正弦波随着时间的推移，向负X方向移动。
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i];
            Vector3 pos = point.localPosition;
            pos.y = Mathf.Sin((pos.x + Time.time)* Mathf.PI);
            point.localPosition = pos;
        }
    }
}