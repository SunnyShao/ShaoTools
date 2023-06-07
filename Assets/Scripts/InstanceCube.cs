using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceCube : MonoBehaviour
{
    static int BlednColorId = Shader.PropertyToID("_BlendColor");

    [SerializeField]
    Color baseColor = Color.white;

    static MaterialPropertyBlock block;

    private void Awake()
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
        }
        block.SetColor(BlednColorId, baseColor);
        GetComponent<Renderer>().SetPropertyBlock(block);
    }

    //void OnValidate()
    //{
    //    if (block == null)
    //    {
    //        block = new MaterialPropertyBlock();
    //    }
    //    block.SetColor(BlednColorId, baseColor);
    //    GetComponent<Renderer>().SetPropertyBlock(block);
    //    //Graphics.DrawMeshInstanced(mesh, 0, material, matrices, 1023, block);
    //}

    //_BlendColor
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
