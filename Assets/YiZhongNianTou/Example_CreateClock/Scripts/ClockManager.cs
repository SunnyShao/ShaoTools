using System;
using UnityEngine;

namespace Example_CreateClock
{
    public class ClockManager : MonoBehaviour
    {
        private const int ClockHours = 12;      //时钟上有12个小时显示
        private const int EveryHourAngle = 30;  //每两个时针角度间隔
        private const int EveryMinuteAngle = 6;

        //12个小时底座
        private GameObject hourItem;

        //时针 分针 秒针
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
        //第二种时钟，简易时针 时针每隔一小时跳一下
        void Update()
        {
              DateTime curNow = DateTime.Now;
              hour_Arm.rotation = Quaternion.Euler(new Vector3(0f, curNow.Hour * EveryHourAngle, 0f));
              minute_Arm.rotation = Quaternion.Euler(new Vector3(0f, curNow.Minute * EveryMinuteAngle, 0f));
              second_Arm.rotation = Quaternion.Euler(new Vector3(0f, curNow.Second * EveryMinuteAngle, 0f));
        }
#else
        //第一种时钟，时刻向前 平滑过渡
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