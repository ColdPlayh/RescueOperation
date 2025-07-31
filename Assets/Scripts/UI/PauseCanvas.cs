using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{
    private Button continueBtn;
    private Button reSelectLevelBtn;
    private Button settingBtn;
    private Button backMainMenuBtn;

    private Image fade;

    public PopupCanvas popupCanvas;

    private void Awake()
    {
        continueBtn = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>();
        reSelectLevelBtn=transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>();
        settingBtn=transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Button>();
        backMainMenuBtn=transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Button>();
        fade = transform.GetChild(0).GetChild(0).GetChild(7).GetComponent<Image>();
        
        continueBtn.onClick.AddListener(ContinueBtnOnClick);
        reSelectLevelBtn.onClick.AddListener(ReSelectLevelBtnOnClick);
        settingBtn.onClick.AddListener(SettingBtnOnClick);
        backMainMenuBtn.onClick.AddListener(BackMainMenuBtnOnClick);
        
    }

    private void Start()
    {
        Time.timeScale = 0;
    }

    public void ContinueBtnOnClick()
    {
        Destroy(gameObject);
    }

    public void ReSelectLevelBtnOnClick()
    {
        
        StartCoroutine(ShowPopup("您确定要返回选关界面吗？",Constant.SCENE_NAME_SELECTLEVEL));
    }

    IEnumerator ShowPopup(string content,string sceneName)
    {
        PopupCanvas popup=Instantiate(popupCanvas);
        popup.setText(content);
        popup.Show();
        while (popup.isActiveAndEnabled && popup!=null)
        {
            if (popup.isOk)
            {
                Time.timeScale = 1f;
                Destroy(popup.gameObject);
                SceneChangeManager.Instance.LoadSceneNotLodingHaveFade(sceneName,fade);
                yield break;
            }
            yield return null;
        }
        Destroy(popup.gameObject);
    }
    public void SettingBtnOnClick()
    {
        
    }

    public void BackMainMenuBtnOnClick()
    {
        StartCoroutine(ShowPopup("您确定要返回主菜单吗？",Constant.SCENE_NAME_MAINMAENU));
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }
}
