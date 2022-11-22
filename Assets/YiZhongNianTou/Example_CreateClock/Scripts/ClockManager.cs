using System;
using UnityEngine;

namespace Example_CreateClock
{
    public class ClockManager : MonoBehaviour
    {
        private const int ClockHours = 12;      //ʱ������12��Сʱ��ʾ
        private const int EveryHourAngle = 30;  //ÿ����ʱ��Ƕȼ��
        private const int EveryMinuteAngle = 6;

        //12��Сʱ����
        private GameObject hourItem;

        //ʱ�� ���� ����
        private Transform hour_Arm;
        private Transform minute_Arm;
        private Transform second_Arm;

        private void Awake()
        {
            hourItem = transform.Find("Hour_Indicator_Item").gameObject;
            hour_Arm = transform.Find("Hour_Arm");
            minute_Arm = transform.Find("Minute_Arm");
            second_Arm = transform.Find("Second_Arm");
        }

        void Start()
        {
            Debug.Log(DateTime.Now);
            Debug.Log(DateTime.Now.Hour);
            for (int i = 0; i < ClockHours; i++)
            {
                GameObject hourSpawn = Instantiate(hourItem, transform);
                hourSpawn.transform.rotation = Quaternion.Euler(new Vector3(0f, i * EveryHourAngle, 0f));
            }
        }

#if CLOCK_SAMPLE_TIME
        void Update()
        {
            hour_Arm.rotation = Quaternion.Euler(new Vector3(0, DateTime.Now.Hour % ClockHours * EveryHourAngle, 0));
            minute_Arm.rotation = Quaternion.Euler(new Vector3(0, DateTime.Now.Minute * EveryMinuteAngle, 0));
            second_Arm.rotation = Quaternion.Euler(new Vector3(0, DateTime.Now.Second * EveryMinuteAngle, 0));
        }
#else
        void Update()
        {
            hour_Arm.rotation = Quaternion.Euler(new Vector3(0, DateTime.Now.Hour % ClockHours * EveryHourAngle, 0));
            minute_Arm.rotation = Quaternion.Euler(new Vector3(0, DateTime.Now.Minute * EveryMinuteAngle, 0));
            second_Arm.rotation = Quaternion.Euler(new Vector3(0, DateTime.Now.Second * EveryMinuteAngle, 0));
        }
#endif
    }
}