using System;
using System.Collections.Generic;
using Building;
using States.MonoBehavior;
using UnityEngine;
using UnityEngine.SocialPlatforms;


[RequireComponent(typeof(DefenceBuildingState))]
public class RecoverTower : MonoBehaviour
{
    [Header("Base Setting")]
    public float recoverRadius=5f;

    [Range(2,6)]
    public int maxReoverTower=6;

    public Transform[] shootPoints;

    public Transform storagePoint;

    private DefenceBuildingState towerState;

    [Header("Recover Setting")]
    public float RecoverCd;

    public float powerStorageTime=3;

    public RecoverBullet bulletPrefab;

    public GameObject storageEffect;

    public GameObject gunEffect;

    private float mLastRecoverTime;

    private float mPowerStorageTime;

    private List<GameObject> targetList=new List<GameObject>();

    private RecoverBullet currBullet;

    private GameObject currEffect;
    

    public enum TowerEnum
    {
        RECOVE,NORMAL,Dead
    }

    private TowerEnum mTowerEnum;
    private void Awake()
    {
        towerState = GetComponent<DefenceBuildingState>();
    }

    private void Start()
    {
        mTowerEnum = TowerEnum.RECOVE;
        mLastRecoverTime = 0;
        mPowerStorageTime = powerStorageTime;
        GameModel.Instance.CurrTower += 1;
    }

    private void OnDestroy()
    {
        GameModel.Instance.CurrTower -= 1;
    }


    private void Update()
    {
        switch (mTowerEnum)
        {
            case TowerEnum.NORMAL:
                break;
            case TowerEnum.RECOVE:
                RecoverState();
                break;
            case TowerEnum.Dead:
                break;
        }
    }

    public void RecoverState()
    {
        
        
        if (!(targetList.Count > 0))
        {
            foundRecoverTarget();
        }
        if (mLastRecoverTime <= 0)
        {
            if (mPowerStorageTime <= 0)
            {
                for(int i=0;i<targetList.Count;i++)
                {
                    if (shootPoints[i])
                    {
                        currBullet =Instantiate(bulletPrefab,shootPoints[i].position, shootPoints[i].rotation);
                        currBullet.SetRecover(towerState.CurrAttack);
                        currBullet.bezier.setBezier(currBullet.transform.position,targetList[i].transform.position);
                    }
                }
                mLastRecoverTime = RecoverCd;
                mPowerStorageTime = powerStorageTime;
                Destroy(currEffect);
            }
            else
            {
                if(!currEffect)
                    currEffect = Instantiate(storageEffect, storagePoint.position, storagePoint.rotation);
                mPowerStorageTime -= Time.deltaTime;
            }
        }
        else
        {
            mLastRecoverTime -= Time.deltaTime;
        }
    }
    
    public bool foundRecoverTarget()
    {
        targetList.Clear();
        int layer = LayerMask.NameToLayer("Defence Building");
        var colliders =Physics.OverlapSphere(transform.position, recoverRadius, 1 << layer);

        if (colliders.Length == 0)
        {
            return false;
        }
        
        foreach (var target in colliders)
        {
            if (target.TryGetComponent(out RecoverTower recoverState))
            {
                continue;
            }
            if (targetList.Count == maxReoverTower)
            {
                return true;
            }
            
            if (target.CompareTag("building"))
            {
                targetList.Add(target.gameObject);
            }
            
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.green;
        Gizmos.DrawWireSphere(transform.position,recoverRadius);
    }
}
