using System;
using UnityEngine;


public class BlastBullet : MonoBehaviour
{
    public BlastEffect explosionEffect;
    public float yOffset = -4f;
    private int damage;
    
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            
            var effect=Instantiate(explosionEffect, 
                transform.position +Vector3.up * yOffset,
                Quaternion.Euler(Vector3.up));
            effect.setDamage(damage);
            Destroy(gameObject);
        }
    }

    public void SetDamage(int input)
    {
        damage = input;
    }
}
