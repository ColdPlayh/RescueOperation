using UnityEngine;
using UnityEngine.UI;

public class CheckPointState : MonoBehaviour
{
   public CheckPointState_So checkPointStateSo;

   public int levelNum
   {
      get => checkPointStateSo.levelNum;
   }
   public string levelName
   {
      get => checkPointStateSo.levelName;
   }
   public string levelInformation
   {
      get => checkPointStateSo.levelInformation;
   }
   public bool IsOpen
   {
      get => checkPointStateSo.isOpen;
      set
      {
         checkPointStateSo.isOpen = value;
      }
   }
   public bool IsNotHave
   {
      get => checkPointStateSo.isNotHave;
   }
   public string SceneName
   {
      get => checkPointStateSo.sceneName;
   }
}
