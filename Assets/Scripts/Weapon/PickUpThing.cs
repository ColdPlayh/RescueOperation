using Manager;
using UnityEngine;
using UnityEngine.UI;

public class PickUpThing : MonoBehaviour
{
   private GameObject PickUpPanel;
   private Button PickUpButton;
   
   
   private void Start()
   {
      PickUpPanel = UIManager.Instance.PickUpPanel;
      PickUpButton = UIManager.Instance.PickUpButton;
      PickUpButton.onClick.AddListener(
         () =>
         {
            PickUpPanel.gameObject.SetActive(true);
            PickUpButton.gameObject.SetActive(false);
         }
         );
   }

   public void OnCollisionEnter(Collision other)
   {
      if (other.gameObject.CompareTag("Player"))
      {
         Debug.Log("检测岛玩家");
         PickUpButton.gameObject.SetActive(true);
         WeaponManager.Instance.SetPickUpAndRenderWeapon(gameObject.name);
      }
     
   }

   private void OnTriggerStay(Collider other)
   {
      
   }

   private void OnCollisionExit(Collision other)
   {
      PickUpButton.gameObject.SetActive(false);
      WeaponManager.Instance.HideAllRenderWeapon();
   }
}
