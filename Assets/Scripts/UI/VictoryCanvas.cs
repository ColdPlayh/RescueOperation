using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryCanvas : MonoBehaviour
{
   private Button reStartBtn;
   private Button nextLevelBtn;
   private Button backMainMenuBtn;
   private Image fade;
   public PopupCanvas popupCanvas;

   private void Awake()
   {
      reStartBtn = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>();
      nextLevelBtn= transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<Button>();
      backMainMenuBtn=transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetComponent<Button>();
      fade = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(4).GetComponent<Image>();
      
      reStartBtn.onClick.AddListener(ReStart);
      backMainMenuBtn.onClick.AddListener(BackMainMenuBtnOnClick);
      nextLevelBtn.onClick.AddListener(nextLevelBtnOnClick);
      SceneChangeManager.Instance.level2.isOpen= true;

   }

   public void ReStart()
   {
      StartCoroutine(ShowPopup("是否重新游玩本关？",SceneManager.GetActiveScene().name));
   }

   public void nextLevelBtnOnClick()
   {
      if (!SceneManager.GetActiveScene().Equals("Level2"))
      {
         SceneChangeManager.Instance.LoadSceneHaveLoadingHaveFade("Level2");
      }
      else
      {
         UIManager.Instance.TipText("敬请期待",Color.red);
      }
     
   }

   public void BackMainMenuBtnOnClick()
   {
      StartCoroutine(ShowPopup("确定返回主菜单吗？",Constant.SCENE_NAME_MAINMAENU));
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
