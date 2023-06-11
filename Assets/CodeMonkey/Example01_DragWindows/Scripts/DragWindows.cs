using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindows : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField]
    private RectTransform m_RectTransform;
    [SerializeField]
    private Canvas m_Canvas;

    void Awake()
    {
        if (m_RectTransform == null && transform.parent != null)
        {
            m_RectTransform = transform.parent.GetComponent<RectTransform>();
        }
        else
        {
            m_RectTransform = GetComponent<RectTransform>();
        }

        if (m_Canvas == null)
        {
            Transform trans = transform.parent;
            while (trans != null)
            {
                if (trans.GetComponent<Canvas>() != null)
                {
                    m_Canvas = trans.GetComponent<Canvas>();
                    break;
                }
                trans = trans.parent;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_RectTransform.anchoredPosition += eventData.delta / m_Canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_RectTransform.SetAsLastSibling();
    }
}