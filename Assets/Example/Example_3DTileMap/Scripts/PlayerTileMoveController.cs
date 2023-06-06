using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerTileMoveController : MonoBehaviour
{
    [Header("������")]
    public Transform bornTrans;

    [Header("��������")]
    public float moveSpeed;
    [Header("���Ƿ�������")]
    public float flySpeed;

    [SerializeField]
    [Header("���ǵ�ǰ��������")]
    private Vector3Int cellPos;

    // �Ƿ��ƶ���
    private bool isMove;
    // �Ƿ��ڵ���
    private bool isGround = true;
    // �Ƿ������
    private bool isFly = true;
    // �����ƶ�Ŀ���
    private Vector3 targetPos;
    // ��ɫģ��ƫ��ֵ
    private Vector3 playerOffestPos = new Vector3(1.5f, 1.5f, 0f);

    private Rigidbody rigidbody;

    // �����ƶ�����
    private float input_X;
    private float input_Y;
    private Vector3 input_MoveDir;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        isMove = false;
        isGround = true;

        // ���ó�����
        Vector3 bornCellPos = TileMapManager.Instance.GetWorldPosByWorldPos(bornTrans.position);
        transform.position = bornCellPos + playerOffestPos;
        // ��õ�ǰ���Ƕ�Ӧ��������
        cellPos = TileMapManager.Instance.GetCellPosByWorldPos(bornCellPos);

        InputManager.Instance.Controller.Player.FlyBtn.started += OnFlyStarted;
        InputManager.Instance.Controller.Player.FlyBtn.canceled += OnFlyCanceled;
        InputManager.Instance.Controller.Player.LeftMove.started += OnLeftMoveStarted;
        InputManager.Instance.Controller.Player.RightMove.started += OnRightMoveStarted;

        InputManager.Instance.Controller.Player.Move.performed += OnMovePerformed;
        InputManager.Instance.Controller.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnRightMoveStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isMove) return;
        isMove = true;

        if (isGround)
        {
            Vector3Int targetCellPos = new Vector3Int(cellPos.x + 1, cellPos.y, 0);
            TileMapManager.Instance.DeleteCell(targetCellPos);
            targetPos = TileMapManager.Instance.GetWorldPosByCellPos(targetCellPos);
            cellPos = TileMapManager.Instance.GetCellPosByWorldPos(targetPos);
            targetPos += playerOffestPos;
            Debug.Log(cellPos + "::::" + transform.position + "�����ƶ�Ŀ��� = " + targetPos);
        }
        //else
        //{
        //    input_MoveDir = new Vector3(1, input_MoveDir.y, 0);
        //}
    }

    private void OnLeftMoveStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isMove) return;
        isMove = true;

        if (isGround)
        {
            Vector3Int targetCellPos = new Vector3Int(cellPos.x - 1, cellPos.y, 0);
            TileMapManager.Instance.DeleteCell(targetCellPos);

            targetPos = TileMapManager.Instance.GetWorldPosByCellPos(targetCellPos);
            cellPos = TileMapManager.Instance.GetCellPosByWorldPos(targetPos);
            targetPos += playerOffestPos;
            Debug.Log(transform.position + "�����ƶ�Ŀ��� = " + targetPos);
        }
        //else
        //{
        //    input_MoveDir = new Vector3(-1, input_MoveDir.y, 0);
        //}
    }

    private void OnFlyStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //Debug.LogError("OnFlyStarted");
        if (isMove) return;
        //rigidbody.isKinematic = false;
        isGround = false;

    }

    private void OnFlyCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //Debug.LogError("OnFlyCanceled");
        //rigidbody.isKinematic = true;
        //isGround = true;
        //rigidbody.AddForce(new Vector3(0, -1, 0) * 10000 * Time.deltaTime, ForceMode.Force);
    }

    private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Vector2 moveDir = obj.ReadValue<Vector2>();
        input_X = moveDir.x;
        input_Y = moveDir.y;
        if (input_X != 0 && input_Y != 0)
        {
            input_X = input_X * 0.7f;
            input_Y = input_Y * 0.7f;
        }
        input_MoveDir = new Vector2(input_X, input_Y);
        Debug.LogError(input_MoveDir);
    }

    private void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        input_X = 0;
        input_Y = 0;
        input_MoveDir = Vector3.zero;
    }

    public void OnPlayerLeftMove()
    {
        //if (isMove) return;
        //isMove = true;

        //if (isGround)
        //{
        //    Vector3Int targetCellPos = new Vector3Int(cellPos.x - 1, cellPos.y, 0);
        //    TileMapManager.Instance.DeleteCell(targetCellPos);

        //    targetPos = TileMapManager.Instance.GetWorldPosByCellPos(targetCellPos);
        //    cellPos = TileMapManager.Instance.GetCellPosByWorldPos(targetPos);
        //    targetPos += playerOffestPos;
        //    Debug.Log(transform.position + "�����ƶ�Ŀ��� = " + targetPos);
        //}
        //else
        //{
        //    input_MoveDir = new Vector3(-1, input_MoveDir.y, 0);
        //}
    }

    public void OnPlayerRightMove()
    {
        //if (isMove) return;
        //isMove = true;

        //if (isGround)
        //{
        //    Vector3Int targetCellPos = new Vector3Int(cellPos.x + 1, cellPos.y, 0);
        //    TileMapManager.Instance.DeleteCell(targetCellPos);
        //    targetPos = TileMapManager.Instance.GetWorldPosByCellPos(targetCellPos);
        //    cellPos = TileMapManager.Instance.GetCellPosByWorldPos(targetPos);
        //    targetPos += playerOffestPos;
        //    Debug.Log(transform.position + "�����ƶ�Ŀ��� = " + targetPos);
        //}
        //else
        //{
        //    input_MoveDir = new Vector3(1, input_MoveDir.y, 0);
        //}
    }

    public void OnPlayerBottomMove()
    {
        Debug.LogError(isMove + ":::" + isGround);
        if (isMove || !isGround) return;

        isMove = true;
        Vector3Int targetCellPos = new Vector3Int(cellPos.x, cellPos.y - 1, 0);
        TileMapManager.Instance.DeleteCell(targetCellPos);

        targetPos = TileMapManager.Instance.GetWorldPosByCellPos(targetCellPos);
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
            Debug.LogError("Ŀ��� + " + targetPos);
            float step = moveSpeed * Time.deltaTime;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, step);

            if (Vector3.Distance(transform.localPosition, targetPos) < 0.01f)
            {
                ClearMove();
            }
        }

        if (!isGround)
        {
            rigidbody.AddForce(input_MoveDir * flySpeed * Time.deltaTime, ForceMode.Force);
            cellPos = TileMapManager.Instance.GetCellPosByWorldPos(transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("3DTiles"))
        {
            Debug.LogError("�������� ������= " + other.gameObject.name);
            isGround = true;
            cellPos = TileMapManager.Instance.GetCellPosByWorldPos(transform.position);
        }
    }
}
