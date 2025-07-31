using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrenadeScript : MonoBehaviour {

	[Header("Timer")]
	//手雷爆炸前的时间
	public static float  grenadeTimer = 5.0f;

	[Header("Explosion Prefabs")]
	//爆炸的特效
	public Transform explosionPrefab;

	[Header("Explosion Options")]
	//爆炸的范围
	public float radius = 25.0F;
	//爆炸力大小
	public float power = 350.0F;	

	[Header("Audio")]
	//碰撞声音
	public AudioSource impactSound;

	private void Awake () 
	{
		//手榴弹随机旋转
		GetComponent<Rigidbody>().AddRelativeTorque 
		   (Random.Range(500, 1500),   //X 
			Random.Range(0,0), 		 //Y 
			Random.Range(0,0)  		 //Z 
			* Time.deltaTime * 5000);
	}
    private void Start () 
	{
		GetComponent<Rigidbody>().velocity =PlayerControl.VelovityV3;
		
		StartCoroutine (ExplosionTimer ());
	}

	private void OnCollisionEnter (Collision collision) 
	{
		impactSound.Play ();
	}
	

	private IEnumerator ExplosionTimer () 
	{
		yield return new WaitForSeconds(grenadeTimer);

		RaycastHit checkGround;
		if (Physics.Raycast(transform.position, Vector3.down, out checkGround, 50))
		{
			Instantiate (explosionPrefab, checkGround.point, 
				Quaternion.FromToRotation (Vector3.forward, checkGround.normal)); 
		}

		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody> ();

			if (rb != null)
				rb.AddExplosionForce (power * 5, explosionPos, radius, 3.0F);				
		}
		Destroy (gameObject);
	}
}