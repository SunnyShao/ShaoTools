using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example01_CreateFuncGraph : MonoBehaviour
{
    [Header("������ͼ��Ԥ����")]
    public Transform pointPrefab;

    [Header("�ֱ��ʻ���")]
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


        //Y = X * X (������)
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
        //��Ҫ�ô˺����������������ڼ������Һ���֮ǰ����ǰ��Ϸʱ����ӵ�X�ϡ��������Ҳͨ��������ʱ�䣬���������ÿ�����ظ�һ�Ρ�����ʹ��f(x��t)=sin(��(x+t))������t�Ǿ�������Ϸʱ�䡣�⽫�ƶ����Ҳ�����ʱ������ƣ���X�����ƶ���
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i];
            Vector3 pos = point.localPosition;
            pos.y = Mathf.Sin((pos.x + Time.time)* Mathf.PI);
            point.localPosition = pos;
        }
    }
}