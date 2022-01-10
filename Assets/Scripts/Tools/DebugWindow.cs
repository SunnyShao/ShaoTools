using TMPro;
using UnityEngine;

namespace Game
{
    public class DebugWindow : MonoBehaviour
    {
        private TextMeshProUGUI label;

        private int frameCout;
        private float lastTime;

        // Start is called before the first frame update
        void Start()
        {
            label = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            label.text = "";
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time - lastTime > 1)
            {
                label.text = string.Format("FPS:{0}", frameCout);
                lastTime = Time.time;
                frameCout = 0;
            }
            else
            {
                frameCout++;
            }
        }
    }
}
