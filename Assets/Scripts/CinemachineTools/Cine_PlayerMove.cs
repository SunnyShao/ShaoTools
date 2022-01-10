using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cine_PlayerMove : MonoBehaviour
{
    public float speed = 10f;

    public int jumpTime = 30;
    public int fallTime = 20;
    public float jumpHeight = 1f;

    private GameControls inputActions;
    private Vector2 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        inputActions = new GameControls();
        inputActions.Enable();
        inputActions.Player.Move.performed += OnPlayerMove;
        inputActions.Player.Move.canceled += OnPlayerMove;
        inputActions.Player.Jump.performed += OnPlayerJump;
    }

    private void OnPlayerJump(InputAction.CallbackContext obj)
    {
        JumpForSeconds();
    }

    private void OnDestroy()
    {
        inputActions.Player.Move.performed -= OnPlayerMove;
        inputActions.Player.Move.canceled -= OnPlayerMove;
        inputActions.Player.Jump.performed -= OnPlayerJump;
        inputActions.Disable();
    }

    private void OnPlayerMove(InputAction.CallbackContext obj)
    {
        moveDir = obj.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        //float h = Input.GetAxisRaw("Horizontal");
        //float v = Input.GetAxisRaw("Vertical");
        ////transform.position = transform.position + new Vector3(h * speed *Time.deltaTime, 0, v * speed * Time.deltaTime);
        transform.Translate(new Vector3(moveDir.x * speed * Time.deltaTime, 0, moveDir.y * speed * Time.deltaTime));

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        JumpForSeconds();
    //    }
    }

    void JumpForSeconds()
    {
        StartCoroutine(jumpForSeconds());
    }

    IEnumerator jumpForSeconds()
    {
        int time = jumpTime;
        float speed = jumpHeight / jumpTime;
        while (time > 0)
        {
            time--;
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.up * speed, 1f);
            //transform.Translate(transform.up * speed);
            yield return new WaitForEndOfFrame();
        }
        time = fallTime;
        speed = -jumpHeight / fallTime;
        while (time > 0)
        {
            time--;
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.up * speed, 1f);
            //transform.Translate(transform.up * speed);
            yield return new WaitForEndOfFrame();
        }
    }
}
