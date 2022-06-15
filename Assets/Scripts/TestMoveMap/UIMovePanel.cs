using System.Collections.Generic;
using UnityEngine;

public class UIMovePanel : MonoBehaviour
{
    public static UIMovePanel instance;

    public GameObject[] followObj;
    private GameObject moveUI;
    public Camera uicamera;
    public Vector3 offest;

    private List<GameObject> moveUIList = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        moveUI = transform.Find("NameUI").gameObject;
        InitWorldUI();
    }

    void LateUpdate()
    {
        for (int i = 0; i < followObj.Length; i++)
        {
            moveUIList[i].GetComponent<RectTransform>().anchoredPosition = GetUIPos(followObj[i].transform.position + offest); //时刻转换
        }
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

    //3D世界坐标转换为UI坐标
    private Vector2 GetUIPos(Vector3 worldPos)
    {
        Vector2 uiPos;
        var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
        uiPos = screenPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(moveUI.transform.parent.GetComponent<RectTransform>(), screenPoint, uicamera, out uiPos);
        return uiPos;
    }

    public void OnMapClick(Vector2 touchPos)
    {
        var ray = Camera.main.ScreenPointToRay(touchPos);
        if (Physics.Raycast(ray , out var hit, 1000, 1 << LayerMask.NameToLayer("WorldObj")))
        {
            Debug.LogError(hit.collider.gameObject.name + "::" + hit.collider.gameObject.transform.position);
        }


    }

    private void OnDestroy()
    {
        moveUIList.Clear();
    }
}