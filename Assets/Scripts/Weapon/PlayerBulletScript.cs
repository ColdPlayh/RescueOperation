using UnityEngine;
using System.Collections;
using States.MonoBehavior;

public class PlayerBulletScript : MonoBehaviour {

	[Range(1, 100)]
	//最多多长时间销毁子弹预制体
	public float destroyAfter;
	//子弹是否在碰撞的时候销毁
	public bool destroyOnImpact = false;

	public bool impactOnDestroy= false;
	//碰撞后子弹被销毁的最短时间
	public float minDestroyTime; 
	//碰撞后子弹撞击的最长时间
	public float maxDestroyTime;
	[Header("Impact Effect Prefabs")]
	public Transform [] bloodImpactPrefabs;
	public Transform [] metalImpactPrefabs;
	public Transform [] dirtImpactPrefabs;
	public Transform []	concreteImpactPrefabs;
	public Transform ExpolsionPrefab;

	private void Start () 
	{	
		StartCoroutine (DestroyAfter ());
	}
	
	private void OnCollisionEnter (Collision collision) 
	{
		//如果碰撞时候等待一段时间销毁
		if (!destroyOnImpact) 
		{
			StartCoroutine (DestroyTimer ());
		}
		else 
		{
			Destroy (gameObject);
		}

		if (ExpolsionPrefab != null)
		{
			Instantiate (ExpolsionPrefab, transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			return;
		}

		//忽略玩家
		if (collision.gameObject.tag == "Player") 
		{
		
			Debug.LogWarning("Collides with player");
			Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());

		}
		//如果该物体的特效为出血
		if (collision.gameObject.tag == "Blood") 
		{
			Debug.Log("击中boold");
			
			Instantiate (bloodImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			
			if (collision.gameObject.GetComponent<EnemyState>().CurrHealth <=
			    GameManager.Instance.CharacterState.CurrDamage)
			{
				collision.gameObject.GetComponent<EnemyState>().CurrHealth = 0;
			}
			else
			{
				collision.gameObject.GetComponent<EnemyState>().CurrHealth -=
					GameManager.Instance.CharacterState.CurrDamage;
			}
			
			Destroy(gameObject);
		}

		//金属
		if (collision.transform.tag == "Metal") 
		{
			Instantiate (metalImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			if (collision.transform.TryGetComponent<BatteryControl>(out BatteryControl batteryControl))
			{
				UIManager.Instance.DamageText = "无效";
				collision.transform.GetComponent<EnemyHpBar>().showStringText();
				return;
			}
			if (collision.transform.TryGetComponent<EnemyState>(out EnemyState enemyState))
			{
				if (enemyState.CurrHealth <= GameManager.Instance.CharacterState.CurrDamage)
				{
					enemyState.CurrHealth = 0;
				}
				else
				{
					enemyState.CurrHealth -= GameManager.Instance.CharacterState.CurrDamage;
				}
			}
			
			Destroy(gameObject);
		}
		//木屑
		if (collision.transform.tag == "Dirt") 
		{
			Instantiate (dirtImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			
			Destroy(gameObject);
		}

		//土壤
		if (collision.transform.tag == "Concrete") 
		{
			Instantiate (concreteImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));

			Destroy(gameObject);
		}

		//射击把
		if (collision.transform.tag == "Target") 
		{
			collision.transform.gameObject.GetComponent
				<TargetScript>().isHit = true;

			Destroy(gameObject);
		}
		//爆炸物
		if (collision.transform.tag == "ExplosiveBarrel") 
		{
			collision.transform.gameObject.GetComponent
				<ExplosiveBarrelScript>().explode = true;

			Destroy(gameObject);
		}
	}

	private IEnumerator DestroyTimer () 
	{
		yield return new WaitForSeconds
			(Random.Range(minDestroyTime, maxDestroyTime));
		Destroy(gameObject);
	}

	private IEnumerator DestroyAfter () 
	{
		
		yield return new WaitForSeconds (destroyAfter);
		if (impactOnDestroy)
		{
			Instantiate(ExpolsionPrefab, transform.position, transform.rotation);
		}
		Destroy (gameObject);
	}
}