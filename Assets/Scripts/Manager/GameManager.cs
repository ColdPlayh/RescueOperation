using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using States.MonoBehavior;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    private CharacterState characterState;
    private List<IGameOver> enmeyList=new List<IGameOver>();
    public PlayerControl player;
    public bool gameOver = false;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public CharacterState CharacterState
    {
        get => characterState;
    }
    public void RegisterPlayer(PlayerControl input)
    {
        this.player = input;
    }

    public void RegisterCharacterState(CharacterState input)
    {
        this.characterState = input;
    }

    public void RegisterEnemy(IGameOver register)
    {
        if(!enmeyList.Contains(register))
            enmeyList.Add(register);
    }

    public void UnRegisterEnemy(IGameOver unregister)
    {
        if(enmeyList.Contains(unregister))
            enmeyList.Remove(unregister);
    }

    public bool isCotainIGameOver(IGameOver input)
    {
        if (enmeyList.Contains(input))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void GameOver()
    {
        if (!gameOver)
        {
            GameOverBroadCast();
            UIManager.Instance.GameOver();
            gameOver = true;
        }
    }

    public void GameOverBroadCast()
    {
        foreach (var enemy in enmeyList)
        {
            enemy.OverExecute();
        }
    }
}
