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

        private List<AStarNode> openList = new List<AStarNode>();
        private List<AStarNode> closeList = new List<AStarNode>();

        private int mapW;
        private int mapH;

        private List<AStarNode> returnNodes = new List<AStarNode>();

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

                    //������ӽ��в������� ��ʽ��Ŀ���Ƕ�ȡ����
                    int isStop = Random.Range(0, 5);
                    var nodeType = isStop == 0 ? NodeType.STOP : NodeType.WALK;

                    starNode.Init(i, j, nodeType);

                    nodes[i, j] = starNode;
                }
            }
        }

        public List<AStarNode> FindPath(AStarNode startNode, AStarNode endNode)
        {
            //��ʼ������
            openList.Clear();
            closeList.Clear();
            returnNodes.Clear();

            //�����Ž��ر��б���
            closeList.Add(startNode);

            //�������ݺ͸��ڵ�Ҫ���ã�������¼�ϴεĸ��ڵ㣬������ѭ��
            startNode.Reset();

            //������������ ֱ����ǰ��ѯ������СFֵ�ڵ���յ��غ�
            while (true)
            {
                //����㿪ʼ����Χ8����
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

                //�����ҵ���С��Fֵ
                openList.Sort(OpenListSort);

                //��СFֵ�Ľڵ� ���뵽�رն���
                var curCloseNode = openList[0];
                closeList.Add(curCloseNode);
                openList.Remove(curCloseNode);

                //��һ����ʼ���ҽڵ�Ϊ���ҵ���Fֵ��С�ڵ�
                startNode = curCloseNode;

                //�ж������ �Ƿ���յ��Ǻ�
                if (startNode == endNode)
                {
                    //���ν����ڵ�����б���
                    returnNodes.Add(endNode);
                    while(endNode.parentNode != null)
                    {
                        returnNodes.Add(endNode.parentNode);
                        endNode = endNode.parentNode;
                    }
                    returnNodes.Reverse(); //��ת�� �ӿ�ʼ�������ĵ�����
                    return returnNodes;
                }
            }
        }

        private int OpenListSort(AStarNode x, AStarNode y)
        {
            if (x.F >= y.F)
                return 1;
            else if (x.F < y.F)
                return -1;
            else return 1;
        }

        /// <summary>
        /// Ѱ����Χ�ڵ�
        /// </summary>
        /// <param name="x">Ҫ���ҽڵ��X����</param>
        /// <param name="y">Ҫ���ҽڵ��Y����</param>
        /// <param name="parentNode">�����ڵ㣺����˭���ҹ����Ľڵ�</param>
        /// <param name="endNode">���սڵ�Ŀ��</param>
        public void GetNearNode(int x, int y, AStarNode parentNode, AStarNode endNode)
        {
            if (x < 0 || x >= mapW || y < 0 || y >= mapH)
                return;

            //�����赲 Ҳ�����ڿ����͹ر��б�
            AStarNode curNode = nodes[x, y];
            if (curNode.nodeType != NodeType.STOP && !openList.Contains(curNode) && !closeList.Contains(curNode))
            {
                //�����ܷ��ϵĽڵ���ӵ������б���
                openList.Add(curNode);
                //���úø����ڵ�
                curNode.SetNodeParent(parentNode);
                //����FGH
                curNode.SetAStarValue(parentNode, endNode);
            }
        }
    }
}