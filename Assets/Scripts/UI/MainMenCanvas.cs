using System;
using UnityEngine.UI;
using UnityEngine;

public class MainMenCanvas : MonoBehaviour
{
    private Button startBtn;
    private Button quitBtn;
    private Button helpBtn;
    private Image fadeImage;
    public RectTransform helpCanvas;
    private void Awake()
    {
        startBtn = transform.GetChild(0).GetComponent<Button>();
        quitBtn = transform.GetChild(1).GetComponent<Button>();
        helpBtn = transform.GetChild(2).GetComponent<Button>();
        fadeImage = transform.GetChild(4).GetComponent<Image>();
        
        startBtn.onClick.AddListener(StartBtnOnClick);
        quitBtn.onClick.AddListener(QuitBtnOnClick);
        helpBtn.onClick.AddListener(HelpBtnOnClick);
    }
    private void StartBtnOnClick()
    {
        SceneChangeManager.Instance.LoadSceneNotLodingHaveFade(Constant.SCENE_NAME_SELECTLEVEL,fadeImage);
    }
    private void QuitBtnOnClick()
    {
        Application.Quit();
    }
    private void HelpBtnOnClick()
    {
        throw new NotImplementedException();
    }
}
