using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private DefenceBuildingState defenceBuildingState;

    private void Awake()
    {
        defenceBuildingState = GetComponent<DefenceBuildingState>();
        
    }

    private void OnEnable()
    {
        defenceBuildingState.OnHealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        defenceBuildingState.OnHealthChanged -= OnHealthChanged;
    }

    private void Start()
    {
        GameModel.Instance.CurrTower += 1;
    }

    private void OnDestroy()
    {
        GameModel.Instance.CurrTower -= 1;
    }

    public void BuckleBoold(int damage)
    {
        defenceBuildingState.CurrHealth -= damage;
    }

    public void OnHealthChanged(int currHealth,int maxHealth,int sub)
    {
        if (currHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
