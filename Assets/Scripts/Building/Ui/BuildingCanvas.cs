using System;
using UnityEngine;
using UnityEngine.UI;
public class BuildingCanvas : MonoBehaviour
{
   public ToggleGroup toggleGroup;
   private Toggle[] toggles;
   private GameObject currBuild;

   private Transform buildTrans;
   
   private Button backBtn;
   private Button buildBtn;
   private Text towerNameText;
   private Text towerDescribeText;
   private int currExpenses=100;
   private bool isInit = false;
   private DefenceBuildingState towerState;
   private void Awake()
   {
      toggles = toggleGroup.transform.GetComponentsInChildren<Toggle>();
      foreach(Toggle toggle in toggles)
      {
         toggle.onValueChanged.AddListener((bool isOn) => OnToggleClick(isOn,toggle));
      }

      buildBtn = transform.GetChild(0).GetChild(3).GetComponent<Button>();
      backBtn = transform.GetChild(0).GetChild(4).GetComponent<Button>();

      towerNameText = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
      towerDescribeText = transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>();
      
      backBtn.onClick.AddListener(HideCanvas);
      
      buildBtn.onClick.AddListener(BuildTower);
      
      buildBtn.transform.GetChild(0).GetComponent<Text>().text = currExpenses.ToString();
      
      if (currExpenses > GameModel.Instance.CurrCoin)
      {
         buildBtn.enabled = false;
         buildBtn.transform.GetChild(0).GetComponent<Text>().text="金币不足";
      }
   }
   private void OnToggleClick(bool isOn,Toggle toggle)
   {
      Debug.Log(toggle+":"+isOn);
      if (!isOn)
         return;
      Debug.Log("123");
      BuildObj build = toggle.GetComponent<BuildObj>();
      towerNameText.text = build.towerName;
      towerDescribeText.text = build.towerDestribe;
      buildBtn.transform.GetChild(0).GetComponent<Text>().text = currExpenses.ToString();
      if (build.buildPrefab)
      {
         currBuild = build.buildPrefab;
         currExpenses = build.defenceBuildingStateSo.buildCoin;
         // towerState = build.towerState;
         //currExpenses = towerState.BuildCoin;
      }
      else
      {
         currBuild = null;
      }

      buildBtn.transform.GetChild(0).GetComponent<Text>().text = currExpenses.ToString();
      
      if (currExpenses > GameModel.Instance.CurrCoin)
      {
         buildBtn.enabled = false;
         buildBtn.transform.GetChild(0).GetComponent<Text>().text="金币不足";
      }
      else
      {
         buildBtn.enabled = true;
      }
      
   }

   public void setTrans(Transform trans)
   {
      buildTrans = trans;
   }
   private void HideCanvas()
   {
      Destroy(gameObject);
   }

   private void BuildTower()
   {
      GameModel.Instance.CurrCoin -= currExpenses;
      if (currExpenses > GameModel.Instance.CurrCoin)
      {
         buildBtn.enabled = false;
         buildBtn.transform.GetChild(0).GetComponent<Text>().text="金币不足";
      }

      if (currBuild)
      {
         Instantiate(currBuild, buildTrans.position, buildTrans.rotation);
      }
       
      
      
      Destroy(gameObject);
   }
}
