using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowButton : MonoBehaviour
{
    public GameObject canvasPrefab;
    public AnyButton anyButton;
    private AnyButton currButton;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currButton = Instantiate(anyButton,UIManager.Instance.otherCanvas.transform);
            currButton.setTextAndPrefab("升级",canvasPrefab,transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(currButton) 
                Destroy(currButton.gameObject);
        }
    }
}
