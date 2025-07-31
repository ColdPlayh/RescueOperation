using System;
using States.MonoBehavior;
using UnityEngine;


public class BlastEffect : MonoBehaviour
{
    public float destoryTime=1f;

    public float blastRadius=5f;
    
    public int damage=25;
    

    private void Start()
    {
        Destroy(gameObject,destoryTime);
        int layer = LayerMask.NameToLayer("Enemy");
        var colliders = Physics.OverlapSphere(transform.position, blastRadius, 1 << layer);
        Debug.Log("co"+colliders.Length);
        foreach (var enmey in colliders)
        {
            enmey.GetComponent<EnemyState>().CurrHealth -= damage;
            Debug.Log("co"+enmey.GetComponent<EnemyState>().CurrHealth);
        }
    }
    public void setDamage(int input)
    {
        damage = input;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color=Color.green;
        Gizmos.DrawWireSphere(transform.position,blastRadius);
    }
}
