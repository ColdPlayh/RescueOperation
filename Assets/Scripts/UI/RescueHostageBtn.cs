using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RescueHostageBtn : MonoBehaviour
{
    private bool isRescueing = false;
    public Canvas victoryCanvas;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(RescueHostage);
    }

    public void RescueHostage()
    {
        if (!isRescueing)
        {
            StartCoroutine(Process(10f));
            GetComponent<Button>().enabled = false;
        }
    }

    IEnumerator Process(float time)
    {
        isRescueing = true;
        UIManager.Instance.circleProcess(time);
        while (!UIManager.Instance.IsProcessed)
        {
            yield return null;
        }
        Debug.Log("游戏胜利");
        UIManager.Instance.IsProcessed = false;
        Instantiate(victoryCanvas);
        //Time.timeScale = 0;
        Destroy(gameObject);
        yield break;
    }

    public void DestoryObj()
    {
        Destroy(gameObject);
    }
}
