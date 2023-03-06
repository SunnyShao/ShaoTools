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

            //���ݵ�ͼ������ɸ���
            nodes = new AStarNode[curW, curH];
            for (int i = 0; i < curW; i++)
            {
                for (int j = 0; j < curH; j++)
                {
                    GameObject starNodeObj = GameObject.Instantiate(AStarMain.instance.m_AStarNode, AStarMain.instance.m_AStarParent);
                    var starNodePosX = i + 0.1f * i;
                    var starNodePosY = j + 0.1f * j;
                    starNodeObj.transform.position = new Vector3(starNodePosX, 0f, starNodePosY);
                    var starNode = starNodeObj.AddComponent<AStarNode>();
                    starNode.x = starNodePosX;
                    starNode.y = starNodePosY;

                    //������ӽ��в�������
                    int isStop = Random.Range(0, 5);
                    starNode.nodeType = isStop == 0 ? NodeType.STOP : NodeType.WALK;
                }
            }
        }

        public List<AStarNode> FindPath(Vector3 startNode, Vector3 endNode)
        {
            //ת��V2��AStarNode
            //�ж�ת�����Ƿ��赲
            //����㿪ʼ����Χ��
            //�ҵ���Χ�㣬�пգ��ж��Ƿ�Ϊ�赲��
            //����FGH
            //�ҵ���С��F ���뵽�رն���
            //�ж������ �Ƿ���յ��Ǻ�
            //������������
            return null;
        }
    }
}