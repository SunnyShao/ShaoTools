using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quaternion_AngleAxis : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError(Quaternion.AngleAxis(-15, Vector3.left));
        Debug.LogError(Quaternion.AngleAxis(-15, Vector3.left) * Vector3.forward);
        transform.rotation = Quaternion.LookRotation( Quaternion.AngleAxis(-15, Vector3.left) * Vector3.forward , Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
