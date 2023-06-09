using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey
{
    /// <summary>
    /// 血条降落脚本
    /// </summary>
    public class HealthBarCutFallDown : MonoBehaviour
    {
        // 生成后延迟的下落时间
        private float downTime = 0.5f;

        // 下落后多少秒之后开始隐藏的时间
        private float hideTime = 1f;

        public RectTransform curRectTrans;
        public Image curImage;

        private void Awake()
        {
            curRectTrans = GetComponent<RectTransform>();
            curImage = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            // 先开始执行延迟多少秒下落
            downTime -= Time.deltaTime;
            if (downTime < 0)
            {
                // 开始下落之后 每秒多少速度下降
                curRectTrans.anchoredPosition += Vector2.down * 50f * Time.deltaTime;

                // 开始下降之后 开始执行多少秒后隐藏
                hideTime -= Time.deltaTime;
                if (hideTime < 0)
                {
                    // Alpha值递减，为0后删除物体
                    Color imageColor = curImage.color;
                    imageColor.a -= 5f * Time.deltaTime;
                    curImage.color = imageColor;

                    if (curImage.color.a <= 0f)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}