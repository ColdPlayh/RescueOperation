using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New WeaponState", menuName = "Weapon State/Data")]
public class WeaponState_So : ScriptableObject
{
    //子弹的力
    public float bulletForce;
    //弹夹容量
    public int magazines;
    //默认备弹
    public int spareAmmunition;
    //当前弹夹弹药
    public int currMagazines;
    //当前弹药
    public int currAmmunition;
    //基础伤害
    public int baseAttack;
    //当前伤害
    public int currAttack;
}
