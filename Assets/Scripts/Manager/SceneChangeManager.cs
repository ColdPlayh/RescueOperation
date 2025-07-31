using System.Collections;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeManager:SingleTon<SceneChangeManager>
{
    [HideInInspector]
    public string LevelOne="LevelOne";
    [HideInInspector]
    public string MainMenu = "MainMenu";
    public Canvas LoadCanvas;
    public float loadSpeed=0.5f;
    public CheckPointState_So level1;
    public CheckPointState_So level2;
    public CheckPointState_So level3;
    
    public bool isDontDestroyOnLoad=false;
    private Slider loadSlider;
    private Text loadText;
    private Text loadingValue;
    private Image currFadeImage;
    protected override void Awake()
    {
        base.Awake();
        if (isDontDestroyOnLoad)
        {
            DontDestroyOnLoad(this);
        }
        
    }

    public void LoadSceneHaveLoadingNotFade(string sceneNmae)
    {
        StartCoroutine(HaveLodingNotFade(sceneNmae));
    }
    IEnumerator HaveLodingNotFade(string sceneName)
    {

        var canvas=Instantiate(LoadCanvas);
        loadSlider = canvas.transform.GetChild(0).GetChild(1).GetComponent<Slider>();
        loadingValue = canvas.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>();
        loadText = canvas.transform.GetChild(0).GetChild(3).GetComponent<Text>();
        loadText.gameObject.SetActive(false);

        var operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            loadSlider.value += 0.01f * loadSpeed;
            loadingValue.text = loadSlider.value * 100 + "%";
            if (loadSlider.value >= 0.99f)
            {
                loadText.gameObject.SetActive(true);
                if (Input.touchCount > 0 || Input.anyKeyDown) operation.allowSceneActivation = true;
            }

            yield return null;
        }

        operation.allowSceneActivation = true;
        yield break;
    }

    public void LoadSceneHaveLoadingHaveFade(string sceneName)
    {
        StartCoroutine(HaveLoadingHaveFade(sceneName));
    }

    IEnumerator HaveLoadingHaveFade(string sceneName)
    {
        var canvas=Instantiate(LoadCanvas);
        loadSlider = canvas.transform.GetChild(0).GetChild(1).GetComponent<Slider>();
        loadingValue = canvas.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>();
        loadText = canvas.transform.GetChild(0).GetChild(3).GetComponent<Text>();
        loadText.gameObject.SetActive(false);
        
        
        
        var operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            loadSlider.value += 0.01f * loadSpeed;
            loadingValue.text = loadSlider.value * 100 + "%";
            if (loadSlider.value >= 0.99f)
            {
                loadSlider.value = 1;
                loadText.gameObject.SetActive(true);
                if (Input.touchCount > 0 || Input.anyKeyDown)
                {
                    currFadeImage=canvas.transform.GetChild(0).GetChild(4).GetComponent<Image>();
                    currFadeImage.GetComponent<Animator>().SetBool("FadeOut",true);
                    yield return new WaitForSeconds(1.75f);
                    operation.allowSceneActivation = true;

                }
                
               
                
            }

            yield return null;
        }

        operation.allowSceneActivation = true;
        yield break;
    }
    public void LoadSceneNotLodingHaveFade(string sceneName,Image fade)
    {
        StartCoroutine(NotLodingHaveFade(sceneName,fade));
    }

    IEnumerator NotLodingHaveFade(string sceneName,Image fade)
    {
        currFadeImage = fade;
        currFadeImage.GetComponent<Animator>().SetBool("FadeIn",false);
        currFadeImage.GetComponent<Animator>().SetBool("FadeOut",true);
        
        var operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1.75f);
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
        yield break;
    }
}