using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarFade : MonoBehaviour
{
    private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 1f;  

    private Image barImage;
    private Image damagedBarImage;
    private Color damagedColor;

    //Ω•±‰ ±º‰
    private float damagedHealthFadeTimer;

    private HealthSystem healthSystem;

    private void Awake()
    {
        barImage = transform.Find("bar").GetComponent<Image>();
        damagedBarImage = transform.Find("damagedBar").GetComponent<Image>();
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

    private void HealthSystem_OnHealed(object sender, EventArgs e)
    {
        SetHealth(healthSystem.GetHealthNormalized());
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        SetHealth(healthSystem.GetHealthNormalized());
    }

    private void SetHealth(float health)
    {
        barImage.fillAmount = health;
    }
}