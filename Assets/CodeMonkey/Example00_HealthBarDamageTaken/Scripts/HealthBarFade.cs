using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarFade : MonoBehaviour
{
    private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 1f;  //渐变计时器上限

    private Image barImage;
    private Image damagedBarImage;
    private Button addButton;
    private Button subtractionButton;

    // 缓存颜色
    private Color damagedColor;

    //渐变时间 当计时器小于0时，降低alpha值
    private float damagedHealthFadeTimer;

    private HealthSystem healthSystem;

    private void Awake()
    {
        barImage = transform.Find("bar").GetComponent<Image>();
        damagedBarImage = transform.Find("damagedBar").GetComponent<Image>();

        addButton = transform.Find("AddButton").GetComponent<Button>();
        addButton.onClick.AddListener(OnAddButtonClick);
        subtractionButton = transform.Find("SubtractionButton").GetComponent<Button>();
        subtractionButton.onClick.AddListener(OnSubtarctionBtnClick);

        damagedColor = damagedBarImage.color;
        damagedColor.a = 0f;
        damagedBarImage.color = damagedColor;
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
        // 当受伤进度条显示出来时
        if (damagedColor.a > 0f)
        {
            // 受伤进度条显示指定的时间后 受伤进度条渐隐
            damagedHealthFadeTimer -= Time.deltaTime;
            if(damagedHealthFadeTimer < 0)
            {
                damagedColor.a -= 5f * Time.deltaTime;
                damagedBarImage.color = damagedColor;
            }
        }
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e)
    {
        SetHealth(healthSystem.GetHealthNormalized());
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        if (damagedColor.a <= 0f)
        {
            //Damage bar Image is invisible
            damagedBarImage.fillAmount = barImage.fillAmount;
        }

        damagedColor.a = 1;
        damagedBarImage.color = damagedColor;
        damagedHealthFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;

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