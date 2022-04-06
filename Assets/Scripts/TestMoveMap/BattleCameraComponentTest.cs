using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class BattleCameraComponentTest : MonoBehaviour
{
    public CinemachineVirtualCamera m_VCam;

    private const float speed = 2;
    private Vector2 m_TouchPos;
    private float fovOffset;
    private float lastFrameDistance;
    private bool canMove;   //是否可以移动
    private bool canScale;  //是否可以无极缩放

    private void Start()
    {
        InputManager.Instance.Controller.UI.Press.started += PressStart;
        InputManager.Instance.Controller.UI.Press.canceled += PressCancel;
        InputManager.Instance.Controller.UI.Zoom.started += ScrollY;
        InputManager.Instance.Controller.UI.Zoom.canceled += SrollYCancel;
        InputManager.Instance.Controller.UI.SecondPress.started += OnSecondTouchPress;
        InputManager.Instance.Controller.UI.SecondPress.canceled += OnSecondTouchCancel;
    }

    private void LateUpdate()
    {
        if (canScale)
        {
            var touchPos = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();
            var mainTouchPos = InputManager.Instance.Controller.UI.SecondTouch.ReadValue<Vector2>();
            var distance = Vector2.Distance(mainTouchPos, touchPos);
            if (lastFrameDistance != 0)
            {
                var offset = distance - lastFrameDistance;
                fovOffset = offset;
                lastFrameDistance = distance;
            }
            else
            {
                lastFrameDistance = distance;
            }

            SetCameraMovRot();

            //如果到了最大高度 或者 最低高度 如果还是 这个方向的数据就不移动
            Debug.LogError(fovOffset + " = distance = " + distance);

            canMove = false;
        }

        if (canMove)
        {
            var laterPoint = Vector3.zero;
            var afterPoint = Vector3.zero;
            var touchPos = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();
            if (m_TouchPos != Vector2.zero)
            {
                Ray laterRay = Camera.main.ScreenPointToRay(m_TouchPos);
                if (Physics.Raycast(laterRay, out var point, 1000, 1 << LayerMask.NameToLayer("Plane")))
                {
                    laterPoint = point.point;
                }

                Ray afterRay = Camera.main.ScreenPointToRay(touchPos);
                if (Physics.Raycast(afterRay, out var hit, 1000, 1 << LayerMask.NameToLayer("Plane")))
                {
                    afterPoint = hit.point;
                }
                if (laterPoint != Vector3.zero && afterPoint != Vector3.zero)
                {
                    var dir = afterPoint - laterPoint;
                    if (Mathf.Abs(m_VCam.transform.position.x - Camera.main.transform.position.x) > 10 || Mathf.Abs(m_VCam.transform.position.z - Camera.main.transform.position.z) > 10) //相机超出边界自动拉回
                    {
                        //Debug.LogError("超出去了" + Mathf.Abs(m_VCam.transform.position.x - Camera.main.transform.position.x) + ":::" + Mathf.Abs(m_VCam.transform.position.z - Camera.main.transform.position.z));
                        m_VCam.transform.position = Camera.main.transform.position;
                    }
                    var flwPos = m_VCam.transform.position;
                    var targetPos = flwPos - dir;
                    m_VCam.transform.position = targetPos;
                    m_TouchPos = touchPos;
                }
            }
            else
            {
                m_TouchPos = touchPos;
            }
        }
    }

    private void SetCameraMovRot()
    {
        float hight;
        if (fovOffset > 0)
            hight = m_VCam.transform.position.y + -speed;
        else
            hight = m_VCam.transform.position.y + speed;

        float minHight = 18;
        float maxHight = 65;
        hight = Mathf.Clamp(hight, minHight, maxHight);

        float rate = (hight - minHight) / (maxHight - minHight);

        float minAngle = 40;
        float maxAngle = 60;
        float targetAngle = rate * (maxAngle - minAngle) + minAngle;

        m_VCam.transform.position = new Vector3(m_VCam.transform.position.x, hight, m_VCam.transform.position.z);
        Vector3 targetRotate = m_VCam.transform.rotation.eulerAngles;
        targetRotate.x = targetAngle;
        m_VCam.transform.rotation = Quaternion.Euler(targetRotate);
    }

    private void ScrollY(InputAction.CallbackContext context)
    {
        var offet = context.ReadValue<Vector2>();
        fovOffset = offet.y;
        Debug.LogError("fovOffset = " + fovOffset);
        SetCameraMovRot();
    }

    private void SrollYCancel(InputAction.CallbackContext contex)
    {
        fovOffset = 0;
    }

    Vector2 startTouch;
    Vector2 endTouch;

    private void PressStart(InputAction.CallbackContext context)
    {
        startTouch = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();
        canMove = true;
    }

    private void PressCancel(InputAction.CallbackContext context)
    {
        endTouch = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();
        if (startTouch != Vector2.zero && endTouch != Vector2.zero && Vector2.Distance(startTouch, endTouch) > 0.5f)
        {
            //Debug.LogError("两次不相等 ， 属于滑动了手指");
        }
        else
        {
            //Debug.LogError("两次是是是是相等 ， 进行点击");
        }
        canMove = false;
        m_TouchPos = Vector2.zero;
    }

    private void OnSecondTouchPress(InputAction.CallbackContext context)
    {
        canScale = true;
    }

    private void OnSecondTouchCancel(InputAction.CallbackContext context)
    {
        canScale = false;
        lastFrameDistance = 0;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.Controller.UI.Press.performed -= PressStart;
            InputManager.Instance.Controller.UI.Press.canceled -= PressCancel;
            InputManager.Instance.Controller.UI.Zoom.started -= ScrollY;
            InputManager.Instance.Controller.UI.Zoom.canceled -= SrollYCancel;
            InputManager.Instance.Controller.UI.SecondPress.started -= OnSecondTouchPress;
            InputManager.Instance.Controller.UI.SecondPress.canceled -= OnSecondTouchCancel;
        }
    }
}