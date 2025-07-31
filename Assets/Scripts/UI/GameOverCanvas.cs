using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    private Button reStartBtn;
    private Button backMainMenuBtn;
    private Image fade;
    public PopupCanvas popupCanvas;

    private void Awake()
    {
        reStartBtn=transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>();
        backMainMenuBtn=transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<Button>();
        fade = transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Image>();
        reStartBtn.onClick.AddListener(ReStart);
        backMainMenuBtn.onClick.AddListener(BackMainMenu);
    }

    public void ReStart()
    {
        Debug.Log("restart is pressed");
        GameManager.Instance.gameOver = false;
        SceneChangeManager.Instance.LoadSceneHaveLoadingHaveFade(SceneManager.GetActiveScene().name);
    }
    public void BackMainMenu()
    {
        GameManager.Instance.gameOver = false;
        StartCoroutine(ShowPopup("确定返回主菜单？", Constant.SCENE_NAME_MAINMAENU));

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
                Destroy(popup.gameObject);
                SceneChangeManager.Instance.LoadSceneNotLodingHaveFade(sceneName,fade);
                yield break;
            }
            yield return null;
        }
        Destroy(popup.gameObject);
    }
}
