using States.MonoBehavior;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAttack : MonoBehaviour
{
    public EnemyAttack_So enemyAttackSo;
    public int OneAttackDamage
    {
        get => enemyAttackSo.oneAttackDamage;
    }
    public int TwoAttackDamage
    {
        get => enemyAttackSo.twoAttackDamage;
    }
    public int ThreeAttackDamage
    {
        get => enemyAttackSo.threeAttackDamage;
    }
    public int FourAttackDamage
    {
        get => enemyAttackSo.fourAttackDamage;
    }
    public int FiveAttackDamage
    {
        get => enemyAttackSo.fiveAttackDamage;
    }
}
