using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCanvas : MonoBehaviour
{
    private Button cancleBtn;
    private Button okBtn;
    private Button closeBtn;
    private Text contentText;
    [HideInInspector] public bool isOk=false;
    
    private void Awake()
    {
        okBtn = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>();
        cancleBtn= transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Button>();
        closeBtn= transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Button>();
        
        contentText= transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<Text>();
        
        okBtn.onClick.AddListener(okBtnOnClick);
        cancleBtn.onClick.AddListener(CancleBtnOnClick);
        closeBtn.onClick.AddListener(CloseBtnOnClick);
    }
    public void okBtnOnClick()
    {
        isOk = true;
    }

    public void CancleBtnOnClick()
    {
        gameObject.SetActive(false);
    }

    public void CloseBtnOnClick()
    {
        gameObject.SetActive(false);
    }

    public void setText(string content)
    {
        contentText.text = content;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }


}


