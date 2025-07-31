using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelUpCanvas : MonoBehaviour
{
    [Header("Base Setting")] public List<Texture> textures = new List<Texture>();


    private RawImage towerImage;
    private Text towerNameText;
    private Text towerLevelText;
    private Text towerCurrHealthText;
    private Text towerMaxHealthText;
    private Text towerAttackText;

    private Button backBtn;
    private Button upGradeBtn;

    private int currExpenses=100;

    private DefenceBuildingState towerState;

    private void Awake()
    {
        towerImage = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<RawImage>();
        Debug.Log(towerImage);
        towerNameText = transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>();
        towerLevelText=transform.GetChild(0).GetChild(2).GetChild(1).GetChild(1).GetComponent<Text>();
        towerCurrHealthText=transform.GetChild(0).GetChild(2).GetChild(1).GetChild(2).GetComponent<Text>();
        towerMaxHealthText=transform.GetChild(0).GetChild(2).GetChild(1).GetChild(3).GetComponent<Text>();
        towerAttackText=transform.GetChild(0).GetChild(2).GetChild(1).GetChild(4).GetComponent<Text>();

        backBtn = transform.GetChild(0).GetChild(3).GetComponent<Button>();
        upGradeBtn = transform.GetChild(0).GetChild(4).GetComponent<Button>();
        
        backBtn.onClick.AddListener(backBtnOnClick);
        upGradeBtn.onClick.AddListener(UpGradeBtnOnClick);



    }
    public void Init(DefenceBuildingState input)
    {

        towerState = input;
        
        towerImage.texture = getTexture(input.TowerName);
        
        towerNameText.text = input.TowerName;
        towerLevelText.text = input.CurrLevel.ToString();
        towerCurrHealthText.text = input.CurrHealth.ToString();
        towerMaxHealthText.text = input.MaxHealth.ToString();
        towerAttackText.text = input.CurrAttack.ToString();
        switch (input.CurrLevel)
        {
            case 1:
                upGradeBtn.transform.GetChild(0).GetComponent<Text>().text = "100";
                currExpenses = 100;
                break;
            case 2:
                upGradeBtn.transform.GetChild(0).GetComponent<Text>().text = "300";
                currExpenses = 300;
                break;
            case 3:
                upGradeBtn.transform.GetChild(0).GetComponent<Text>().text = "最高等级";
                upGradeBtn.enabled = false;
                break;
        }

        if (currExpenses > GameModel.Instance.CurrCoin)
        {
            upGradeBtn.enabled = false;
            upGradeBtn.transform.GetChild(0).GetComponent<Text>().text="金币不足";
        }
        
    }

    public Texture getTexture(string name)
    {
        if (name.Equals(Constant.TOWER_NAME_STORAGE))
        {
            return textures[0];
        }
        else if (name.Equals(Constant.TOWER_NAME_GATING))
        {
            return textures[1];
        }
        else if (name.Equals(Constant.TOWER_NAME_LIGHTING))
        {
            return textures[2];
        }
        else if (name.Equals(Constant.TOWER_NAME_MACHINE))
        {
            return textures[3];
        }
        else if (name.Equals(Constant.TOWER_NAME_OBSTACLE))
        {
            return textures[4];
        }
        else if (name.Equals(Constant.TOWER_NAME_RECOVER))
        {
            return textures[5];
        }
        else if (name.Equals(Constant.TOWER_NAME_VOLLEY))
        {
            return textures[6];
        }

        return null;
    }
    
    public void backBtnOnClick()
    {
        Destroy(gameObject);
    }

    public void UpGradeBtnOnClick()
    {
        
        GameModel.Instance.CurrCoin -= currExpenses;
        towerState.levelUp();
        Init(towerState);
        Debug.Log("currcost"+currExpenses+":"+GameModel.Instance.CurrCoin+":"+(currExpenses > GameModel.Instance.CurrCoin && towerState.CurrLevel<3));
        if (currExpenses > GameModel.Instance.CurrCoin && towerState.CurrLevel<3)
        {
            upGradeBtn.enabled = false;
            upGradeBtn.transform.GetChild(0).GetComponent<Text>().text="金币不足";
        }
       
    }
}
