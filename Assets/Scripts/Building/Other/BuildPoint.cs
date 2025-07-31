using System;
using UnityEngine;

public class BuildPoint : MonoBehaviour
{
    public AnyButton anyButton;
    public GameObject buildCanvas;
    private AnyButton currButton;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("tag"+other.tag);
        if (other.CompareTag("Player"))
        {
            currButton=Instantiate(anyButton,UIManager.Instance.otherCanvas.transform);
            currButton.setTextAndPrefab("建造",buildCanvas,transform);
        }
        if (other.gameObject.CompareTag("building"))
        {
            Debug.Log(11111);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("tag"+other.tag);
        if (other.CompareTag("Player"))
        {
            if(currButton)
                Destroy(currButton.gameObject);
        }
        if (other.gameObject.CompareTag("building"))
        {
            gameObject.SetActive(true);
        }
    }
    
}