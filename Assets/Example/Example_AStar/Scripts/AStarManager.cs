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
                    GameObject starNodeObj = GameObject.Instantiate(AStarMain.instance.m_AStarNode, AStarMain.instance.transform);
                    var starNodePosX = i + 0.1f * i;
                    var starNodePosY = j + 0.1f * j; 
                    starNodeObj.transform.position = new Vector3(starNodePosX, 0f, starNodePosY);

                    var starNode = starNodeObj.AddComponent<AStarNode>();

                    //随机格子进行不可行走
                    int isStop = Random.Range(0, 5);
                    var nodeType = isStop == 0 ? NodeType.STOP : NodeType.WALK;

                    starNode.Init(starNodePosX, starNodePosY, nodeType);

                    nodes[i,j] = starNode;
                }
            }
        }

        public List<AStarNode> FindPath(AStarNode startNode, AStarNode endNode)
        {
            //转换V2到AStarNode 暂时采用直接传递Node方式

            //判断转换后是否阻挡
            if (startNode.nodeType == NodeType.STOP || endNode.nodeType == NodeType.STOP)
                return null;

            //将起点放进关闭列表中
            closeList.Add(startNode);

            //从起点开始找周围点


            //找到周围点，判空，判断是否为阻挡格
            //计算FGH
            //找到最小的F 加入到关闭队列
            //判断这个点 是否和终点吻合
            //继续查找流程
            return null;
        }

        public AStarNode GetNearNode(int x , int y)
        {


            return null;
        }
    }
}