using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey
{
    /// <summary>
    /// 血条渐渐的消失
    /// </summary>
    public class HealthBarShrink : MonoBehaviour
    {
        private const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = 1f;  //渐变计时器上限

        private Image barImage;
        private Image damagedBarImage;
        private Button addButton;
        private Button subtractionButton;

        //渐变时间 当计时器小于0时，降低alpha值
        private float damagedHealthShrinkTimer;

        private HealthSystem healthSystem;

        private void Awake()
        {
            barImage = transform.Find("bar").GetComponent<Image>();
            damagedBarImage = transform.Find("damagedBar").GetComponent<Image>();
            damagedBarImage.fillAmount = 1f;

            addButton = transform.Find("AddButton").GetComponent<Button>();
            addButton.onClick.AddListener(OnAddButtonClick);
            subtractionButton = transform.Find("SubtractionButton").GetComponent<Button>();
            subtractionButton.onClick.AddListener(OnSubtarctionBtnClick);
        }

        private void Start()
        {
            healthSystem = new HealthSystem(100);
            SetHealth(healthSystem.GetHealthNormalized());
            healthSystem.OnDamaged += HealthSystem_OnDamaged;
            healthSystem.OnHealed += HealthSystem_OnHealed;
        }

        private void Update()
        {
            damagedHealthShrinkTimer -= Time.deltaTime;
            if (damagedHealthShrinkTimer < 0)
            {
                if (barImage.fillAmount < damagedBarImage.fillAmount)
                {
                    damagedBarImage.fillAmount -= 1f * Time.deltaTime;
                }
            }
        }

        private void HealthSystem_OnHealed(object sender, EventArgs e)
        {
            damagedBarImage.fillAmount = healthSystem.GetHealthNormalized();

            SetHealth(healthSystem.GetHealthNormalized());
        }

        private void HealthSystem_OnDamaged(object sender, EventArgs e)
        {
            damagedHealthShrinkTimer = DAMAGED_HEALTH_SHRINK_TIMER_MAX;

            SetHealth(healthSystem.GetHealthNormalized());
        }

        private void SetHealth(float health)
        {
            barImage.fillAmount = health;
        }

        private void OnSubtarctionBtnClick()
        {
            healthSystem.Damage(10);
        }

        private void OnAddButtonClick()
        {
            healthSystem.Heal(10);
        }

        private void OnDestroy()
        {
            Debug.Log(healthSystem == null);
            healthSystem.OnDamaged -= HealthSystem_OnDamaged;
            healthSystem.OnHealed -= HealthSystem_OnHealed;
            healthSystem = null;

            addButton.onClick.RemoveAllListeners();
            subtractionButton.onClick.RemoveAllListeners();
        }
    }
}