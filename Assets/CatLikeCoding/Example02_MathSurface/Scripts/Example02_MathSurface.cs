using UnityEngine;

public class Example02_MathSurface : MonoBehaviour
{
    [Header("构建视图的预制体")]
    public Transform pointPrefab;

    [Header("分辨率滑块")]
    [Range(10, 100)]
    public int resolution = 50;

    [Range(0, 1)]
    public int functionIndex = 0;

    private Transform[] points;
    private static GraphFunction[] functions = { SineFunction, MultiSineFunction };

    void Start()
    {
        points = new Transform[resolution];
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 pos;
        pos.z = 0f;
        pos.y = 0f;

        for (int i = 0; i < points.Length; i++)
        {
            Transform trans = Instantiate(pointPrefab);
            pos.x = (i + 0.5f) * step - 1f;
            trans.localPosition = pos;
            trans.localScale = scale;
            trans.SetParent(transform, false);
            points[i] = trans;
        }
    }

    private void Update()
    {
        float t = Time.time;
        GraphFunction func = functions[functionIndex];
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i];
            Vector3 pos = point.localPosition;
            func(pos.x, t);
            point.localPosition = pos;
        }
    }

    private static float SineFunction(float x, float t)
    {
        return Mathf.Sin((x + t) * Mathf.PI);
    }

    private static float MultiSineFunction(float x, float t)
    {
        float y = Mathf.Sin((x + t) * Mathf.PI);
        y += Mathf.Sin((x + t) * Mathf.PI * 2) / 2f;
        return y;
    }
}