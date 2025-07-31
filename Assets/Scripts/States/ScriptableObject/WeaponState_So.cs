using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New WeaponState", menuName = "Weapon State/Data")]
public class WeaponState_So : ScriptableObject
{
    //�ӵ�����
    public float bulletForce;
    //��������
    public int magazines;
    //Ĭ�ϱ���
    public int spareAmmunition;
    //��ǰ���е�ҩ
    public int currMagazines;
    //��ǰ��ҩ
    public int currAmmunition;
    //�����˺�
    public int baseAttack;
    //��ǰ�˺�
    public int currAttack;
}
