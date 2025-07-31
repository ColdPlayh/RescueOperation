using System;
using States.MonoBehavior;
using UnityEngine;


public class MachineGunTower : MonoBehaviour
{
    [Header("Base Setting")]
    private DefenceBuildingState towerState;

    public Transform headPart;

    public Transform gunPart;

    public ParticleSystem gunEffect;

    [Header("Attack Setting")] 
    public float targetUpOffset=2f;
    
    public float sightRadius;

    public float fireRate;

    public Transform shootPoint;
    
    public GameObject bulletPrefab;

    public float bulletForce;

    private Transform mAttackTarget;

    private GameObject lastAttackTarget;

    private EnemyState tempEnemyState;

    private Level2EnemyControl tempEnemyControl;
        
    private Vector3 forceDir;

    private float lastFireTime=0;
    
    public enum TowerEnum
    {
        NOMARL, ATTACK, DEAD
    }

    private TowerEnum mTowerEnum;
    private void Awake()
    {
        
        towerState = GetComponent<DefenceBuildingState>();
        
        Init();
    }

    void Init()
    {
        mTowerEnum = TowerEnum.NOMARL;
        lastAttackTarget = gameObject;

    }

    private void Start()
    {
        lastFireTime = Time.time;
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
            case TowerEnum.NOMARL:
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
        headPart.LookAt(mAttackTarget);
        gunPart.forward = tempEnemyControl.scopePoint.position- gunPart.transform.position;
        if (tempEnemyState.CurrHealth <= 0)
        {
            mAttackTarget.gameObject.layer = LayerMask.NameToLayer("Default");
            mAttackTarget = null;
            if (!FoundAttackTarget())
            {
                mTowerEnum = TowerEnum.NOMARL;
            }
            return;
        }
        
        if (Vector3.Distance(transform.position, mAttackTarget.position)>sightRadius)
        {
            mAttackTarget = null;
            FoundAttackTarget();
            return;
        }
        
        if (Time.time-lastFireTime > 1 / fireRate)
        {
            
            lastFireTime = Time.time;
            if(gunEffect)
                Instantiate(gunEffect,shootPoint.position,shootPoint.rotation);
            var bullet = Instantiate(bulletPrefab, shootPoint.position,shootPoint.rotation).transform;
            bullet.GetComponent<MaChineBullet>().SetDamage(towerState.CurrAttack,tempEnemyControl.scopePoint);
            bullet.GetComponent<Rigidbody>().velocity = shootPoint.forward * bulletForce;
            //ToDO: 发射子弹攻击敌人
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
            mTowerEnum = TowerEnum.NOMARL;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position,sightRadius);
    }
}
