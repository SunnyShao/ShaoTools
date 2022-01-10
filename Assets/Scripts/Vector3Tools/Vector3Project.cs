using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Project : MonoBehaviour
{

    Vector3 A;

    // Start is called before the first frame update
    void Start()
    {
        //Vector3.Project        : 将一个向量投影到另一个向量上
        //Vector3.ProjectOnPlane : 将向量投影到由垂直于该平面的法线定义的平面上

        A = new Vector3(2, 2, 2);
        Debug.LogError(Vector3.Project(A, Vector3.right));          //向量在 X轴(1,0,0) 方向上的投影      (2,0,0)
        Debug.LogError(Vector3.ProjectOnPlane(A, Vector3.right));   //向量在 X轴平面上的投影        (0,2,2)

        Debug.LogError(Vector3.Project(A, Vector3.up));             //向量在 Y轴(0,1,0) 方向上的投影      (0,2,0)
        Debug.LogError(Vector3.ProjectOnPlane(A, Vector3.up));      //向量在 Y轴平面上的投影        (2,0,2)

        Debug.LogError(Vector3.Project(A, Vector3.forward));        //向量在 Z轴(0,0,1) 方向上的投影      (0,0,2)
        Debug.LogError(Vector3.ProjectOnPlane(A, Vector3.forward)); //向量在 Z轴平面上的投影        (2,2,0)

        Debug.DrawRay(transform.position, A, Color.red, 600);
        Debug.DrawRay(transform.position, -A, Color.green, 600);
        Debug.DrawRay(transform.position, Vector3.Project(A, Vector3.right), Color.yellow, 600);
        Debug.DrawRay(transform.position, Vector3.ProjectOnPlane(A, Vector3.right), Color.black, 600);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(Vector3.zero , new Vector3(0, 2, 2));
        //Gizmos.DrawLine(Vector3.zero , new Vector3(1, 0, 0));
    }
}
