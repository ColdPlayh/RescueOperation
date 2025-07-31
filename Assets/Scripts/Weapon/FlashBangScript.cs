
using System;
using Unity.Mathematics;
using UnityEngine;

public class FlashBangScript : MonoBehaviour
{
    public ParticleSystem explosionPrefab;

    public float fuseTime = 3f;

    public float blindRadius;

    public AudioSource impactSound;

    private Camera mainCamera;
    
    private void Awake()
    {
        GetComponent<Rigidbody>().velocity =PlayerControl.VelovityV3;
    }

    private void OnCollisionEnter (Collision collision) 
    {
        impactSound.Play();
    }

    void Start()
    {
        mainCamera=Camera.main;
        Invoke("Explose",fuseTime);
    }

    private void Explose()
    {
        BlindEnemy();
        
        if (CheckVisibility())
        {
            Debug.Log("闪光：闪到");
            FlashEffectScrpit.Instance.GoBlind();
        }
        else
        {
            Debug.Log("闪光：没有被闪");
            Instantiate(explosionPrefab, transform.position, quaternion.identity);
        }
        
        Destroy(gameObject);
        
    }

    private bool CheckVisibility()
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        var point = transform.position;

        foreach (var p in planes)
        {
            
            if (p.GetDistanceToPoint(point) > 0)
            {
                Ray ray = new Ray(mainCamera.transform.position, transform.position - mainCamera.transform.position);
                RaycastHit hit;
                
                if (Physics.Raycast(ray,out hit,1000, ~(1 << 3)))
                {
                    Debug.Log("2"+":"+(hit.transform.gameObject == this.gameObject)+":"+hit.transform.gameObject);
                    return hit.transform.gameObject == this.gameObject;
                }
            }
            else return false;
        }
        return false;
    }

    public void BlindEnemy()
    {
        var colliders = Physics.OverlapSphere(transform.position, blindRadius);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<EnmeyControl>(out EnmeyControl enmeyControl))
            {
                enmeyControl.setBlindState();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.yellow;
        Gizmos.DrawWireSphere(transform.position,blindRadius);
    }
}
