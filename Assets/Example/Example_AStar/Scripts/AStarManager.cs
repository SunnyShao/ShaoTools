using System.Collections.Generic;
using UnityEngine;

namespace Example_AStar
{
    public class AStarManager
    {
        private static AStarManager instance;

        public static AStarManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new AStarManager();
                return instance;
            }
        }

        //��ͼ���и��ӵ�����
        private AStarNode[,] nodes;

        public List<AStarNode> openList = new List<AStarNode>();
        public List<AStarNode> closeList = new List<AStarNode>();

        [Header("���ӿ��")]
        public int mapW;
        public int mapH;

        public void InitMap(int curW, int curH)
        {  
            //���úõ�ͼ���
            this.mapW = curW;
            this.mapH = curH;
            nodes = new AStarNode[curW, curH];

            //���ݵ�ͼ������ɸ���
            for (int i = 0; i < curW; i++)
            {
                for (int j = 0; j < curH; j++)
                {
                    GameObject starNodeObj = GameObject.Instantiate(AStarMain.instance.m_AStarNode, AStarMain.instance.transform);
                    var starNodePosX = i + 0.1f * i;
                    var starNodePosY = j + 0.1f * j; 
                    starNodeObj.transform.position = new Vector3(starNodePosX, 0f, starNodePosY);

                    var starNode = starNodeObj.AddComponent<AStarNode>();

                    //������ӽ��в�������
                    int isStop = Random.Range(0, 5);
                    var nodeType = isStop == 0 ? NodeType.STOP : NodeType.WALK;

                    starNode.Init(starNodePosX, starNodePosY, nodeType);

                    nodes[i,j] = starNode;
                }
            }
        }

        public List<AStarNode> FindPath(AStarNode startNode, AStarNode endNode)
        {
            //ת��V2��AStarNode ��ʱ����ֱ�Ӵ���Node��ʽ

            //�ж�ת�����Ƿ��赲
            if (startNode.nodeType == NodeType.STOP || endNode.nodeType == NodeType.STOP)
                return null;

            //�����Ž��ر��б���
            closeList.Add(startNode);

            //����㿪ʼ����Χ��


            //�ҵ���Χ�㣬�пգ��ж��Ƿ�Ϊ�赲��
            //����FGH
            //�ҵ���С��F ���뵽�رն���
            //�ж������ �Ƿ���յ��Ǻ�
            //������������
            return null;
        }

        public AStarNode GetNearNode(int x , int y)
        {


            return null;
        }
    }
}