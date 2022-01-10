using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundSphere : MonoBehaviour
{
    public GameObject earth;

    [Header("°ë¾¶")]
    public float r;
    [Header("½Ç¶È")]
    public float w = 0.3f;
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        r = Vector3.Distance(transform.position, earth.transform.position);
        //for (int i = 0; i < 361; i++)
        //{
        //    Debug.LogError(i + ":::" + Mathf.Cos(i));
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(earth.transform.position, new Vector3(1,1,1), speed * Time.deltaTime);


        w += speed * Time.deltaTime;

        float x = Mathf.Cos(w) * r;
        float y = Mathf.Sin(w) * r;
        transform.position = new Vector3(x, y, 2);
        //Vector3 temp = Vector3.Lerp(transform.position, new Vector3(x, y, transform.position.z), 1);
        //transform.SetPositionAndRotation(temp, Quaternion.identity);
        //transform.LookAt(earth.transform.position);
    }
}
