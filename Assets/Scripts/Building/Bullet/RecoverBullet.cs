using System;
using UnityEngine;
public class RecoverBullet : MonoBehaviour
{

    private int recoverValue=5;
    [HideInInspector]
    public BezierManager bezier;

    private void Awake()
    {
        bezier = GetComponent<BezierManager>();
    }

    public void SetRecover(int input)
    {
        recoverValue = input;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="building"&& !other.TryGetComponent(out RecoverTower tower))
        {
            Debug.Log("otherbuilding");
            other.GetComponent<DefenceBuildingState>().CurrHealth += recoverValue;
            Destroy(gameObject);
        }
    }
}
