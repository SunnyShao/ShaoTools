using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Example_AStar
{
    public class AStarMain : SingletonBehaviour<AStarMain>
    {
        [Header("��ͼĬ�Ͽ��")]
        public int m_MapW;
        public int m_MapH;

        [Header("����Ԥ�Ƽ����")]
        public GameObject m_AStarNode;
        public Material m_WalkMat;
        public Material m_StopMat;
        public Material m_SelectStartMat;
        public Material m_SelectEndMat;

        [Header("ѡ��ĸ���")]
        public AStarNode m_SelectStartNode;

        //��ʱ����
        public List<AStarNode> m_SelectNodes;
        private Vector2 mousePos;
        private RaycastHit raycastHit;
        private Ray ray;

        void OnEnable()
        {
            //�����ֻ��͵��ԵĴ�������
            InputManager.Instance.Controller.Common.TouchScreen.performed += OnTouchScreenPerformed;
            InputManager.Instance.Controller.Common.Press.started += OnTouchStarted;
        }

        private void OnTouchStarted(InputAction.CallbackContext obj)
        {
            ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out raycastHit, 1000))
            {
                if (raycastHit.collider.gameObject.tag == "AStar_Node")
                {
                    Debug.LogError("����������� = " + raycastHit.collider.gameObject.name);
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
            Debug.Log("AStarMain OnDisable" + (InputManager.Instance == null));
            InputManager.Instance.Controller.Common.TouchScreen.performed -= OnTouchScreenPerformed;
            InputManager.Instance.Controller.Common.Press.started -= OnTouchStarted;
        }

        private void OnDestroy()
        {
            m_SelectNodes.Clear();
            m_SelectNodes = null;
        }

        public void OnSelectNode(AStarNode node)
        {
            if (node.nodeType == NodeType.STOP)
            {
                Debug.LogWarning("�ĵ㲻�ɽ���Ѱ·");
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
                    Debug.LogWarning("Ѱ·�����յ��غ�");
                    return;
                }

                m_SelectNodes = AStarManager.Instance.FindPath(m_SelectStartNode, node);

                if (m_SelectNodes == null)
                    Debug.LogWarning("������·������Ѱ·�޷����");

                ResetColor(false);
                m_SelectStartNode = null;
            }
        }

        //���ñ���ѡ�е���ɫ
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