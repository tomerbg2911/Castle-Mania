using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 1f;

    public Slider healthSlider;
    public Slider damageSlider;
    public Image damageImage;
    public Image healthImage;
    public Color damageColor;
    private float damageFadeTimer;

    private void Awake()
    {
        damageImage = damageSlider.gameObject.GetComponentInChildren<Image>();
        healthImage = healthSlider.gameObject.GetComponentInChildren<Image>();
        damageColor = damageImage.color;
    }

    private void Update()
    {
        //if (damageColor.a > 0)
        //{
        //    damageFadeTimer -= Time.deltaTime;
        //    if (damageFadeTimer < 0)
        //    {
        //        float fadeAmount = 3f;
        //        damageColor.a -= fadeAmount * Time.deltaTime;
        //        damageImage.color = damageColor;
        //    }
        //}

    }

    public void SetMana(int health)
    {

        if (damageColor.a <= 0)
        {
            damageSlider.value = healthSlider.value;
        }
        damageColor.a = 1;
        damageImage.color = damageColor;
        damageFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;

        healthSlider.value = health;
    }
}