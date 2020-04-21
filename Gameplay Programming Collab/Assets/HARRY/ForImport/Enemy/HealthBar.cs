using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        
        if(health == 3)
        {
            fill.color = gradient.Evaluate(1f);
        }

        if (health == 2)
        {
            fill.color = gradient.Evaluate(0.5f);
        }

        if (health == 1)
        {
            fill.color = gradient.Evaluate(0.33f);
        }
    }
}
