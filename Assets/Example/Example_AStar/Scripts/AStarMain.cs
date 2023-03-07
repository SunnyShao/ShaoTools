using UnityEngine;

namespace Example_AStar
{
    public class AStarMain : MonoBehaviour
    {
        public static AStarMain instance;

        [Header("��ͼĬ�Ͽ��")]
        public int m_MapW;
        public int m_MapH;

        [Header("����Ԥ�Ƽ����")]
        public GameObject m_AStarNode;
        public Material m_StopMat;

        private void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            AStarManager.Instance.InitMap(m_MapW, m_MapH);
        }
    }
}