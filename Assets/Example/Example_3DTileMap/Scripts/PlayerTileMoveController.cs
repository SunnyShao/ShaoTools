using UnityEngine;

public class PlayerTileMoveController : MonoBehaviour
{
    [Header("出生点")]
    public Transform bornTrans;

    [Header("主角移速")]
    public float moveSpeed;
    [Header("主角飞翔移速")]
    public float flySpeed;

    [SerializeField]
    [Header("主角当前格子坐标")]
    private Vector3Int cellPos;

    // 本次移动的目标方向
    private Vector3Int targetMoveDir;
    // 本次移动的目标点
    private Vector3Int targetCellPos;
    // 是否移动中
    private bool isMove;
    // 是否在地面
    private bool isGround = true;
    // 是否飞行中
    private bool isFly = false;
    // 本地移动目标点
    private Vector3 targetPos;
    // 角色模型偏移值
    private Vector3 playerOffestPos = new Vector3(1.5f, 1.5f, 0f);

    private Rigidbody rigidbody;

    // 缓存移动方向
    private float input_X;
    private float input_Y;
    private Vector3 input_MoveDir;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        //rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        targetCellPos = Vector3Int.zero;
        targetMoveDir = Vector3Int.zero;
        isMove = false;
        isGround = true;
        isFly = false;

        // 设置出生点
        Vector3 bornCellPos = TileMapManager.Instance.GetWorldPosByWorldPos(bornTrans.position);
        transform.position = bornCellPos + playerOffestPos;
        // 获得当前主角对应格子数据
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
        if (isMove || !isGround) return;

        targetMoveDir = new Vector3Int(0, -1, 0);
        targetCellPos = new Vector3Int(cellPos.x, cellPos.y - 1, 0);
        PlayerMove();
    }

    private void OnDownMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
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
        targetMoveDir = Vector3Int.zero;
        targetCellPos = Vector3Int.zero;
    }

    private void PlayerMove()
    {
        if (CheckPlayerIsWall())
        {
            Debug.LogError("触碰到墙");
            return;
        }

        isMove = true;

        TileMapManager.Instance.DeleteCell(targetCellPos);

        targetPos = TileMapManager.Instance.GetWorldPosByCellPos(targetCellPos);
        cellPos = TileMapManager.Instance.GetCellPosByWorldPos(targetPos);
        targetPos += playerOffestPos;
        Debug.Log($"当前坐标 {transform.position} 向目标点移动 {targetPos} 目标点格子 {cellPos}");
    }

    private void OnFlyStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //如果处在移动中，那么不能飞行
        if (isMove) return;

        //判断头顶是否存在格子，如果存在格子不能开始飞
        Vector3Int targetFlyCellPos = new Vector3Int(cellPos.x, cellPos.y + 1, 0);
        bool hasTopCell = TileMapManager.Instance.IsCellDataByCellPos(targetFlyCellPos);
        if (hasTopCell) return;

        //rigidbody.isKinematic = false;
        isGround = false;
        isFly = true;
        //rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
    }

    private void OnFlyCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isFly = false;
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
            //Debug.LogError("目标点 + " + targetPos);
            float step = moveSpeed * Time.deltaTime;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, step);

            if (Vector3.Distance(transform.localPosition, targetPos) < 0.01f)
            {
                OnMoveOver();
            }
        }

        if (!isGround)
        {
            cellPos = TileMapManager.Instance.GetCellPosByWorldPos(transform.position);

            if (!isFly)
            {
                //不在地面，并且没有启动飞行的时候，证明是在下落，要时刻检查角色是否落到地面
                CheckPlayerIsGround();
            }
            else
            {
                //不在地面，并且是启动飞行 才会执行刚体施加力逻辑
                rigidbody.AddForce(input_MoveDir * flySpeed * Time.deltaTime, ForceMode.Force);
            }
        }
    }

    // 检测角色是否在地面
    private bool CheckPlayerIsGround()
    {
        // 检测方案二  向脚下发射一个碰撞块判断是否碰到地面
        var raycastColliders = Physics.OverlapBox(transform.position - new Vector3(0f, 1.5f, 0f), new Vector3(1f, 0.1f, 1f), Quaternion.identity, 1 << LayerMask.NameToLayer("Ground"));
        if (raycastColliders.Length > 0)
        {
            Debug.Log(raycastColliders.Length + "到地面了 触碰到= " + raycastColliders[0].name);
            isGround = true;
            //rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            cellPos = TileMapManager.Instance.GetCellPosByWorldPos(transform.position);
        }
        else if (isGround)
        {
            Debug.Log("脚下没有东西 = " + cellPos);
            isGround = false;
            //rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            cellPos = TileMapManager.Instance.GetCellPosByWorldPos(transform.position);
        }
        return isGround;

        // 检测方案一  向脚下发射射线判断是否碰到地面(问题在于正好卡在地面边缘射线无法判断)
        //if (Physics.Raycast(transform.position, Vector3.down, out var hitInfo, 1.6f, 1 << LayerMask.NameToLayer("Ground")))
        //{
        //    Debug.Log("到地面了 触碰到= " + hitInfo.transform.name);
        //    isGround = true;
        //    //rigidbody.useGravity = false;
        //    rigidbody.isKinematic = true;
        //    cellPos = TileMapManager.Instance.GetCellPosByWorldPos(transform.position);
        //}
        //else if (isGround)
        //{
        //    Debug.Log("脚下没有东西 = " + cellPos);
        //    isGround = false;
        //    //rigidbody.useGravity = true;
        //    rigidbody.isKinematic = false;
        //    cellPos = TileMapManager.Instance.GetCellPosByWorldPos(transform.position);
        //}
        //return isGround;
    }

    // 检测角色是否碰到墙体
    private bool CheckPlayerIsWall()
    {
        return Physics.Raycast(transform.position, targetMoveDir, 2f, 1 << LayerMask.NameToLayer("Wall"));
    }

    private void OnDrawGizmosSelected()
    {
        // 方案一 画线
        //Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, Vector3.down * 1.6f);

        // 方案二 画线
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position - new Vector3(0f, 1.5f, 0f), new Vector3(1f, 0.1f, 1f));
    }

    // 当移动完成处理
    public void OnMoveOver()
    {
        isMove = false;
        targetPos = Vector3.zero;

        // 判断本次移动后玩家是否处于地面
        if (CheckPlayerIsGround())
        {
            // 玩家在地面，并且还在持续按下移动按钮，则继续下一次移动
            if (targetMoveDir != Vector3Int.zero)
            {
                targetCellPos += targetMoveDir;
                PlayerMove();
            }
        }
        else
        {
            targetMoveDir = Vector3Int.zero;
            targetCellPos = Vector3Int.zero;
        }
    }
}