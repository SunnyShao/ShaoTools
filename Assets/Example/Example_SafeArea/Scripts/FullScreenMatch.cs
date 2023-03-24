using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [ExecuteAlways]
    public class FullScreenMatch : MonoBehaviour
    {
        [Range(1f, 2f)]
        public float scale = 1f;
        private RectTransform CanvasRect, SelfRect;

        [ExecuteAlways]
        private void OnEnable()
        {
            DoMatch();
        }

        [ExecuteInEditMode]
        private void Update()
        {
            DoMatch();
        }

        private void DoMatch()
        {
            if (CanvasRect == null || SelfRect == null)
            {
                var canvas = GetComponentInParent<Canvas>();
                if (canvas != null)
                {
                    CanvasRect = canvas.transform as RectTransform;
                }

                SelfRect = transform as RectTransform;
            }

            if (CanvasRect != null && SelfRect != null)
            {
                var width = CanvasRect.sizeDelta.x > CanvasRect.sizeDelta.y ? CanvasRect.sizeDelta.x : CanvasRect.sizeDelta.y;
                width = Mathf.CeilToInt(width * scale);
                SelfRect.sizeDelta = new Vector2(width, width);
            }
        }
    }
}
