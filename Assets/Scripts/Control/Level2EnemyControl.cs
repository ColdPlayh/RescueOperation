using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Manager;
using RootMotion.FinalIK;
using States.MonoBehavior;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using Random = System.Random;


[RequireComponent(typeof(NavMeshAgent))]
public class Level2EnemyControl : MonoBehaviour,IGameOver
{
    public enum EnemyEnum
    {
        Normal,Attack,ChaseBoos,Dead,Over
    }
    
    //Compoent

    private Animator mAnim;
    private NavMeshAgent mAgent;
    private EnemyState enemyState;
    private EnemyAttack enemyAttack;

    [Header("Base Setting")]
    public  Transform scopePoint;
    
    public float normalSpeed=1f;

    public float chaseSpeed=2f;

    public float sightRadius = 5f;

    public float wayOffset = 2f;

    private Queue<WayPoint> mWayPoints;

    private WayPoint mCurrWayPoint;

    private EnemyEnum mEnemyEnum;

    [Header("Attack Setting")] 
    public List<String> canAttackType=new List<string>();

    public bool canAttackPlayer;
    public bool canAttackBuilding;
    public float attackRange = 1.5f;
    public float attackCd = 1.5f;

    private List<GameObject> mAttackTargetBuffer=new List<GameObject>();

    private GameObject mAttackTarget;

    private DefenceBuildingState targetState;

    private float mLastAttackTime=0;
    
    //anim bool
    private bool isWalk;
    private bool isRun;
    private bool isDead;
    
    
    //other bool
    private bool isFoundTarget;

    private bool haveReward=true;

    private void OnEnable()
    {
        enemyState.enmeyDead += OnEnemyDead;
        if (GameManager.isInitialized())
        {
            GameManager.Instance.RegisterEnemy(this);
        }
    }

    public void OnDisable()
    { 
        enemyState.enmeyDead -= OnEnemyDead;
        if (GameManager.isInitialized())
        {
            GameManager.Instance.UnRegisterEnemy(this);
        }
    }

    private void OnDestroy()
    {
        if (haveReward)
        {
            GameModel.Instance.CurrCoin += enemyState.KillCoin;
            GameModel.Instance.CurrScore += enemyState.KillScore;
            GameModel.Instance.CurrEnemy -= 1;
        }

        EnemyManager.Instance.buffer.Remove(this);
    }

    private void Awake()
    {
        mAnim = GetComponent<Animator>();
        mAgent = GetComponent<NavMeshAgent>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyState = GetComponent<EnemyState>();
       

        Init();
    }

    private void Start()
    {
        mWayPoints=EnemyManager.Instance.GetWayPoints();
        mCurrWayPoint = mWayPoints.Dequeue();
    }

    public void Init()
    {
        mAgent.speed = normalSpeed;
        mEnemyEnum = EnemyEnum.Normal;
        if (canAttackPlayer)
        {
            canAttackType.Add(Constant.ATTACK_PLAYER);
        }

        if (canAttackBuilding)
        {
            canAttackType.Add(Constant.ATTACK_BULIDING);
        }
    }
    private void Update()
    {
        switch (mEnemyEnum)
        {
            case EnemyEnum.Normal:
                NormalState();
                break;
            case EnemyEnum.ChaseBoos:
                break;
            case EnemyEnum.Attack:
                AttackState();
                break;
            case EnemyEnum.Over:
                break;
            case EnemyEnum.Dead:
                DeadState();
                break;
        }
        Test();
        SwitchAnim();
    }

    public void Test()
    {
        
    }

    public void SwitchAnim()
    {
        mAnim.SetBool("isWalk",isWalk);
        mAnim.SetBool("isRun",isRun);
        mAnim.SetBool("isDead",isDead);
    }
    
