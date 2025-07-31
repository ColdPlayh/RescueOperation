using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierManager : MonoBehaviour
{
    //p0 start  p1 center p2 end
   
    [Header("test")]
    [SerializeField, Range(0, 1)] 
    float slider;

    [Header("Base Setting")] 
    public float yOffset = 2f;

    public float heightOffset = 2f;

    public float forwardOffset = 1f;
    
    //p2距离起始点的比例
    [Range(0, 1f)] 
    public float precent=0.5f;
    
    public float speed=1;
    
    private bool isInit = false;
    
    public Vector3 p0, p1, p2;
    
    
    public void setBezier(Vector3 start,Vector3 end)
    {
        transform.position = start;
        
        p0 = start;
        
        p2 = end+Vector3.up*heightOffset+(end-start).normalized*forwardOffset;
        
        Vector3 minPoint= VectorTool.GetBetweenPoint(start, p2, precent);
        
        p1 = new Vector3(minPoint.x, minPoint.y + yOffset, minPoint.z);
        
        
        
        isInit = true;
    }
    private void Update()
    {
        if (isInit)
        {
            
            slider = slider + Time.deltaTime * speed;
            gameObject.transform.position = twoPointBezier(p0, p1, p2, slider);
        }
    }
    
    public Vector3 twoPointBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 result = new Vector3();
        float t1 = (1 - t) * (1 - t);
        float t2 = 2 * t * (1 - t);
        float t3 = t * t;
        result = p0 * t1 + p1 * t2 + p2 * t3;
        return result;
    }
    
}
