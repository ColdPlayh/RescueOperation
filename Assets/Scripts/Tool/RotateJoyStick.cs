using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RotateJoyStick : MonoBehaviour
{
    public Image image;
    public void ShowImage()
    {
        image.gameObject.SetActive(true);

    }
    public void HideImage()
    {
        image.gameObject.SetActive(false);
    }
}
