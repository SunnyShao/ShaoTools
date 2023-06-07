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

    // �����ƶ���Ŀ�귽��
    private Vector3Int targetMoveDir;
    // �����ƶ���Ŀ���
    private Vector3Int targetCellPos;
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
        targetCellPos = Vector3Int.zero;
        targetMoveDir = Vector3Int.zero;
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
        InputManager.Instance.Controller.Player.LeftMove.canceled += OnLeftMoveCanceled;
        InputManager.Instance.Controller.Player.RightMove.started += OnRightMoveStarted;
        InputManager.Instance.Controller.Player.RightMove.canceled += OnRightMoveCanceled;
        InputManager.Instance.Controller.Player.DownMove.started += OnDownMoveStarted;
        InputManager.Instance.Controller.Player.DownMove.canceled += OnDownMoveCanceled;

        InputManager.Instance.Controller.Player.Move.performed += OnMovePerformed;
        InputManager.Instance.Controller.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDownMoveStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.LogError(isMove + ": OnPlayerBottomMove :" + isGround);
        if (isMove || !isGround) return;

        targetMoveDir = new Vector3Int(0, -1, 0);
        targetCellPos = new Vector3Int(cellPos.x, cellPos.y - 1, 0);
        PlayerMove();
    }

    private void OnDownMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.LogError("OnDownMoveCanceled");
        targetMoveDir = Vector3Int.zero;
        targetCellPos = Vector3Int.zero;
    }

    private void OnRightMoveStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isMove) return;

        if (isGround)
        {
            targetMoveDir = new Vector3Int(1, 0, 0);
            targetCellPos = new Vector3Int(cellPos.x + 1, cellPos.y, 0);
            PlayerMove();
        }
    }

    private void OnRightMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.LogError("OnRightMoveCanceled");
        targetMoveDir = Vector3Int.zero;
        targetCellPos = Vector3Int.zero;
    }

    private void OnLeftMoveStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isMove) return;

        if (isGround)
        {
            targetMoveDir = new Vector3Int(-1, 0, 0);
            targetCellPos = new Vector3Int(cellPos.x - 1, cellPos.y, 0);
            PlayerMove();
        }
    }

    private void OnLeftMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.LogError("OnLeftMoveCanceled");
        targetMoveDir = Vector3Int.zero;
        targetCellPos = Vector3Int.zero;
    }

    private void PlayerMove()
    {
        isMove = true;

        TileMapManager.Instance.DeleteCell(targetCellPos);

        targetPos = TileMapManager.Instance.GetWorldPosByCellPos(targetCellPos);
        cellPos = TileMapManager.Instance.GetCellPosByWorldPos(targetPos);
        targetPos += playerOffestPos;
        Debug.Log($"��ǰ���� {transform.position} ��Ŀ����ƶ� {targetPos} Ŀ������ {cellPos}");
    }

    private void OnFlyStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //��������ƶ��У���ô���ܷ���
        if (isMove) return;

        //�ж�ͷ���Ƿ���ڸ��ӣ�������ڸ��Ӳ��ܿ�ʼ��
        Vector3Int targetFlyCellPos = new Vector3Int(cellPos.x, cellPos.y + 1, 0);
        bool hasTopCell = TileMapManager.Instance.IsCellDataByCellPos(targetFlyCellPos);
        Debug.LogError("ͷ���Ƿ��и��� = " + hasTopCell);
        if (hasTopCell) return;

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

    private void Update()
    {
        if (isMove)
        {
            //Debug.LogError("Ŀ��� + " + targetPos);
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
        if (other.CompareTag("3DTiles"))
        {
            Debug.LogError("�������� ������= " + other.gameObject.name);
            isGround = true;
            cellPos = TileMapManager.Instance.GetCellPosByWorldPos(transform.position);
        }
    }

    public void ClearMove()
    {
        isMove = false;
        targetPos = Vector3.zero;

        Debug.LogError($"��ǰ targetCellPos = {targetCellPos} targetMoveDir = {targetMoveDir}");
        if (targetMoveDir != Vector3Int.zero)
        {
            targetCellPos += targetMoveDir;
            PlayerMove();
        }
    }
}
