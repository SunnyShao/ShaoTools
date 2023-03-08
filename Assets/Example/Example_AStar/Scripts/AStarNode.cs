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
        [Header("AStar值")]
        public float F;
        public float G;
        public float H;

        [Header("坐标")]
        public int x;
        public int y;

        [Header("父节点")]
        public AStarNode parentNode;

        [Header("类型")]
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
            G = parentNode.G + Mathf.Abs(Vector2.Distance(new Vector2(x, y), startPos)); //父节点距离起点距离 + 当前节点距离传进来起始点距离
            H = Mathf.Abs(endNode.x - x) + Mathf.Abs(endNode.y - y);    //当前节点到终点距离
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