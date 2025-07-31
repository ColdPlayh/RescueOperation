using System;
using  UnityEngine;
namespace States.MonoBehavior
{
    public class CharacterState : MonoBehaviour
    {
        public CharacterState_So characterStateSo;
        public Action onCurrHealthChanged;
        public int CurrHealth
        {
            get { return characterStateSo.currHealth; }
            set
            {
                characterStateSo.currHealth = value;
                onCurrHealthChanged?.Invoke();
            }
        }
        
        public int MaxHealth
        {
            get { return characterStateSo.maxHealth; }
            set { characterStateSo.maxHealth = value; }
        }

        public int CurrDamage
        {
            get => characterStateSo.currDamage;
            set
            {
                characterStateSo.currDamage = value;
            }
        }
        
    }
}