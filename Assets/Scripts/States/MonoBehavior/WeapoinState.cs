using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeapoinState : MonoBehaviour
{
    public WeaponState_So weaponState_So;
    public int Magazines { get { return weaponState_So.magazines; } }
    public int SpareAmmunition { get { return weaponState_So.spareAmmunition; } }
    public int BaseAttack { get { return weaponState_So.baseAttack; } }
    
    public int CurrMagazines
    {
        get { return weaponState_So.currMagazines; }
        set { weaponState_So.currMagazines = value;  }
    }
    public int CurrAmmunition
    {
        get { return weaponState_So.currAmmunition; }
        set { weaponState_So.currAmmunition = value; }
    }
    public int CurrAttack
    {
        get { return weaponState_So.currAttack; }
        set { weaponState_So.currAttack = value; }
    }

}
