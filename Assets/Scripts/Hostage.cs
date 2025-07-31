using UnityEngine;

public class Hostage : MonoBehaviour
{
    private bool isinstantiate;
    [HideInInspector]
    public Transform btn;
    private void OnTriggerEnter(Collider other)
    {
        if (!isinstantiate)
        {
            btn=Instantiate(UIManager.Instance.rescueHostageBtn.transform,
                UIManager.Instance.otherCanvas.transform);
            isinstantiate = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        isinstantiate = false;
        if(btn!=null)
            btn.GetComponent<RescueHostageBtn>().DestoryObj();
    }
}
