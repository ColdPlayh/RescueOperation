using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashEffectScrpit : SingleTon<FlashEffectScrpit>
{
    private Image whiteImage;
    private Image panelImage;
    private Animator whiteAnim;
    private Animator panelAnim;
    private int height;
    private int width;

    private void Start()
    {
        whiteImage = transform.GetChild(1).GetComponent<Image>();
        panelImage = transform.GetChild(0).GetComponent<Image>();

        whiteAnim = whiteImage.gameObject.GetComponent<Animator>();
        panelAnim = panelImage.gameObject.GetComponent<Animator>();
        width = Screen.width;
        height = Screen.height;
    }

    public void GoBlind()
    {
        StartCoroutine(Blind());
    }

    public IEnumerator Blind()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("进入bind");
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0,0,width,height),0,0);
        tex.Apply();
        panelImage.sprite = Sprite.Create(tex,new Rect(0,0,width,height),new Vector2(0.5f,0.5f),100);
        whiteAnim.SetTrigger("GoBlind");
        panelAnim.SetTrigger("GoBlind");
    }
}
