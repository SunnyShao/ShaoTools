using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example_AStar
{
    public class AStarMain : MonoBehaviour
    {
        public static AStarMain instance;

        [Header("地图默认宽高")]
        public int m_MapW;
        public int m_MapH;

        [Header("格子预制件相关")]
        public Transform m_AStarParent;
        public GameObject m_AStarNode;

        private void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            AStarManager.Instance.InitMap(m_MapW, m_MapH);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}