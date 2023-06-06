using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTileMoveController : MonoBehaviour
{
    [Header("出生点")]
    public Transform bornTrans;

    [Header("主角移速")]
    public float moveSpeed;

    [SerializeField]
    [Header("主角当前格子坐标")]
    private Vector3Int cellPos;

    // 是否移动中
    private bool isMove;
    // 本地移动目标点
    private Vector3 targetPos;
    // 角色模型偏移值
    private Vector3 playerOffestPos = new Vector3(1.5f, 1.5f, 0f);

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        // 设置出生点
        Vector3 bornCellPos = TileMapManager.Instance.GetWorldPosByWorldPos(bornTrans.position);
        transform.position = bornCellPos + playerOffestPos;
        // 获得当前主角对应格子数据
        cellPos = TileMapManager.Instance.GetCellPosByWorldPos(bornCellPos);
    }

    public void OnPlayerLeftMove()
    {
        if (isMove) return;
        isMove = true;

        Vector3Int targetCellPos = new Vector3Int(cellPos.x - 1, cellPos.y, 0);
        TileMapManager.Instance.DeleteCell(targetCellPos);

        targetPos = TileMapManager.Instance.GetWorldPosByCellPos(targetCellPos);
        cellPos = TileMapManager.Instance.GetCellPosByWorldPos(targetPos);
        targetPos += playerOffestPos;
        Debug.Log(transform.position + "向左移动目标点 = " + targetPos);
    }

    public void OnPlayerRightMove()
    {
        if (isMove) return;
        isMove = true;
        Vector3Int targetCellPos = new Vector3Int(cellPos.x + 1, cellPos.y, 0);
        TileMapManager.Instance.DeleteCell(targetCellPos);

        targetPos = TileMapManager.Instance.GetWorldPosByCellPos(targetCellPos);
        cellPos = TileMapManager.Instance.GetCellPosByWorldPos(targetPos);
        targetPos += playerOffestPos;
        Debug.Log(transform.position + "向右移动目标点 = " + targetPos);
    }

    public void OnPlayerBottomMove()
    {
        if (isMove) return;
        isMove = true;
        Vector3Int targetCellPos = new Vector3Int(cellPos.x, cellPos.y - 1, 0);
        TileMapManager.Instance.DeleteCell(targetCellPos);

        targetPos = TileMapManager.Instance.GetWorldPosByCellPos(targetCellPos);
        cellPos = TileMapManager.Instance.GetCellPosByWorldPos(targetPos);
        targetPos += playerOffestPos;
        Debug.Log(transform.position + "向下移动目标点 = " + targetPos);
    }

    public void OnPlayerTopMove()
    {
        if (isMove) return;

        rigidbody.isKinematic = false;
    }

    public void ClearMove()
    {
        isMove = false;
        targetPos = Vector3.zero;
    }

    private void Update()
    {
        if (isMove)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, step);

            if (Vector3.Distance(transform.localPosition, targetPos) < 0.01f)
            {
                ClearMove();
            }
        }
    }
}
