using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleCameraComponent : MonoBehaviour
{
    public float speed;
    public CinemachineVirtualCamera m_VCam;
    public GameObject follower;
    private Camera m_Cam;

    private Vector2 m_TouchPos;

    private float fovOffset;
    private float lastFrameDistance;

    private bool canMove;
    private bool canScale;
    private bool canRotate;

    public float m_Distance;
    public float m_Radius;

    public Transform minConfine;
    public Transform maxConfine;

    public float minDistance;
    public float maxDistance;

    private CinemachineTransposer transposer;

    private void Awake()
    {
        m_Cam = Camera.main;
    }

    // Start is called before the first frame update
    private void Start()
    {
        InputManager.Instance.Controller.UI.Press.started += PressStart;
        InputManager.Instance.Controller.UI.Press.canceled += PressCancel;
        InputManager.Instance.Controller.UI.Zoom.started += ScrollY;
        InputManager.Instance.Controller.UI.Zoom.canceled += SrollYCancel;
        InputManager.Instance.Controller.UI.SecondPress.started += OnSecondTouchPress;
        InputManager.Instance.Controller.UI.SecondPress.canceled += OnSecondTouchCancel;

        transposer = m_VCam.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void LateUpdate()
    {
        if (canScale)
        {
            var touchPos = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();

            var mainTouchPos = InputManager.Instance.Controller.UI.SecondTouch.ReadValue<Vector2>();

            var distance = Vector2.Distance(mainTouchPos, touchPos);
            //两个触点之间的距离
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
            canMove = false;
        }
        if (canMove)
        {
            var laterPoint = Vector3.zero;
            var afterPoint = Vector3.zero;
            var touchPos = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();
            if (m_TouchPos != Vector2.zero)
            {
                Ray laterRay = m_Cam.ScreenPointToRay(m_TouchPos);
                if (Physics.Raycast(laterRay, out var point, 1000, 1 << 9))
                {
                    laterPoint = point.point;
                }

                Ray afterRay = m_Cam.ScreenPointToRay(touchPos);
                if (Physics.Raycast(afterRay, out var hit, 1000, 1 << 9))
                {
                    afterPoint = hit.point;
                }
                if (laterPoint != Vector3.zero && afterPoint != Vector3.zero)
                {
                    var dir = afterPoint - laterPoint;
                    var flwPos = follower.transform.position;

                    var targetPos = flwPos - dir;

                    //var x = Mathf.Clamp(targetPos.x, minConfine.position.x, maxConfine.position.x);
                    //var z = Mathf.Clamp(targetPos.z, minConfine.position.z, maxConfine.position.z);
                    //follower.transform.position = new Vector3(x, targetPos.y, z);

                    follower.transform.position = targetPos;
                    m_TouchPos = touchPos;
                }
            }
            else
            {
                m_TouchPos = touchPos;
            }
        }

        var touchScalePos = Vector3.zero;
        var followerPos = transposer.m_FollowOffset; //当前 FollowOffset
        //计算目标位置的FollowOffset
        var py = followerPos - (transposer.m_FollowOffset * Time.deltaTime) * fovOffset / 100 * speed;
        //计算当前半径
        var vCamPointInPlane = Vector3.ProjectOnPlane(m_VCam.transform.position, Vector3.up);
        var oriPointInPlane = Vector3.ProjectOnPlane(follower.transform.position, Vector3.up);
        m_Radius = (vCamPointInPlane - oriPointInPlane).magnitude;
        //计算当前偏移距离
        m_Distance = Vector3.Distance(follower.transform.position, m_VCam.transform.position);

        //此处是缩放限制
        if (((int)m_Distance) < ((int)maxDistance) && ((int)m_Distance) > ((int)minDistance))
        {
            touchScalePos = new Vector3(py.x, py.y, py.z);
        }
        else if (((int)m_Distance) >= ((int)maxDistance))
        {
            if (fovOffset > 0)
            {
                touchScalePos = new Vector3(py.x, py.y, py.z);
            }
            else if (fovOffset < 0)
            {
                touchScalePos = followerPos.normalized * maxDistance;
            }
        }
        else if (((int)m_Distance) <= ((int)minDistance))
        {
            if (fovOffset < 0)
            {
                touchScalePos = new Vector3(py.x, py.y, py.z);
            }
            else if (fovOffset > 0)
            {
                touchScalePos = followerPos.normalized * minDistance;
            }
        }

        if (fovOffset != 0 && touchScalePos != Vector3.zero && !canRotate)
        {
            transposer.m_FollowOffset = touchScalePos;
        }
        fovOffset = 0;
    }

    private void ScrollY(InputAction.CallbackContext context)
    {
        var offet = context.ReadValue<Vector2>();
        fovOffset = offet.y;
    }

    private void SrollYCancel(InputAction.CallbackContext contex)
    {
        fovOffset = 0;
    }

    private void PressStart(InputAction.CallbackContext context)
    {
        canMove = true;
    }

    private void PressCancel(InputAction.CallbackContext context)
    {
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