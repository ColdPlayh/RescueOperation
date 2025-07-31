using System;
using UnityEngine;
using UnityEngine.UI;


public class BuildObj : MonoBehaviour
{
    [HideInInspector]
    public  Toggle toggle;
    public GameObject buildPrefab;
    public string towerName="尚未发现";
    public string towerDestribe = "尚未发现";
    public DefenceBuildingState_So defenceBuildingStateSo;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        if(defenceBuildingStateSo)
            Debug.Log(defenceBuildingStateSo.buildCoin);
    }
}
