using System;
using System.Collections.Generic;
using States.MonoBehavior;
using UnityEngine;

[RequireComponent(typeof(DefenceBuildingState))]
public class LightingTower : MonoBehaviour
{
    [Header("Base Setting")]
    private DefenceBuildingState towerState;

    public Transform headPart;

    public Transform gunPart;

    [Header("Attack Setting")]

    public float targetPointUpOffset=2f;
    
    public float sightRadius;

    public float fireRate;

    public Transform shootPoint;

    public LightingBullet bulletPrefab;
    
    private EnemyState tempEnemyState;

    private Level2EnemyControl tempEnemyControl;
    
    private LineRenderer lineRenderer;

    private LightingBullet currBullet;
    
    private Transform mAttackTarget;

    private GameObject lastAttackTarget;

    private Vector3 lastTargetPos;
    
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
        
        if (tempEnemyState.CurrHealth <= 0)
        {
            mAttackTarget.gameObject.layer = LayerMask.NameToLayer("Default");
            if(currBullet)
                currBullet.Hide();
            if (!FoundAttackTarget() && !currBullet)
            {
                mAttackTarget = null;
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
        headPart.LookAt(mAttackTarget);

        gunPart.forward = tempEnemyControl.scopePoint.position- gunPart.transform.position;
        
        if (currBullet)
        {
            currBullet.Show(shootPoint,tempEnemyControl.scopePoint);
        }
        else
        {
            currBullet=Instantiate(bulletPrefab,shootPoint.position,shootPoint.rotation);
        }
        
        if (Time.time-lastFireTime > 1 / fireRate)
        {
            tempEnemyState.CurrHealth -= towerState.CurrAttack;
            lastFireTime = Time.time;
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
    

    public void SetLineWidth(float width)
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position,sightRadius);
    }
}
