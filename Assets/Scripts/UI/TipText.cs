using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipText : MonoBehaviour
{
    private Text tipText;
    private Animator anim;

    private void Awake()
    {
        tipText = GetComponent<Text>();
        anim = GetComponent<Animator>();
    }

    public void Show(string tip,Color color)
    {
        tipText.text= tip;
        tipText.color = color;
        anim.SetTrigger("Tip");
        Destroy(gameObject,1.5f);
    }
}
