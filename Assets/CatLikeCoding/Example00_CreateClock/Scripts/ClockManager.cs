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
            for (int i = 0; i < ClockHours; i++)
            {
                GameObject hourSpawn = Instantiate(hourItem, transform);
                hourSpawn.transform.rotation = Quaternion.Euler(new Vector3(0f, i * EveryHourAngle, 0f));
            }
        }

#if CLOCK_SIMPLE_TIME
        //�ڶ���ʱ�ӣ�����ʱ�� ʱ��ÿ��һСʱ��һ��
        void Update()
        {
              DateTime curNow = DateTime.Now;
              hour_Arm.rotation = Quaternion.Euler(new Vector3(0f, curNow.Hour * EveryHourAngle, 0f));
              minute_Arm.rotation = Quaternion.Euler(new Vector3(0f, curNow.Minute * EveryMinuteAngle, 0f));
              second_Arm.rotation = Quaternion.Euler(new Vector3(0f, curNow.Second * EveryMinuteAngle, 0f));
        }
#else
        //��һ��ʱ�ӣ�ʱ����ǰ ƽ������
        void Update()
        {
            TimeSpan timeSpan = DateTime.Now.TimeOfDay;
            hour_Arm.rotation = Quaternion.Euler(new Vector3(0f, (float)timeSpan.TotalHours * EveryHourAngle, 0f));
            minute_Arm.rotation = Quaternion.Euler(new Vector3(0f, (float)timeSpan.TotalMinutes * EveryMinuteAngle, 0f));
            second_Arm.rotation = Quaternion.Euler(new Vector3(0f, (float)timeSpan.TotalSeconds * EveryMinuteAngle, 0f));
        }
#endif
    }
}