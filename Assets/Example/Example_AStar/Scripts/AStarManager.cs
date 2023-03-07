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

        //地图所有格子的容器
        private AStarNode[,] nodes;

        public List<AStarNode> openList = new List<AStarNode>();
        public List<AStarNode> closeList = new List<AStarNode>();

        [Header("格子宽高")]
        public int mapW;
        public int mapH;

        public void InitMap(int curW, int curH)
        {
            //设置好地图宽高
            this.mapW = curW;
            this.mapH = curH;
            nodes = new AStarNode[curW, curH];

            //根据地图宽高生成格子
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

                    //随机格子进行不可行走
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
            //转换V2到AStarNode 暂时采用直接传递Node方式

            //判断转换后是否阻挡
            if (startNode.nodeType == NodeType.STOP || endNode.nodeType == NodeType.STOP)
                return null;

            //初始化数据
            openList.Clear();
            closeList.Clear();

            //将起点放进关闭列表中
            closeList.Add(startNode);

            //持续查找流程 直到当前查询到的节点和终点重合
            while (true)
            {
                //从起点开始找周围点
                GetNearNode(startNode.x, startNode.y + 1, startNode, endNode);
                GetNearNode(startNode.x, startNode.y - 1, startNode, endNode);
                GetNearNode(startNode.x + 1, startNode.y - 1, startNode, endNode);
                GetNearNode(startNode.x + 1, startNode.y, startNode, endNode);
                GetNearNode(startNode.x + 1, startNode.y + 1, startNode, endNode);
                GetNearNode(startNode.x - 1, startNode.y - 1, startNode, endNode);
                GetNearNode(startNode.x - 1, startNode.y, startNode, endNode);
                GetNearNode(startNode.x - 1, startNode.y + 1, startNode, endNode);

                //openList不存在数据 证明本次为死路
                if (openList.Count <= 0)
                    return null;

                //找到最小的F 加入到关闭队列
                openList.Sort(OpenListSort);
                var curCloseNode = openList[0];
                closeList.Add(curCloseNode);
                openList.Remove(curCloseNode);

                startNode = curCloseNode;

                //判断这个点 是否和终点吻合
                if (startNode == endNode)
                {
                    for (int i = 0; i < closeList.Count; i++)
                    {
                        Debug.LogError(closeList[i].name);
                    }
                    //closeList.Reverse(); //翻转后 从开始到结束的点排序
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

        //寻找周围节点
        public void GetNearNode(int x, int y, AStarNode parentNode, AStarNode endNode)
        {
            if (x < 0 || x >= mapW || y < 0 || y >= mapH)
                return;

            //不是阻挡 也不存在开启和关闭列表
            AStarNode curNode = nodes[x, y];
            if (curNode.nodeType != NodeType.STOP && !openList.Contains(curNode) && !closeList.Contains(curNode))
            {
                openList.Add(curNode);
                curNode.SetNodeParent(parentNode);
                //计算FGH
                curNode.SetAStarValue(parentNode, endNode);
            }
        }
    }
}