using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BattleCameraComponent : MonoBehaviour
{
    private const string LAYERMASKNAME = "Plane";   //���϶��Ĳ㼶
    private const float MOVE_DEVIATION = 5f;        //�ƶ���ƫ��ֵ
    private const float MOVE_CAMERA_CONFINER = 10;  //���������ƺ��������
    private const float SCALE_SPEED = 4;            //����޼�����ϵ��
    private const int FLIP_CONST = 20;              //�Զ��廬��ϵ��
    private const float FLIP_DEVIATION = 10;        //������ƫ��ֵ
    private const float FLIP_SPEED = 3;             //�������ٶ�
    private const float FLIP_STOP = 0.1f;           //���������پ���ֹͣ
    private const float MIN_HEIGHT = 110;
    private const float MAX_HEIGHT = 300;
    private const float MIN_ANGLE = 70;
    private const float MAX_ANGLE = 80;

    private GameObject m_Cam;
    private float cameraHeight;         //����߶�
    private float scaleOffset;          //����ƫ��ֵ
    private float scaleLastDistance;
    private bool canMove;               //�Ƿ�����ƶ�
    private bool canScale;              //�Ƿ�����޼�����
    private bool canFlip;               //�Ƿ���Թ����ƶ�
    private bool isPressAction;         //�Ƿ񴥷��¼�
    private Vector2 startTouch;         //����ʱ����Ļ����
    private Vector2 endTouch;           //����ʱ����Ļ����
    private Vector2 moveTempTouchPos;   //�ƶ�ʱ�������Ļ����
    private Vector3 moveDir;            //ÿ���ƶ��ķ���
    private Vector3 flipTarget;         //�������յ�

    private void OnEnable()
    {
        InputManager.Instance.Controller.UI.Press.started += PressStart;
        InputManager.Instance.Controller.UI.Press.canceled += PressCancel;
        InputManager.Instance.Controller.UI.Zoom.started += ScrollY;
        InputManager.Instance.Controller.UI.Zoom.canceled += SrollYCancel;
        InputManager.Instance.Controller.UI.SecondPress.started += OnSecondTouchPress;
        InputManager.Instance.Controller.UI.SecondPress.canceled += OnSecondTouchCancel;
        cameraHeight = MAX_HEIGHT;
    }

    private void Start()
    {
        m_Cam = this.gameObject;
        SetCameraMovRot();
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

    private void Update()
    {
        if (isPressAction) //����ص�����Update�д���������InputSystem�д��������߼�
        {
            isPressAction = false;
            if (!EventSystem.current.IsPointerOverGameObject())
                UIMovePanel.instance.OnMapClick(InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>());
        }
    }

    private void LateUpdate()
    {
        if (canScale && !EventSystem.current.IsPointerOverGameObject())
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
            //����������߶� ���� ��͸߶� ������� �����������ݾͲ��ƶ�
            //Debug.LogError(scaleOffset + " = distance = " + distance);
            canMove = false;
        }

        if (canMove && !EventSystem.current.IsPointerOverGameObject())
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
                    if (Mathf.Abs(m_Cam.transform.position.x - Camera.main.transform.position.x) > MOVE_CAMERA_CONFINER || Mathf.Abs(m_Cam.transform.position.z - Camera.main.transform.position.z) > MOVE_CAMERA_CONFINER) //��������߽��Զ�����
                    {
                        m_Cam.transform.position = Camera.main.transform.position;
                    }

                    moveDir = new Vector3(moveDir.x, 0, moveDir.z); //����y���ϵ��ƶ�(��ƽ���ƶ�)
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
        else
            canMove = false;  //�����UI�� ֱ�����ò����ƶ� ���ú������Ի�������ִ��

        if (canFlip)
        {
            var newVec = Vector3.Lerp(m_Cam.transform.position, flipTarget, Time.deltaTime * FLIP_SPEED);
            m_Cam.transform.position = newVec;
            if (Vector3.Distance(m_Cam.transform.position, flipTarget) < FLIP_STOP)
            {
                canFlip = false;
            }
        }

        if (scaleOffset != 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            SetCameraHeight();
        }
    }

    private void PressStart(InputAction.CallbackContext context)
    {
        canFlip = false; //�ƶ�ʱ��ֹ�������
        startTouch = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();
        canMove = true;
    }

    private void PressCancel(InputAction.CallbackContext context)
    {
        endTouch = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();
        if (startTouch != Vector2.zero && endTouch != Vector2.zero && Vector2.Distance(startTouch, endTouch) > MOVE_DEVIATION)
        {
            var touchPosCancel = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();
            if (Vector2.Distance(touchPosCancel, moveTempTouchPos) > FLIP_DEVIATION && canMove) //����ʱ������Ļ������ �� ���һ���ƶ�ʱ������Ļ������ ����һ����ֵ�͹��Ի���
            {
                canFlip = true;
                flipTarget = m_Cam.transform.position - (moveDir.normalized * FLIP_CONST); //Ŀ��ֵ
            }
        }
        else
        {
            isPressAction = true; //��������ȵ�,���е���߼�
        }
        canMove = false;
        moveTempTouchPos = Vector2.zero;
    }

    private void ScrollY(InputAction.CallbackContext context)
    {
        canFlip = false; //����ʱ��ֹ�������
        var offet = context.ReadValue<Vector2>();
        scaleOffset = offet.y;
    }

    private void SrollYCancel(InputAction.CallbackContext contex)
    {
        scaleOffset = 0;
    }

    private void OnSecondTouchPress(InputAction.CallbackContext context)
    {
        canFlip = false; //����ʱ��ֹ�������
        canScale = true;
    }

    private void OnSecondTouchCancel(InputAction.CallbackContext context)
    {
        canScale = false;
        scaleLastDistance = 0;
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
}