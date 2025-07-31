using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DefenceBuildingState",menuName = "DefenceBuildingState/Data")]
public class DefenceBuildingState_So : ScriptableObject
{
    public string towerName;
    public int currHealth;
    public int maxHealth;
    public int currLevel;
    public int maxLevel;
    public int currAttack;
    public int baseAttack;
    public int buildCoin;
}
