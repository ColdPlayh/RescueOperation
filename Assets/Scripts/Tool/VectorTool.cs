using System;
using UnityEngine;

public class VectorTool : MonoBehaviour
{
   
    public static Vector3 GetBetweenPoint(Vector3 start, Vector3 end, float precent)
    {
        Vector3 result;
        if (precent > 1 || precent < 0)
        {
            throw new Exception("百分比不对");
        }
        Vector3 direction = (end - start).normalized;
        float distance = Vector3.Distance(start, end);
        result = start + direction * distance * precent;
        return result;
    }
}