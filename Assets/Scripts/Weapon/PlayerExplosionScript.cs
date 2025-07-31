using UnityEngine;
using System.Collections;
using States.MonoBehavior;

public class PlayerExplosionScript : MonoBehaviour {

    [Header("Customizable Options")]
    //粒子多久销毁
    public float despawnTime = 5.0f;

    public float radius;

    public int demage;

    [Header("Audio")]
    public AudioClip[] explosionSounds;
    public AudioSource audioSource;

    private void Start ()
    {
        StartCoroutine (DestroyTimer ());
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        var colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.layer==enemyLayer)
            {
                collider.GetComponent<EnemyState>().CurrHealth -= demage;
                break;
            }
        }
        if (audioSource != null)
        {
            audioSource.clip = explosionSounds
                [Random.Range(0, explosionSounds.Length)];
            audioSource.Play();
        }
			
    }
    private IEnumerator DestroyTimer () {
        yield return new WaitForSeconds (despawnTime);
        Destroy (gameObject);
    }
}