using UnityEngine;

namespace Example_CreateClock
{
    public class ClockManager : MonoBehaviour
    {
        private const int ClockHours = 12;      //时钟上有12个小时显示
        private const int EveryHourAngle = 30;  //每两个时针角度间隔

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