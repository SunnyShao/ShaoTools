using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example_MaterialInstance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var render = GetComponent<Renderer>();
        // ���лᷢ������cube �����ɰ�ɫ
        // sharedMaterial �ǹ��õ� Material����Ϊ������ʡ��޸Ĺ�����ʻ�ı�����ʹ�øò��ʵ����壬���ұ༭���еĲ�������Ҳ��ı�
        render.sharedMaterial.color = Color.white;

        // ���лᷢ��ֻ�й���������ű���Cuble ���ɰ�ɫ��Material�������(Instance)
        // material �Ƕ����� Material�����ط������Ⱦ���ĵ�һ�����ʡ��޸Ĳ��ʽ���ı������Ĳ��ʡ�����ò��ʱ���������Ⱦ��ʹ�ã�����¡�ò��ʲ����ڵ�ǰ����Ⱦ��
        //render.material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
