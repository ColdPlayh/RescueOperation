using UnityEngine;
using System.Collections;

public class BatteryExplosionScript : MonoBehaviour {

	[Header("Customizable Options")]
	//粒子多久销毁
	public float despawnTime = 10.0f;

	public float radius;

	public int demage;

	[Header("Audio")]
	public AudioClip[] explosionSounds;
	public AudioSource audioSource;

	private void Start () {
		StartCoroutine (DestroyTimer ());

		var colliders = Physics.OverlapSphere(transform.position, radius);
		foreach (var collider in colliders)
		{
			if (collider.CompareTag("Player"))
			{
				GameManager.Instance.CharacterState.CurrHealth -= demage;
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