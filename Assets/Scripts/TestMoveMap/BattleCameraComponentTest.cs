using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleCameraComponentTest : MonoBehaviour
{
    private const string CAMERAHEIGHT = "CAMERAHEIGHT";
    private const string LAYERMASKNAME = "Plane";   //可拖动的层级
    private const float MOVE_DEVIATION = 5f;        //移动的偏差值
    private const float MOVE_CAMERA_CONFINER = 10;  //超出该限制后拉回相机
    private const float SCALE_SPEED = 2;            //相机无极缩放系数
    private const int FLIP_CONST = 20;              //自定义滑动系数
    private const float FLIP_DEVIATION = 10;        //滑动的偏差值
    private const float FLIP_SPEED = 3;            //滑动的速度
    private const float FLIP_STOP = 0.1f;           //滑动到多少距离停止
    private const float MIN_HEIGHT = 30;
    private const float MAX_HEIGHT = 65;
    private const float DEFAULT_HEIGHT = 47f;
    private const float MIN_ANGLE = 45;
    private const float MAX_ANGLE = 60;

    private GameObject m_Cam;
    private float cameraHeight; //相机高度
    private float scaleOffset; //缩放偏移值
    private float scaleLastDistance;
    private bool canMove;   //是否可以移动
    private bool canScale;  //是否可以无极缩放
    private bool canFlip;   //是否可以惯性移动
    private Vector2 startTouch; //按下时的屏幕坐标
    private Vector2 endTouch;   //松手时的屏幕坐标
    private Vector2 moveTempTouchPos; //移动时缓存的屏幕坐标
    private Vector3 moveDir;    //每次移动的方向
    private Vector3 flipTarget;     //滑动的终点

    private void OnEnable()
    {
        InputManager.Instance.Controller.UI.Press.started += PressStart;
        InputManager.Instance.Controller.UI.Press.canceled += PressCancel;
        InputManager.Instance.Controller.UI.Zoom.started += ScrollY;
        InputManager.Instance.Controller.UI.Zoom.canceled += SrollYCancel;
        InputManager.Instance.Controller.UI.SecondPress.started += OnSecondTouchPress;
        InputManager.Instance.Controller.UI.SecondPress.canceled += OnSecondTouchCancel;

        cameraHeight = PlayerPrefs.GetFloat(CAMERAHEIGHT, DEFAULT_HEIGHT);
    }

    private void Start()
    {
        m_Cam = this.gameObject;
        SetCameraMovRot();
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(CAMERAHEIGHT, cameraHeight);
    }

    private void OnDestroy()
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

    private void LateUpdate()
    {
        if (canScale)
        {
            var firstTouchPos = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();
            var secondTouchPos = InputManager.Instance.Controller.UI.SecondTouch.ReadValue<Vector2>();
            var distance = Vector2.Distance(secondTouchPos, firstTouchPos);
            if (scaleLastDistance != 0)
            {
                var offset = distance - scaleLastDistance;
                scaleOffset = offset;
                scaleLastDistance = distance;
            }
            else
            {
                scaleLastDistance = distance;
            }

            SetCameraHeight();

            //如果到了最大高度 或者 最低高度 如果还是 这个方向的数据就不移动
            //Debug.LogError(scaleOffset + " = distance = " + distance);

            canMove = false;
        }

        if (canMove)
        {
            var laterPoint = Vector3.zero;
            var afterPoint = Vector3.zero;
            var touchPos = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();
            if (moveTempTouchPos != Vector2.zero && moveTempTouchPos != touchPos)
            {
                Ray laterRay = Camera.main.ScreenPointToRay(moveTempTouchPos);
                if (Physics.Raycast(laterRay, out var point, 1000, 1 << LayerMask.NameToLayer(LAYERMASKNAME)))
                {
                    laterPoint = point.point;
                }

                Ray afterRay = Camera.main.ScreenPointToRay(touchPos);
                if (Physics.Raycast(afterRay, out var hit, 1000, 1 << LayerMask.NameToLayer(LAYERMASKNAME)))
                {
                    afterPoint = hit.point;
                }

                if (laterPoint != Vector3.zero && afterPoint != Vector3.zero && laterPoint != afterPoint)
                {
                    moveDir = afterPoint - laterPoint;
                    if (Mathf.Abs(m_Cam.transform.position.x - Camera.main.transform.position.x) > MOVE_CAMERA_CONFINER || Mathf.Abs(m_Cam.transform.position.z - Camera.main.transform.position.z) > MOVE_CAMERA_CONFINER) //相机超出边界自动拉回
                    {
                        //Debug.LogError("超出去了" + Mathf.Abs(m_VCam.transform.position.x - Camera.main.transform.position.x) + ":::" + Mathf.Abs(m_VCam.transform.position.z - Camera.main.transform.position.z));
                        m_Cam.transform.position = Camera.main.transform.position;
                    }

                    moveDir = new Vector3(moveDir.x, 0, moveDir.z); //避免y轴上的移动(仅平面移动)
                    var flwPos = m_Cam.transform.position;
                    var targetPos = flwPos - moveDir;
                    m_Cam.transform.position = targetPos;
                    moveTempTouchPos = touchPos;
                }
            }
            else
            {
                moveTempTouchPos = touchPos;
            }
        }

        if (canFlip)
        {
            var newVec = Vector3.Lerp(m_Cam.transform.position, flipTarget, Time.deltaTime * FLIP_SPEED);
            m_Cam.transform.position = newVec;
            if (Vector3.Distance(m_Cam.transform.position, flipTarget) < FLIP_STOP)
            {
                canFlip = false;
                Debug.LogError("滑动结束");
            }
        }
    }

    private void SetCameraHeight()
    {
        if (scaleOffset > 0)
            cameraHeight = m_Cam.transform.position.y + -SCALE_SPEED;
        else
            cameraHeight = m_Cam.transform.position.y + SCALE_SPEED;
        cameraHeight = Mathf.Clamp(cameraHeight, MIN_HEIGHT, MAX_HEIGHT);
        SetCameraMovRot();
    }

    private void SetCameraMovRot()
    {
        float rate = (cameraHeight - MIN_HEIGHT) / (MAX_HEIGHT - MIN_HEIGHT);
        float targetAngle = rate * (MAX_ANGLE - MIN_ANGLE) + MIN_ANGLE;

        m_Cam.transform.position = new Vector3(m_Cam.transform.position.x, cameraHeight, m_Cam.transform.position.z);
        Vector3 targetRotate = m_Cam.transform.rotation.eulerAngles;
        targetRotate.x = targetAngle;
        m_Cam.transform.rotation = Quaternion.Euler(targetRotate);
    }

    private void PressStart(InputAction.CallbackContext context)
    {
        canFlip = false; //移动时禁止相机滑动
        startTouch = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();
        canMove = true;
    }

    private void PressCancel(InputAction.CallbackContext context)
    {
        Debug.LogError("PressCancel canMove =" + canMove);
        endTouch = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();
        if (startTouch != Vector2.zero && endTouch != Vector2.zero && Vector2.Distance(startTouch, endTouch) > MOVE_DEVIATION)
        {
            var touchPosCancel = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();
            Debug.LogError($"两次不相等 ，属于移动了手指，其中最后一次的滑动距离 = {Vector2.Distance(touchPosCancel, moveTempTouchPos)}");

            if (Vector2.Distance(touchPosCancel, moveTempTouchPos) > FLIP_DEVIATION && canMove) //松手时触摸屏幕的坐标 和 最后一次移动时触摸屏幕的坐标 超过一定数值就惯性滑动
            {
                canFlip = true;
                flipTarget = m_Cam.transform.position - (moveDir.normalized * FLIP_CONST); //目标值
            }
        }
        else
        {
            //Debug.LogError("两次是相等的,进行点击逻辑");
        }
        canMove = false;
        moveTempTouchPos = Vector2.zero;
    }

    private void ScrollY(InputAction.CallbackContext context)
    {
        canFlip = false; //缩放时禁止相机滑动
        var offet = context.ReadValue<Vector2>();
        scaleOffset = offet.y;
        SetCameraHeight();
    }

    private void SrollYCancel(InputAction.CallbackContext contex)
    {
        scaleOffset = 0;
    }

    private void OnSecondTouchPress(InputAction.CallbackContext context)
    {
        canFlip = false; //缩放时禁止相机滑动
        canScale = true;
    }

    private void OnSecondTouchCancel(InputAction.CallbackContext context)
    {
        Debug.LogError("OnSecondTouchCancel");
        canScale = false;
        scaleLastDistance = 0;
    }
}