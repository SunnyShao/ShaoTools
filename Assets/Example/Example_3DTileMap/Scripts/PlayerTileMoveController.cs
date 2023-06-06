using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTileMoveController : MonoBehaviour
{
    [Header("������")]
    public Transform bornTrans;

    [Header("��������")]
    public float moveSpeed;

    [SerializeField]
    [Header("���ǵ�ǰ��������")]
    private Vector3Int cellPos;

    // �Ƿ��ƶ���
    private bool isMove;
    // �����ƶ�Ŀ���
    private Vector3 targetPos;

    private Vector3 playerOffestPos = new Vector3(1.5f, 3f, 0f);

    private void Awake()
    {
        // ���ó�����
        Vector3 bornCellPos = TileMapManager.Instance.GetWorldPosByWorldPos(bornTrans.position);
        transform.position = bornCellPos + playerOffestPos;
        // ��õ�ǰ���Ƕ�Ӧ��������
        cellPos = TileMapManager.Instance.GetCellPosByWorldPos(bornCellPos);
    }

    public void OnPlayerLeftMove()
    {
        if (isMove) return;
        isMove = true;
        targetPos = TileMapManager.Instance.GetWorldPosByCellPos(new Vector3Int(cellPos.x - 1, cellPos.y, 0));
        cellPos = TileMapManager.Instance.GetCellPosByWorldPos(targetPos);
        targetPos += playerOffestPos;
        Debug.Log(transform.position + "�����ƶ�Ŀ��� = " + targetPos);
    }

    public void OnPlayerRightMove()
    {
        if (isMove) return;
        isMove = true;
        targetPos = TileMapManager.Instance.GetWorldPosByCellPos(new Vector3Int(cellPos.x + 1, cellPos.y, 0));
        cellPos = TileMapManager.Instance.GetCellPosByWorldPos(targetPos);
        targetPos += playerOffestPos;
        Debug.Log(transform.position + "�����ƶ�Ŀ��� = " + targetPos);
    }

    public void OnPlayerBottomMove()
    {
        if (isMove) return;
        isMove = true;
        targetPos = TileMapManager.Instance.GetWorldPosByCellPos(new Vector3Int(cellPos.x, cellPos.y - 1, 0));
        cellPos = TileMapManager.Instance.GetCellPosByWorldPos(targetPos);
        targetPos += playerOffestPos;
        Debug.Log(transform.position + "�����ƶ�Ŀ��� = " + targetPos);
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

            if(Vector3.Distance(transform.localPosition, targetPos) < 0.01f)
            {
                ClearMove();
            }
        }
    }
}
