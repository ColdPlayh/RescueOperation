using System;
using UnityEditor;
using UnityEngine;


public class DefenceBuildingState : MonoBehaviour
{
    private DefenceBuildingState_So defenceBuildingState;
    public DefenceBuildingState_So templateState;
    public float HealthMultplier = 2f;
    public float AttackMultPlier = 1.5f;
    private void Awake()
    {
        if (templateState != null)
        {
            defenceBuildingState = Instantiate(templateState);
        }

        defenceBuildingState.currHealth = defenceBuildingState.maxHealth;
        defenceBuildingState.currAttack = defenceBuildingState.baseAttack;
        defenceBuildingState.currLevel = 1;
    }

    public string TowerName
    {
        get => defenceBuildingState.towerName;
    }
    public int CurrHealth
    {
        get => defenceBuildingState.currHealth;
        set
        {
            if(!value.Equals(defenceBuildingState.currHealth))
            {
                //扣血最多到0
                if (value <= 0)
                {
                    OnHealthChanged?.Invoke(defenceBuildingState.currHealth,defenceBuildingState.maxHealth,
                        -defenceBuildingState.currHealth);
                    defenceBuildingState.currHealth = 0;
                }
                
                else if (value > defenceBuildingState.currHealth)
                {
                    //加血最多到最大生命值
                    if (value > defenceBuildingState.maxHealth)
                    {
                        OnHealthChanged?.Invoke(defenceBuildingState.currHealth,defenceBuildingState.maxHealth,
                            defenceBuildingState.maxHealth-defenceBuildingState.currHealth);
                        Debug.Log("subvalue1"+(defenceBuildingState.maxHealth-defenceBuildingState.currHealth));

                        defenceBuildingState.currHealth = defenceBuildingState.maxHealth;
                    }
                    else
                    {
                        OnHealthChanged?.Invoke(defenceBuildingState.currHealth,defenceBuildingState.maxHealth,
                            value-defenceBuildingState.currHealth);
                        Debug.Log("subvalue2"+(value-defenceBuildingState.currHealth));
                        defenceBuildingState.currHealth = value;
                    }
                    
                }
                //设置最新的生命值
                else
                {
                    OnHealthChanged?.Invoke(defenceBuildingState.currHealth,defenceBuildingState.maxHealth,
                        value-defenceBuildingState.currHealth);
                    defenceBuildingState.currHealth = value;
                }
            }
        }
    }

    public int MaxHealth
    {
        get => defenceBuildingState.maxHealth;
        set
        {
            defenceBuildingState.maxHealth = value;
        }
    }

    public int CurrLevel
    {
        get => defenceBuildingState.currLevel;
        set { defenceBuildingState.currLevel=value; }
    }

    public int MaxLevel
    {
        get => defenceBuildingState.maxLevel;
    }

    public int BaseAttack
    {
        get => defenceBuildingState.baseAttack;
    }

    public int CurrAttack
    {
        get => defenceBuildingState.currAttack;
        set { defenceBuildingState.currAttack = value; }
    }

    public int BuildCoin
    {
        get => defenceBuildingState.buildCoin;
    }
    public void levelUp()
    {
        defenceBuildingState.currAttack =(int)(CurrAttack* AttackMultPlier);
        defenceBuildingState.maxHealth = (int)(MaxHealth * HealthMultplier);
        defenceBuildingState.currHealth =(int)(CurrHealth* HealthMultplier);
        CurrLevel += 1;
    }
    public Action<int,int,int> OnHealthChanged;
}