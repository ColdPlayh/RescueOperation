using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelCanvas : MonoBehaviour
{
    public RollingUI rollingCanvas;
    private Button previousBtn;
    private Button nextBtn;
    private Button enterLevelBtn;
    private Button backBtn;
    public TipText tipText;
    public Image fade;
    private void Awake()
    {
        previousBtn = transform.GetChild(0).GetChild(0).GetComponent<Button>();
        nextBtn = transform.GetChild(0).GetChild(1).GetComponent<Button>();
        enterLevelBtn = transform.GetChild(0).GetChild(2).GetComponent<Button>();
        backBtn = transform.GetChild(0).GetChild(3).GetComponent<Button>();

        previousBtn.onClick.AddListener(rollingCanvas.LeftBtnOnClick);
        nextBtn.onClick.AddListener(rollingCanvas.RightBtnOnClick);
        enterLevelBtn.onClick.AddListener(EnterLevelBtnOnClick);
        backBtn.onClick.AddListener(BackBtnOnClick);
        
    }

    private void BackBtnOnClick()
    {
        SceneChangeManager.Instance.LoadSceneNotLodingHaveFade("MainMenu",fade);
    }

    public void EnterLevelBtnOnClick()
    {
        if (rollingCanvas.currRect.TryGetComponent<CheckPointState>(out CheckPointState checkPointState))
        {
            if (checkPointState.IsOpen)
            {
                SceneChangeManager.Instance.LoadSceneHaveLoadingHaveFade(
                    checkPointState.SceneName);
            }
            else
            {
                Debug.Log("enter 尚未解锁");
                var tip= Instantiate(tipText,transform);
                tip.Show("尚未解锁",Color.white);
            }
        }
        else
        {
            Debug.Log("enter 敬请期待");
            var tip = Instantiate(tipText,transform);
            tip.Show("敬请期待",Color.red);
        }


    }
}
