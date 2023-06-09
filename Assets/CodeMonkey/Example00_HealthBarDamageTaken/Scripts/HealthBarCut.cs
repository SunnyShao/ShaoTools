using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey
{
    /// <summary>
    /// 一段一段掉落的血条
    /// </summary>
    public class HealthBarCut : MonoBehaviour
    {
        private const float BAR_WIDTH = 500f;  //血条总宽度上限

        private Image barImage;
        private Transform damagedBarTemplate;
        private Button addButton;
        private Button subtractionButton;

        private HealthSystem healthSystem;

        private void Awake()
        {
            barImage = transform.Find("bar").GetComponent<Image>();
            damagedBarTemplate = transform.Find("damagedBarTemplate");

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

        private void HealthSystem_OnHealed(object sender, EventArgs e)
        {
            SetHealth(healthSystem.GetHealthNormalized());
        }

        private void HealthSystem_OnDamaged(object sender, EventArgs e)
        {
            float beforeHealth = barImage.fillAmount;
            SetHealth(healthSystem.GetHealthNormalized());

            Transform damagedBar = Instantiate(damagedBarTemplate, transform);
            damagedBar.gameObject.SetActive(true);
            damagedBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(barImage.fillAmount * BAR_WIDTH, damagedBar.GetComponent<RectTransform>().anchoredPosition.y);
            damagedBar.GetComponent<Image>().fillAmount = beforeHealth - barImage.fillAmount;
            damagedBar.gameObject.AddComponent<HealthBarCutFallDown>();
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
            healthSystem.OnDamaged -= HealthSystem_OnDamaged;
            healthSystem.OnHealed -= HealthSystem_OnHealed;
            healthSystem = null;

            addButton.onClick.RemoveAllListeners();
            subtractionButton.onClick.RemoveAllListeners();
        }
    }
}