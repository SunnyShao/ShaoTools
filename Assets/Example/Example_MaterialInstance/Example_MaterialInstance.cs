using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example_MaterialInstance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var render = GetComponent<Renderer>();
        // 运行会发现两个cube 均会变成白色
        // sharedMaterial 是共用的 Material，称为共享材质。修改共享材质会改变所用使用该材质的物体，并且编辑器中的材质设置也会改变
        render.sharedMaterial.color = Color.white;

        // 运行会发现只有挂载了这个脚本的Cuble 会变成白色，Material后面多了(Instance)
        // material 是独立的 Material，返回分配给渲染器的第一个材质。修改材质仅会改变该物体的材质。如果该材质被其他的渲染器使用，将克隆该材质并用于当前的渲染器
        //render.material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
