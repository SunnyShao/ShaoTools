using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISphereLayout : MonoBehaviour
{
    public GameObject spawnObj;
    public int _count;
    public float radius;
    public float a;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 zeroPos = spawnObj.GetComponent<RectTransform>().anchoredPosition;
        for (int i = 0; i < 360; i++)
        {
            var obj = Instantiate(spawnObj, transform);
            //obj.GetComponent<RectTransform>().anchoredPosition = zeroPos - new Vector2(0, radius);

            float x = zeroPos.x + radius * Mathf.Cos(i * Mathf.PI / 180);
            float y = zeroPos.y + radius * Mathf.Sin(i * Mathf.PI / 180);
            Debug.Log(x + ":::" + y);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
