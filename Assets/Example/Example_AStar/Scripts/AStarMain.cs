using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Example_AStar
{
    public class AStarMain : SingletonBehaviour<AStarMain>
    {
        [Header("地图默认宽高")]
        public int m_MapW;
        public int m_MapH;

        [Header("格子预制件相关")]
        public GameObject m_AStarNode;
        public Material m_WalkMat;
        public Material m_StopMat;
        public Material m_SelectStartMat;
        public Material m_SelectEndMat;

        [Header("选择的格子")]
        public AStarNode m_SelectStartNode;

        //临时变量
        public List<AStarNode> m_SelectNodes;
        private Vector2 mousePos;
        private RaycastHit raycastHit;
        private Ray ray;

        void OnEnable()
        {
            //兼容手机和电脑的触摸操作
            InputManager.Instance.Controller.Common.Press.started += OnTouchStarted;
        }

        private void OnTouchStarted(InputAction.CallbackContext obj)
        {
            mousePos = InputManager.Instance.Controller.Common.TouchScreen.ReadValue<Vector2>();
            ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out raycastHit, 1000))
            {
                if (raycastHit.collider.gameObject.tag == "AStar_Node")
                {
                    OnSelectNode(raycastHit.collider.GetComponent<AStarNode>());
                }
            }
        }

        void Start()
        {
            AStarManager.Instance.InitMap(m_MapW, m_MapH);
        }

        public void OnSelectNode(AStarNode node)
        {
            if (node.nodeType == NodeType.STOP)
            {
                Debug.LogWarning("改点不可进行寻路");
                return;
            }

            if (m_SelectStartNode == null)
            {
                ResetColor();
                ClearData();

                m_SelectStartNode = node;
                m_SelectStartNode.SetSelectStartColor();
            }
            else
            {
                if (m_SelectStartNode == node)
                {
                    Debug.LogWarning("寻路起点和终点重合");
                    return;
                }

                m_SelectNodes = AStarManager.Instance.FindPath(m_SelectStartNode, node);

                if (m_SelectNodes == null)
                    Debug.LogWarning("存在死路，本次寻路无法完成");

                ResetColor(false);
                m_SelectStartNode = null;
            }
        }

        //设置本次选中的颜色
        public void ResetColor(bool isReset = true)
        {
            if (m_SelectNodes == null)
                return;

            for (int i = 0; i < m_SelectNodes.Count; i++)
            {
                if (isReset)
                    m_SelectNodes[i].SetInitColor();
                else
                    m_SelectNodes[i].SetSelectEndColor();
            }
        }

        void OnDisable()
        {
            InputManager.Instance.Controller.Common.Press.started -= OnTouchStarted;
        }

        private void OnDestroy()
        {
            ClearData();
        }

        private void ClearData()
        {
            m_SelectStartNode = null;

            m_SelectNodes?.Clear();
            m_SelectNodes = null;
        }
    }
}