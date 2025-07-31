using UnityEngine;
using System.Collections;

public class ImpactScript : MonoBehaviour {

	[Header("Impact Despawn Timer")]
	//多久销毁
	public float despawnTimer = 10.0f;

	[Header("Audio")]
	public AudioClip[] impactSounds;
	public AudioSource audioSource;

	private void Start () {
		StartCoroutine (DespawnTimer ());
		audioSource.clip = impactSounds
			[Random.Range(0, impactSounds.Length)];
		audioSource.Play();
	}
	
	private IEnumerator DespawnTimer() {
		//等待多久销毁
		yield return new WaitForSeconds (despawnTimer);
		//销毁物体
		Destroy (gameObject);
	}
}