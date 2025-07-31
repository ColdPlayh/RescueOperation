using UnityEngine;
[CreateAssetMenu(fileName = "New EnmeyState", menuName = "Enemy State/Data")]
public class EnemyHealth_So : ScriptableObject
{
    public int currHealth;
    public int maxHealth;
    public int damage;
    public int killScore;
    public int killCoin;
    public int killExperience;
}