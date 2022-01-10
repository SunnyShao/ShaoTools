using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionTools : MonoBehaviour
{
    public Transform targetPos;

    Vector3 a;
    Vector3 b;

    void Start()
    {
        //Test01 四元数 * 向量
        //如果 transform positon 初始坐标为 (0,0,10)  经过 四元数和变量的相乘 表示这个向量按照这个四元数进行旋转之后得到的新的向量 也就是（10,0,0）
        //Quaternion q = Quaternion.Euler(0, 90, 0);
        //transform.position = q * transform.position;

        a = new Vector3(8, 4, 0);
        b = new Vector3(2, 1, 0);
        var c = Vector3.Cross(a, b);
        float d = a.magnitude / b.magnitude;
        Debug.LogError(d);
        //Debug.LogError(Vector3.Dot(c, b)) ;
        //Debug.LogError(Vector3.Dot(c, a)) ;
        //Debug.DrawLine(Vector3.zero, a, Color.red, 200);
        //Debug.DrawLine(Vector3.zero, b, Color.red, 200);        
        //Debug.DrawRay(Vector3.zero, c, Color.blue, 200);        

        //Test02
        targetPos.position = transform.position + Vector3.forward * 10;
        Debug.DrawRay(targetPos.position, targetPos.up, Color.blue, 100f);

        Quaternion q = Quaternion.Euler(0, 45, 0);
        Vector3 v = q * Vector3.forward * 10;
        targetPos.position = transform.position + v;
        Debug.DrawRay(targetPos.position, targetPos.up, Color.red, 100f);


    }

    private void Update()
    {
        //Vector3 dir = targetPos.position - transform.position;
        //Debug.DrawLine(transform.position, dir);
        //transform.Translate(dir.normalized * Time.deltaTime);
    }
}
