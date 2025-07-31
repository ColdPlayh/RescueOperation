using System;
using UnityEngine;
using UnityEngine.UI;

public class CircleBar : MonoBehaviour
{
    private float inputTime;
    private float processTime;
    private bool isStart=false;
    private Transform playerCurrTrans;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            isStart = true;
        
        if (isStart)
        {
            if (processTime <=inputTime)
            {
                Debug.Log(1f * processTime / inputTime);
                GetComponent<Image>().fillAmount = 1f * (processTime / inputTime);
                processTime += Time.deltaTime;
            }
            else
            {
                Debug.Log("1111111");
                UIManager.Instance.IsProcessed = true;
                FindObjectOfType<PlayerControl>().IsCanWalk = true;
                Destroy(gameObject);
            }
        }
    }

    public void setTime(float time)
    {
        inputTime = time;
        processTime = 0;
        StartProcess();
    }

    public void StartProcess()
    {
        isStart = true;
        FindObjectOfType<PlayerControl>().IsCanWalk = false;
    }
    
    
}
