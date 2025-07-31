using System;
using System.Collections;
using States.MonoBehavior;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class MaChineBullet : MonoBehaviour
{

    public float destoryTime = 5f;

    public GameObject effectPrefab;

    private Transform scopePoint;

    private int damage;

    private void Start()
    {
        StartCoroutine(DestoryTime());
    }

    IEnumerator DestoryTime()
    {
        Destroy(gameObject,destoryTime);
        yield return null;
    }

    public void SetDamage(int input,Transform trans)
    {
        damage = input;
        scopePoint = trans;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Instantiate(effectPrefab, scopePoint.position,transform.rotation);
            EnemyState enemyState = other.gameObject.GetComponent<EnemyState>();
            if (enemyState.CurrHealth <= damage)
            {
                enemyState.CurrHealth = 0;
            }
            else
            {
                enemyState.CurrHealth -= damage;
            }
            Destroy(gameObject);
        }
    }
}