using UnityEngine;

namespace Example_CreateClock
{
    public class ClockManager : MonoBehaviour
    {
        private const int ClockHours = 12;      //ʱ������12��Сʱ��ʾ
        private const int EveryHourAngle = 30;  //ÿ����ʱ��Ƕȼ��

        private GameObject hourItem;

        private void Awake()
        {
            hourItem = transform.Find("Hour_Indicator_Item").gameObject;
        }

        void Start()
        {
            for (int i = 0; i < ClockHours; i++)
            {
                GameObject hourSpawn = Instantiate(hourItem, transform);
                hourSpawn.transform.rotation = Quaternion.Euler(new Vector3(0f, i * EveryHourAngle, 0f));
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}