    public void NormalState()
    {
        if (isDead) return;
        isWalk = true;
       // mAgent.enabled = true;
        mAgent.isStopped = false;
        //如果没有到达
        if (Vector3.Distance(transform.position, 
            new Vector3(mCurrWayPoint.transform.position.x ,transform.position.y,mCurrWayPoint.transform.position.z))
            >= mAgent.stoppingDistance+wayOffset)
        {
            mAgent.destination = mCurrWayPoint.GetPosition();
        }
        else
        {
            mAgent.isStopped = true;
            if (mWayPoints.Count != 0)
            {
                mCurrWayPoint = mWayPoints.Dequeue();
            }
            else
            {
                isWalk = false;
                mAgent.destination = transform.position;
            }
        }
        FoundAttackTarget();
    }

    public void OnEnemyDead()
    {
        Debug.Log("isdead");
        mEnemyEnum = EnemyEnum.Dead;
    }
    public void AttackState()
    {
        if (isDead) return;
        if (!mAttackTarget)
        {
            Debug.Log(FoundAttackTarget()+":"+mAttackTarget);
            if (!FoundAttackTarget())
            {

                mEnemyEnum = EnemyEnum.Normal;
                return;
            }
        }
        
        if (!targetState)
        {
            mAttackTarget.TryGetComponent(out targetState);
        }
        if (Vector3.Distance(transform.position, mAttackTarget.transform.position) <= attackRange)
        {
            
            mAgent.isStopped = true;
           // mAgent.enabled = false;
            isWalk = false;
            isRun = false;
            
            if (mLastAttackTime <= 0)
            {
                int dice = GetAttackType();

                switch (dice)
                {
                    case 0:
                        mAnim.Play("Roar");
                        break;
                    
                    case 1:
                        mAnim.Play("AttackOne");
                        if(targetState)
                            targetState.CurrHealth -= enemyAttack.OneAttackDamage;
                        break;
                    
                    case 2:
                        mAnim.Play("AttackTwo");
                        if(targetState)
                            
                            targetState.CurrHealth -= enemyAttack.TwoAttackDamage;
                        break;
                    
                    case 3:
                        mAnim.Play("AttackThree");
                        if(targetState)
                            targetState.CurrHealth -= enemyAttack.ThreeAttackDamage;
                        break;
                    
                    case 4:
                        mAnim.Play("AttackFour");
                        if(targetState)
                            targetState.CurrHealth -= enemyAttack.FourAttackDamage;
                        break;
                }
                mLastAttackTime = attackCd;
            }
            else
            {
                mLastAttackTime-= Time.deltaTime;
            }
        }
        else
        {
            mAgent.enabled = true;
            mAgent.isStopped = false;
            isWalk = true;
            isRun = true;
            mAgent.destination = mAttackTarget.transform.position;
        }
    }

    int GetAttackType()
    {
        return UnityEngine.Random.Range(0, 5);
    }
    bool FoundAttackTarget()
    {
        if (GameManager.Instance.gameOver)
        {
            return false;
        }
        if (isDead) return false;
        int layer = LayerMask.NameToLayer("Defence Building");
        //用一个收集器 在视线范围内所有物体都会进入到var里面 var可以存任何类型object
        var colliders = Physics.OverlapSphere(transform.position, sightRadius,1<<layer);
        
        if (colliders.Length <= 0) return false;
        
        float lastDistance=float.MaxValue;
        GameObject buffer=null;
        //遍历
        foreach (var target in colliders)
        {
            float temp= Vector3.Distance(transform.position, target.transform.position);
            if (temp < lastDistance && target.transform.position.y<2f)
            {
                lastDistance = temp;
                buffer = target.gameObject;
            }
        }

        if (buffer)
        {
            mAttackTarget = buffer;
            mEnemyEnum = EnemyEnum.Attack;
            return true;
        }
        return false;
    }

    public void DeadState()
    {
        mAgent.isStopped = true;
        isWalk = false;
        isRun = false;
        isDead = true;
       
        Destroy(gameObject,2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.blue;
        Gizmos.DrawWireSphere(transform.position,sightRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hostage"))
        {
            haveReward = false;
            GameModel.Instance.CurrFall -= 1;
            Destroy(gameObject);
        }

    }

    public void OverExecute()
    {
        isWalk = false;
        isRun = false;
        mEnemyEnum = EnemyEnum.Over;
        mAgent.isStopped = true;

    }
}
