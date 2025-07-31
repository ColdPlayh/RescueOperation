using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryBulletScrpit : MonoBehaviour
{
    [Header("Base Setting")] 
    public float destroyTime;

    public bool isDestroyOnImpact;
    
    public Transform explosionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("other"+other.tag);
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
        
    }
}
