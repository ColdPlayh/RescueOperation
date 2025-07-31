using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New CheckPointState", menuName = "CheckPoint State/Data")]
public class CheckPointState_So :ScriptableObject
{
    public int levelNum;
    public string levelName;
    public string levelInformation;
    public bool isOpen;
    public bool isNotHave;
    public string sceneName;
}
