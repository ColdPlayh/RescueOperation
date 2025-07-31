using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTower : MonoBehaviour
{
    public float rotateSpeed=0.1f;
    void Update()
    {
        transform.eulerAngles += new Vector3(0, rotateSpeed, 0);
    }
}
