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
        public float x;
        public float y;

        [Header("���ڵ�")]
        public AStarNode father;

        [Header("����")]
        public NodeType nodeType;

        public void Init(float x, float y, NodeType type)
        {
            this.x = x;
            this.y = y;
            this.nodeType = type;

            if (this.nodeType == NodeType.STOP)
                gameObject.GetComponent<MeshRenderer>().material = AStarMain.instance.m_StopMat;
        }

        public void SetAStarValue(AStarNode startNode, AStarNode endNode)
        {
            var startPos = new Vector2(startNode.x, startNode.y);
            var endPos = new Vector2(endNode.x, endNode.y);

            G = Vector2.Distance(new Vector2(x, y), startPos);
            H = Vector2.Distance(startPos, endPos);
            F = G + H;
        }

        public void SetNodeFather(AStarNode fatherNode)
        {
            father = fatherNode;
        }
    }
}