using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHead_WorldCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, 2, 0);
        var pos = Vector3.ProjectOnPlane(transform.position, Camera.main.transform.forward);
        transform.LookAt(pos, Vector3.up);
        //transform.LookAt(transform.position, Camera.main.transform.forward);
        //transform.localRotation *= Quaternion.Euler(new Vector3(0, 180, 0));
    }
}
