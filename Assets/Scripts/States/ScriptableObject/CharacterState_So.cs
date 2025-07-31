using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New CharacterState", menuName = "Character State/Data")]
public class CharacterState_So : ScriptableObject
{
   public int currHealth;
   public int maxHealth;
   public int currScore;
   public int currExperience;
   public int maxExperience;
   public int currLevel;
   public int maxLevel;
   public int currDamage;

}
