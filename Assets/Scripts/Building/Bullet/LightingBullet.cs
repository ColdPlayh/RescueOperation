using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LightingBullet : MonoBehaviour
{
 
    public GameObject gunEffect;

    public GameObject hitEffect;

    public int pointCount = 100;

    public float startLineWidth = 0.3f;

    public float endLineWidth = 0.3f;

    [FormerlySerializedAs("speed")] public float showSpeed=1f;
    public float hideSpeed=2.5f;
    private List<Vector3> pointsList=new List<Vector3>();

    private LineRenderer lineRenderer;

    private Vector3 tempEndPos;

    private Transform startTrans;

    private Transform endTrans;
    

    private Coroutine currCoroutine;

    private bool isShow = false;

    private void Awake()
    {
        
        gunEffect = transform.GetChild(0).gameObject;
        hitEffect = transform.GetChild(1).gameObject;
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        hitEffect.SetActive(false);
    }

    public void Show(Transform start,Transform end)
    {
        startTrans = start;
        endTrans = end;
        lineRenderer.startWidth = startLineWidth;
        lineRenderer.endWidth = endLineWidth;
        lineRenderer.positionCount = pointCount;
        if (hitEffect.transform.position != endTrans.position)
        {
            hitEffect.transform.position=Vector3.MoveTowards(hitEffect.transform.position, endTrans.position, showSpeed);
        }
        else
        {
            hitEffect.SetActive(true);

        }
        gunEffect.transform.SetPositionAndRotation(start.position,start.rotation);
        lineRenderer.SetPositions(GetPoints(start.position,hitEffect.transform.position).ToArray());
        
    }
    public void Hide()
    {
        hitEffect.SetActive(false);

        if (hitEffect.transform.position != startTrans.position)
        {
            hitEffect.transform.position=Vector3.MoveTowards(hitEffect.transform.position, 
                startTrans.position, hideSpeed);
            lineRenderer.SetPositions(GetPoints(startTrans.position,hitEffect.transform.position).ToArray());
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public List<Vector3> GetPoints(Vector3 startPoint,Vector3 endPoint)
    {
        pointsList.Clear();
        for (int i = 0; i < pointCount; i++)
        {
          
            if (i == pointCount - 1)
            {
                pointsList.Add(endPoint);
            }
            else
            {
                pointsList.Add(Vector3.Lerp(startPoint, endPoint,i/pointCount));
            }
        }
        return pointsList;
    }
}
