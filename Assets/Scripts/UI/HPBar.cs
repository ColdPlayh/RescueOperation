using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image Hp;
    public Slider slider;
    public Gradient gradient;
    

    public void SetHp(int currHp)
    {
        slider.value = currHp;
        Hp.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxHp(int maxHp)
    {
        slider.maxValue = maxHp;
        Hp.color = gradient.Evaluate(slider.value);
    }
}
