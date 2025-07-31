using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Weapon;

public class PickUpPanel : MonoBehaviour
{
    [FormerlySerializedAs("rocketGameObj")]

    private Button repMainWeaponBtn;

    private Button repSecondaryWeaponBtn;

    private Button cancleBtn;
    void Start()
    {
        repMainWeaponBtn = transform.GetChild(0).GetChild(2).GetComponent<Button>();
        repSecondaryWeaponBtn = transform.GetChild(0).GetChild(3).GetComponent<Button>();
        cancleBtn = transform.GetChild(0).GetChild(4).GetComponent<Button>();
        repMainWeaponBtn.onClick.AddListener(
            () =>
            {
                UIManager.Instance.RegisterMainWeapon(WeaponManager.Instance.PickUpWepaon);
                UIManager.Instance.PickUpPanel.SetActive(false);
            }
            );
        repSecondaryWeaponBtn.onClick.AddListener(
            () =>
            {
                UIManager.Instance.RegisterSecondaryWeapon(WeaponManager.Instance.PickUpWepaon);
                UIManager.Instance.PickUpPanel.SetActive(false);
            }
            );
        cancleBtn.onClick.AddListener(
            () =>
            {
                UIManager.Instance.PickUpPanel.SetActive(false);
            }
            );
    }

}
