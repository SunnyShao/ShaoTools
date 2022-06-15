using System.Collections.Generic;
using UnityEngine;

public class UIMovePanel : MonoBehaviour
{
    public GameObject[] followObj;
    private GameObject moveUI;
    public Camera uicamera;
    public Vector3 offest;

    private List<GameObject> moveUIList = new List<GameObject>();

    void Start()
    {
        moveUI = transform.Find("MoveUI").gameObject;
        InitWorldUI();
    }

    private void InitWorldUI()
    {
        for (int i = 0; i < followObj.Length; i++)
        {
            var obj = Instantiate(moveUI, moveUI.transform.parent);
            moveUIList.Add(obj);
        }
        moveUI.SetActive(false);
    }

    private void OnDestroy()
    {
        moveUIList.Clear();
    }

    void LateUpdate()
    {
        for (int i = 0; i < followObj.Length; i++)
        {
            moveUIList[i].GetComponent<RectTransform>().anchoredPosition = GetUIPos(followObj[i].transform.position + offest); //时刻转换
        }
    }

    //3D世界坐标转换为UI坐标
    private Vector2 GetUIPos(Vector3 worldPos)
    {
        Vector2 uiPos;
        var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
        uiPos = screenPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(moveUI.transform.parent.GetComponent<RectTransform>(), screenPoint, uicamera, out uiPos);
        return uiPos;
    }
}