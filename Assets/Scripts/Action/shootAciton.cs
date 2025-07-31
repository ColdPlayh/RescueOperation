using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shootAciton : MonoBehaviour
{
    public void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(onClick);
    }
    public void onClick()
    {
        GameManager.Instance.player.Shooting();
    }

}
