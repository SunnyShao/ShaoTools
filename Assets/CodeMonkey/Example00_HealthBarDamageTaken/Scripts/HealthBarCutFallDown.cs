using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey
{
    /// <summary>
    /// Ѫ������ű�
    /// </summary>
    public class HealthBarCutFallDown : MonoBehaviour
    {
        // ���ɺ��ӳٵ�����ʱ��
        private float downTime = 0.5f;

        // ����������֮��ʼ���ص�ʱ��
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
            // �ȿ�ʼִ���ӳٶ���������
            downTime -= Time.deltaTime;
            if (downTime < 0)
            {
                // ��ʼ����֮�� ÿ������ٶ��½�
                curRectTrans.anchoredPosition += Vector2.down * 50f * Time.deltaTime;

                // ��ʼ�½�֮�� ��ʼִ�ж����������
                hideTime -= Time.deltaTime;
                if (hideTime < 0)
                {
                    // Alphaֵ�ݼ���Ϊ0��ɾ������
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