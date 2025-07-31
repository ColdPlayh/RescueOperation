using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class RotateModel : MonoBehaviour
{

    public Transform modelTransform;
    

    private Vector3 startPoint;
    private Vector3 startAngel;
    [Range(0.1f,1f)]
    public float inputRotateSpeed;

    [Range(0.1f, 1f)] 
    public float selfRotateSpeed;

    private void Update()
    {
        Rotate();
    }

    public void Rotate()
    {
        if (Input.touchCount == 0)
        {
            selfRotate();
            return;
        }

        if (Input.touchCount == 1)
        {
            Debug.Log("手指按下了");
            startAngel = modelTransform.eulerAngles;
            if (Input.touches[0].phase ==TouchPhase.Began)
            {
                startPoint = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Moved)
            {
                var currPonit = Input.touches[0].position;
                var x = startPoint.x - currPonit.x;
                modelTransform.eulerAngles = startAngel + new Vector3(0, x*inputRotateSpeed, 0);
            }
            else
            {
                
                return;
            }
        }
    }

    public void selfRotate()
    {

        modelTransform.eulerAngles += new Vector3(0, selfRotateSpeed, 0);
    }
}
