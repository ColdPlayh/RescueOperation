using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameModel : SingleTon<GameModel>
{
    private int currCoin=1000;
    private int currScore=0;
    private int currFall=10;
    private int maxFall=10;
    private string currTime="0:00";
    private int currEnemy=0;
    private int currTower=0;

    public Action<int> OnCurrCoinChanged;
    public Action<int> OnCurrScoreChanged;
    public Action<int> OnCurrFallChanged;
    public Action<int> OnMaxFallChanged;
    public Action<string> OnCurrTimeChanged;
    public Action<int> OnCurrEnemyChanged;
    public Action<int> OnCurrTowerChanged;
    

    public int CurrCoin
    {
        get => currCoin;
        set
        {
            
            if (value != currCoin)
            {
                currCoin = value;
                OnCurrCoinChanged?.Invoke(value);
            }
        }
    }

    public int CurrScore
    {
        get => currScore;
        set
        {
            if (value != currScore)
            {
                currScore = value;
                OnCurrScoreChanged?.Invoke(value);
            }
        }
    }

    public int CurrFall
    {
        get => currFall;
        set
        {
            if (value != currFall)
            {
                currFall = value;
                OnCurrFallChanged?.Invoke(value);
            }
        }
    }

    public int MAXFall
    {
        get => maxFall;
        set
        {
            if (value != maxFall)
            {
                maxFall = value;
                OnMaxFallChanged?.Invoke(value);
            }
        }
    }

    public string CurrTime
    {
        get => currTime;
        set
        {
            if (!value.Equals(currTime))
            {
                currTime = value;
                OnCurrTimeChanged?.Invoke(value);
            }
        }
    }

    public int CurrEnemy
    {
        get => currEnemy;
        set
        {
            if (value != currEnemy)
            {
                currEnemy = value;
                OnCurrEnemyChanged?.Invoke(value);
            }
        }
    }

    public int CurrTower
    {
        get => currTower;
        set
        {
            if (value != currTower)
            {
                currTower = value;
                OnCurrTowerChanged?.Invoke(value);
            }
        }
    }
}
