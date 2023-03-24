using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SafeAreaMatch : MonoBehaviour
    {
        public TextMeshProUGUI tips;
        public Button refreshBtn;

        Vector2 originAnchoredPosition;
        bool m_started;

        private void Awake()
        {
            var rectTran = transform.GetComponent<RectTransform>();
            originAnchoredPosition = rectTran.anchoredPosition;

            refreshBtn.onClick.AddListener(OnRefreshBtnClick);
        }

        private void OnRefreshBtnClick()
        {
            Start();
        }

        private void Update()
        {
            if (m_started)
                return;

            Start();
        }

        private void OnDisable()
        {
            m_started = false;
        }

        private void Start()
        {
            m_started = true;

            var rectTran = transform.GetComponent<RectTransform>();
            var safeArea = Screen.safeArea;
            Debug.LogError($"orientation = {Screen.orientation},safeArea = {safeArea},Screen.width = {Screen.width}");
            tips.text = Screen.safeArea.ToString() + " \n" + Screen.width + " \n" + Screen.height + " \n" + Screen.orientation;
#if UNITY_ANDROID
            if (Screen.orientation == ScreenOrientation.Landscape)
            {
                int offset = Screen.width - (int)safeArea.width;

                if (rectTran.anchorMax.x == rectTran.anchorMin.x)
                {
                    if (rectTran.anchorMax.x == 0)
                        rectTran.anchoredPosition = originAnchoredPosition + new Vector2(offset, 0);
                    else if (rectTran.anchorMax.x == 1 && Screen.autorotateToLandscapeRight) // 允许刘海朝右
                        rectTran.anchoredPosition = originAnchoredPosition - new Vector2(offset, 0);
                }
                else // Stretch
                {
                    if (Screen.autorotateToLandscapeRight) // 允许刘海朝右
                    {
                        rectTran.sizeDelta = new Vector2(-offset << 1, 0);
                    }
                    else
                    {
                        rectTran.sizeDelta = new Vector2(-offset, 0);
                        rectTran.anchoredPosition = new Vector2(originAnchoredPosition.x + (offset >> 1), originAnchoredPosition.y);
                    }
                }
            }
            else if (Screen.orientation == ScreenOrientation.Portrait)
            {
                int offset = Screen.height - (int)safeArea.height;

                if (rectTran.anchorMax.y == rectTran.anchorMin.y)
                {
                    if (rectTran.anchorMax.y == 1)
                    {
                        rectTran.anchoredPosition = originAnchoredPosition - new Vector2(0, offset);
                    }
                    else if (rectTran.anchorMax.y == 0 && Screen.autorotateToPortraitUpsideDown) // 允许刘海朝下
                    {
                        rectTran.anchoredPosition = originAnchoredPosition + new Vector2(0, offset);
                    }
                }
                else // Stretch
                {
                    if (Screen.autorotateToPortraitUpsideDown) // 允许刘海朝下
                    {
                        rectTran.sizeDelta = new Vector2(0, -offset << 1);
                        rectTran.anchoredPosition = new Vector2(0, -offset);
                    }
                    else
                    {
                        rectTran.sizeDelta = new Vector2(0, -offset);
                        rectTran.anchoredPosition = new Vector2(0, -offset);
                    }
                }
            }
#endif
        }
    }
}
