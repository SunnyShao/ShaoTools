using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("主角移速")]
    public float moveSpeed;

    private Rigidbody playerRB;

    // 缓存移动方向
    private float input_X;
    private float input_Y;
    private Vector3 input_MoveDir;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    void Start()
    {
        InputManager.Instance.Controller.Player.Move.performed += OnMovePerformed;
        InputManager.Instance.Controller.Player.Move.canceled += OnMoveCanceled;
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
    }

    private void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        input_X = 0;
        input_Y = 0;
        input_MoveDir = Vector2.zero;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        playerRB.MovePosition(playerRB.position + input_MoveDir * moveSpeed * Time.deltaTime);
    }

    private void OnDisable()
    {
        InputManager.Instance.Controller.Player.Move.performed -= OnMovePerformed;
        InputManager.Instance.Controller.Player.Move.canceled -= OnMoveCanceled;
    }
}
