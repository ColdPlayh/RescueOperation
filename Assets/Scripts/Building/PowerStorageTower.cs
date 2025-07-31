using DuloGames.UI;
using States.MonoBehavior;
using UnityEngine;

public class PowerStorageTower : MonoBehaviour
{
     [Header("Base Setting")]
    private DefenceBuildingState towerState;

    public Transform headPart;

    public Transform gunPart;

    [Header("Attack Setting")]

    public float targetPointUpOffset=2f;
    
    public float sightRadius;

    public float powerStorageTime=1f;

    public float storageTime = 4f;

    private float lastStorageTime;

    public Transform shootPoint;

    public Transform bulletPrefab;

    public float bulletForce = 100f;

    public Transform PowerStoreageEffect;

    public Transform gunEffect;

    private Transform currEffect;


    private EnemyState tempEnemyState;

    private Level2EnemyControl tempEnemyControl;
    

    private Transform currBullet;
    
    private Transform mAttackTarget;

    private GameObject lastAttackTarget;

    private Vector3 lastTargetPos;
    
    private float lastFireTime=0;

    public enum TowerEnum
    {
        NORMAL,ATTACK,DEAD
    }

    private TowerEnum mTowerEnum;
    private void Awake()
    {
        
        towerState = GetComponent<DefenceBuildingState>();

        Init();
    }

    void Init()
    {
        mTowerEnum = TowerEnum.NORMAL;
        lastAttackTarget = gameObject;

    }

    private void Start()
    {
        lastFireTime = 0;
        lastStorageTime = storageTime;
        GameModel.Instance.CurrTower += 1;
    }
    private void OnDestroy()
    {
        GameModel.Instance.CurrTower -= 1;
    }
    public void Update()
    {
        switch (mTowerEnum)
        {
            case TowerEnum.NORMAL:
                NormalState();
                break;
            case TowerEnum.ATTACK:
                AttackState();
                break;
            case TowerEnum.DEAD:
                break;
        }
    }

    private void NormalState()
    {
        FoundAttackTarget();
        
        
        //TODO: 播放动画
    }

    private void AttackState()
    {
        if (!lastAttackTarget.Equals(mAttackTarget))
        {
            lastAttackTarget = mAttackTarget.gameObject;
            tempEnemyState= mAttackTarget.GetComponent<EnemyState>();
            tempEnemyControl = mAttackTarget.GetComponent<Level2EnemyControl>();
        }
        
        if (tempEnemyState.CurrHealth <= 0)
        {
            mAttackTarget.gameObject.layer = LayerMask.NameToLayer("Default");
            if (!FoundAttackTarget())
            {
                mAttackTarget = null;
                mTowerEnum = TowerEnum.NORMAL;

            }
            return;
        }
        
        if (Vector3.Distance(transform.position, mAttackTarget.position)>sightRadius)
        {
            mAttackTarget = null;
            FoundAttackTarget();
            return;
        }
        headPart.LookAt(mAttackTarget);
        
        gunPart.forward = tempEnemyControl.scopePoint.position- gunPart.transform.position;
        
        
        if (lastFireTime<=0)
        {
            if (lastStorageTime <= 0)
            {
               
                lastStorageTime = storageTime;
                lastFireTime = powerStorageTime;
                Instantiate(gunEffect, shootPoint.position, shootPoint.rotation);
                Destroy(currEffect.gameObject);
                currBullet=Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
                currBullet.GetComponent<BlastBullet>().SetDamage(towerState.CurrAttack);
                currBullet.GetComponent<Rigidbody>().velocity = shootPoint.forward * bulletForce;

            }
            else
            {
                lastStorageTime -= Time.deltaTime;
                if(!currEffect)
                    currEffect=Instantiate(PowerStoreageEffect, shootPoint.position, shootPoint.rotation);
            }
        }
        else
        {
            lastFireTime -= Time.deltaTime;

        }
    }
    bool FoundAttackTarget()
    {
        
        int layer = LayerMask.NameToLayer("Enemy");
        //用一个收集器 在视线范围内所有物体都会进入到var里面 var可以存任何类型object
        var colliders = Physics.OverlapSphere(transform.position, sightRadius,1<<layer);
        
        if (colliders.Length <= 0) return false;
        
        float lastDistance=float.MaxValue;
        GameObject buffer=null;
        //遍历
        foreach (var target in colliders)
        {
            float temp= Vector3.Distance(transform.position, target.transform.position);
            if (temp < lastDistance)
            {
                lastDistance = temp;
                buffer = target.gameObject;
            }
        }

        if (buffer)
        {
            mAttackTarget = buffer.transform;
            mTowerEnum = TowerEnum.ATTACK;
            return true;
        }
        else
        {
            mTowerEnum = TowerEnum.NORMAL;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position,sightRadius);
    }
        
}
