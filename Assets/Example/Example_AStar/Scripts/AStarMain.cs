using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

        public List<AStarNode> m_SelectNodes;
        private Vector2 mousePos;

        void OnEnable()
        {
            //兼容手机和电脑的触摸操作
            InputManager.Instance.Controller.Common.TouchScreen.performed += OnTouchScreenPerformed;
            InputManager.Instance.Controller.Common.Press.started += OnTouchStarted;
        }

        private void OnTouchStarted(InputAction.CallbackContext obj)
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out raycastHit, 1000))
            {
                if (raycastHit.collider.gameObject.tag == "AStar_Node")
                {
                    Debug.LogError("检测物体名字 = " + raycastHit.collider.gameObject.name);
                    OnSelectNode(raycastHit.collider.GetComponent<AStarNode>());
                }
            }
        }

        private void OnTouchScreenPerformed(InputAction.CallbackContext obj)
        {
            mousePos = obj.ReadValue<Vector2>();
        }

        void Start()
        {
            AStarManager.Instance.InitMap(m_MapW, m_MapH);
        }

        void OnDisable()
        {
            InputManager.Instance.Controller.Common.TouchScreen.performed -= OnTouchScreenPerformed;
            InputManager.Instance.Controller.Common.Press.started -= OnTouchStarted;
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
                Debug.LogError(m_SelectNodes.Count);
                ResetColor(false);
                m_SelectStartNode = null;
            }
        }

        //设置本次选中的颜色
        public void ResetColor(bool isReset = true)
        {
            for (int i = 0; i < m_SelectNodes.Count; i++)
            {
                if (isReset)
                    m_SelectNodes[i].SetInitColor();
                else
                    m_SelectNodes[i].SetSelectEndColor();
            }
        }
    }
}