using System;
using System.Net.Mime;
using UnityEngine;
using  UnityEngine.UI;


public class WaveText : MonoBehaviour
{
    private float time;

    private Text waveText;

    public float destoryTime=2f;
    [HideInInspector]
    public bool isInit = false;

    public bool isCome = false;
    private void Awake()
    {
        waveText = GetComponent<Text>();
    }

    public void SetTime(float input)
    {
        time = input;
        isInit = true;
    }

    private void Update()
    {
        if (!isInit)
            return;
        
        if (time >= 0 && !isCome)
        {
            waveText.text = "距离下一波敌人到来还有" + Mathf.Round(time)+"秒！";
            time -= Time.deltaTime;
        }
        else
        {
            isCome = true;
            time = 0;
            waveText.text = "敌人来袭";
            Destroy(gameObject,destoryTime);
        }
    }
}
