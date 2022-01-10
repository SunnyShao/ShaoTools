using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Project : MonoBehaviour
{

    Vector3 A;

    // Start is called before the first frame update
    void Start()
    {
        //Vector3.Project        : ��һ������ͶӰ����һ��������
        //Vector3.ProjectOnPlane : ������ͶӰ���ɴ�ֱ�ڸ�ƽ��ķ��߶����ƽ����

        A = new Vector3(2, 2, 2);
        Debug.LogError(Vector3.Project(A, Vector3.right));          //������ X��(1,0,0) �����ϵ�ͶӰ      (2,0,0)
        Debug.LogError(Vector3.ProjectOnPlane(A, Vector3.right));   //������ X��ƽ���ϵ�ͶӰ        (0,2,2)

        Debug.LogError(Vector3.Project(A, Vector3.up));             //������ Y��(0,1,0) �����ϵ�ͶӰ      (0,2,0)
        Debug.LogError(Vector3.ProjectOnPlane(A, Vector3.up));      //������ Y��ƽ���ϵ�ͶӰ        (2,0,2)

        Debug.LogError(Vector3.Project(A, Vector3.forward));        //������ Z��(0,0,1) �����ϵ�ͶӰ      (0,0,2)
        Debug.LogError(Vector3.ProjectOnPlane(A, Vector3.forward)); //������ Z��ƽ���ϵ�ͶӰ        (2,2,0)

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
