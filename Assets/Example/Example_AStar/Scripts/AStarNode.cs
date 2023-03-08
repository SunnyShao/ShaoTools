using UnityEngine;

namespace Example_AStar
{
    public enum NodeType
    {
        WALK = 0,
        STOP,
    }

    public class AStarNode : MonoBehaviour
    {
        [Header("AStarֵ")]
        public float F;
        public float G;
        public float H;

        [Header("����")]
        public int x;
        public int y;

        [Header("���ڵ�")]
        public AStarNode parentNode;

        [Header("����")]
        public NodeType nodeType;

        private MeshRenderer curMeshRenderer;

         void Awake()
        {
            curMeshRenderer = GetComponent<MeshRenderer>();
        }

        public void Init(int x, int y, NodeType type)
        {
            this.x = x;
            this.y = y;
            this.nodeType = type;
            SetInitColor();
        }

        public void Reset()
        {
            F = 0;
            G = 0;
            H = 0;
            parentNode = null;
        }

        public void SetAStarValue(AStarNode startNode, AStarNode endNode)
        {
            var startPos = new Vector2(startNode.x, startNode.y);
            G = parentNode.G + Mathf.Abs(Vector2.Distance(new Vector2(x, y), startPos)); //���ڵ���������� + ��ǰ�ڵ���봫������ʼ�����
            H = Mathf.Abs(endNode.x - x) + Mathf.Abs(endNode.y - y);    //��ǰ�ڵ㵽�յ����
            F = G + H;
        }

        public void SetNodeParent(AStarNode parentNode)
        {
            this.parentNode = parentNode;
        }

        public void SetInitColor()
        {
            curMeshRenderer.material = this.nodeType == NodeType.STOP ? AStarMain.Instance.m_StopMat : AStarMain.Instance.m_WalkMat;
        }

        public void SetSelectStartColor()
        {
            curMeshRenderer.material = AStarMain.Instance.m_SelectStartMat;
        }

        public void SetSelectEndColor()
        {
            curMeshRenderer.material = AStarMain.Instance.m_SelectEndMat;
        }
    }
}