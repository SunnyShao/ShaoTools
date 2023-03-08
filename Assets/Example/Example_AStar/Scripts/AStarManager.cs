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

        private List<AStarNode> openList = new List<AStarNode>();
        private List<AStarNode> closeList = new List<AStarNode>();

        private int mapW;
        private int mapH;

        private List<AStarNode> returnNodes = new List<AStarNode>();

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

                    //随机格子进行不可行走 正式项目都是读取配置
                    int isStop = Random.Range(0, 5);
                    var nodeType = isStop == 0 ? NodeType.STOP : NodeType.WALK;

                    starNode.Init(i, j, nodeType);

                    nodes[i, j] = starNode;
                }
            }
        }

        public List<AStarNode> FindPath(AStarNode startNode, AStarNode endNode)
        {
            //初始化数据
            openList.Clear();
            closeList.Clear();
            returnNodes.Clear();

            //将起点放进关闭列表中
            closeList.Add(startNode);

            //起点的数据和父节点要重置，否则会记录上次的父节点，导致死循环
            startNode.Reset();

            //持续查找流程 直到当前查询到的最小F值节点和终点重合
            while (true)
            {
                //从起点开始找周围8个点
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

                //排序找到最小的F值
                openList.Sort(OpenListSort);

                //最小F值的节点 加入到关闭队列
                var curCloseNode = openList[0];
                closeList.Add(curCloseNode);
                openList.Remove(curCloseNode);

                //下一轮起始查找节点为刚找到的F值最小节点
                startNode = curCloseNode;

                //判断这个点 是否和终点吻合
                if (startNode == endNode)
                {
                    //依次将父节点加入列表中
                    returnNodes.Add(endNode);
                    while(endNode.parentNode != null)
                    {
                        returnNodes.Add(endNode.parentNode);
                        endNode = endNode.parentNode;
                    }
                    returnNodes.Reverse(); //翻转后 从开始到结束的点排序
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
        /// 寻找周围节点
        /// </summary>
        /// <param name="x">要查找节点的X索引</param>
        /// <param name="y">要查找节点的Y索引</param>
        /// <param name="parentNode">父级节点：是由谁查找过来的节点</param>
        /// <param name="endNode">最终节点目标</param>
        public void GetNearNode(int x, int y, AStarNode parentNode, AStarNode endNode)
        {
            if (x < 0 || x >= mapW || y < 0 || y >= mapH)
                return;

            //不是阻挡 也不存在开启和关闭列表
            AStarNode curNode = nodes[x, y];
            if (curNode.nodeType != NodeType.STOP && !openList.Contains(curNode) && !closeList.Contains(curNode))
            {
                //将四周符合的节点添加到开启列表中
                openList.Add(curNode);
                //设置好父级节点
                curNode.SetNodeParent(parentNode);
                //计算FGH
                curNode.SetAStarValue(parentNode, endNode);
            }
        }
    }
}