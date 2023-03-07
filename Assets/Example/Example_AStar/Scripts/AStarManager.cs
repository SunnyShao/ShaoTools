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
                    GameObject starNodeObj = GameObject.Instantiate(AStarMain.Instance.m_AStarNode, AStarMain.Instance.transform);
                    var starNodePosX = i + 0.1f * i;
                    var starNodePosY = j + 0.1f * j;
                    starNodeObj.name = i.ToString() + "_" + j.ToString();
                    starNodeObj.transform.position = new Vector3(starNodePosX, 0f, starNodePosY);

                    var starNode = starNodeObj.AddComponent<AStarNode>();

                    //������ӽ��в�������
                    int isStop = Random.Range(0, 5);
                    var nodeType = isStop == 0 ? NodeType.STOP : NodeType.WALK;

                    starNode.Init(i, j, nodeType);

                    nodes[i, j] = starNode;
                }
            }



            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    Debug.LogError(nodes[i, j].name);
                }
            }
        }

        public List<AStarNode> FindPath(AStarNode startNode, AStarNode endNode)
        {
            //ת��V2��AStarNode ��ʱ����ֱ�Ӵ���Node��ʽ

            //�ж�ת�����Ƿ��赲
            if (startNode.nodeType == NodeType.STOP || endNode.nodeType == NodeType.STOP)
                return null;

            //��ʼ������
            openList.Clear();
            closeList.Clear();

            //�����Ž��ر��б���
            closeList.Add(startNode);

            //������������ ֱ����ǰ��ѯ���Ľڵ���յ��غ�
            while (true)
            {
                //����㿪ʼ����Χ��
                GetNearNode(startNode.x, startNode.y + 1, startNode, endNode);
                GetNearNode(startNode.x, startNode.y - 1, startNode, endNode);
                GetNearNode(startNode.x + 1, startNode.y - 1, startNode, endNode);
                GetNearNode(startNode.x + 1, startNode.y, startNode, endNode);
                GetNearNode(startNode.x + 1, startNode.y + 1, startNode, endNode);
                GetNearNode(startNode.x - 1, startNode.y - 1, startNode, endNode);
                GetNearNode(startNode.x - 1, startNode.y, startNode, endNode);
                GetNearNode(startNode.x - 1, startNode.y + 1, startNode, endNode);

                //openList���������� ֤������Ϊ��·
                if (openList.Count <= 0)
                    return null;

                //�ҵ���С��F ���뵽�رն���
                openList.Sort(OpenListSort);
                var curCloseNode = openList[0];
                closeList.Add(curCloseNode);
                openList.Remove(curCloseNode);

                startNode = curCloseNode;

                //�ж������ �Ƿ���յ��Ǻ�
                if (startNode == endNode)
                {
                    for (int i = 0; i < closeList.Count; i++)
                    {
                        Debug.LogError(closeList[i].name);
                    }
                    //closeList.Reverse(); //��ת�� �ӿ�ʼ�������ĵ�����
                    return closeList;
                }
            }
        }

        private int OpenListSort(AStarNode x, AStarNode y)
        {
            if (x.F > y.F)
                return 1;
            else if (x.F < y.F)
                return -1;
            else return 1;
        }

        //Ѱ����Χ�ڵ�
        public void GetNearNode(int x, int y, AStarNode parentNode, AStarNode endNode)
        {
            if (x < 0 || x >= mapW || y < 0 || y >= mapH)
                return;

            //�����赲 Ҳ�����ڿ����͹ر��б�
            AStarNode curNode = nodes[x, y];
            if (curNode.nodeType != NodeType.STOP && !openList.Contains(curNode) && !closeList.Contains(curNode))
            {
                openList.Add(curNode);
                curNode.SetNodeParent(parentNode);
                //����FGH
                curNode.SetAStarValue(parentNode, endNode);
            }
        }
    }
}