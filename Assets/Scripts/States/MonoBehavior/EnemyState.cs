using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace States.MonoBehavior
{
    public class EnemyState : MonoBehaviour
    {
        //敌人状态
        private EnemyHealth_So enemyHealthSo;
        [FormerlySerializedAs("templateState")] 
        public EnemyHealth_So templateHealth;

        private void Awake()
        {
            if (templateHealth!= null)
            {
                enemyHealthSo = Instantiate(templateHealth);
            }
        }

        public int CurrHealth
        {
            get => enemyHealthSo.currHealth;
            set
            {
                if (!value.Equals(enemyHealthSo.currHealth))
                {
                    int sub = CurrHealth - value;
                    if (sub == 0)
                    {
                        UIManager.Instance.DamageText = "攻击无效";
                    }
                    else
                    {
                        UIManager.Instance.DamageText = sub.ToString();
                    }
                    
                    if (value <= 0)
                    {
                        enemyHealthSo.currHealth = 0;
                        enmeyDead?.Invoke();
                    }
                    else
                    {
                        enemyHealthSo.currHealth = value;
                    }

                    if (value != enemyHealthSo.maxHealth)
                    {
                        OnHealthChanged?.Invoke(enemyHealthSo.currHealth,enemyHealthSo.maxHealth);
                    }
                }
            }
        }
        public Action<int,int> OnHealthChanged;
        public Action enmeyDead;

        public int Damage
        {
            get => enemyHealthSo.damage;
        }

        public int MaxHealth
        {
            get => enemyHealthSo.maxHealth;
        }

        public int KillScore
        {
            get => enemyHealthSo.killScore;
        }

        public int KillCoin
        {
            get => enemyHealthSo.killCoin;
        }

        public int killExperience
        {
            get => enemyHealthSo.killExperience;
        }
        
    }
    
}