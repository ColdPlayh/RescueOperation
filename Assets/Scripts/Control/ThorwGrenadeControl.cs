using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorwGrenadeControl : MonoBehaviour
{
    public GameObject grenadePoint;
    public float pointsCount;
    private Vector3 velocity;
    private Vector3 pos;
    private float boomTime;   
    private float Gravity = 9.8f;
    private bool isCheck;
    private List<Vector3> pointsList = new List<Vector3>();
    private void Start()
    {
        boomTime = GrenadeScript.grenadeTimer;
    }
    private void Update()
    {
        if (PlayerControl.isCheckV)
        {
            velocity = PlayerControl.VelovityV3;
        }
    }
    public List<Vector3> GetPoints()
    {
        velocity = PlayerControl.VelovityV3;
        pos = grenadePoint.transform.position;
        pointsList.Clear();
        
        float Interval = boomTime / pointsCount;
        Debug.Log("time" + Interval);
        for (int i = 0; i < pointsCount; i++)
        {
            pointsList.Add(pos);

            velocity += Vector3.down * Gravity * Interval;
            pos += velocity * Interval;
            Debug.Log(pos);
        }
        return pointsList;
        
        
    }
}
