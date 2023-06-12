using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example02_DynamicMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError("Start");

        Mesh mesh = new Mesh(); 
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
