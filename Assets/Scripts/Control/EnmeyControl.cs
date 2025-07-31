using Manager;
using States.MonoBehavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;


public class EnmeyControl : MonoBehaviour,IEnemy,IGameOver
{
    public enum EnemyEnum
    {
        GUARD,PATROL,CHASE,DEAD,Blind
    }

    [FormerlySerializedAs("enemyState")] 
    [Header("Base Setting")]
    //普通状态下的速度
    public float normalSpeed;

    public float hitRate=0.75f;
    
    
    //敌人数据
    private EnemyEnum enemyEnum;
    //敌人巡逻时侯下一步的地点
    private Vector3 wayPoint;
    

    [Header("Guard Setting")] 
    //敌人是否为站岗敌人
    public bool isGuard;
    private Vector3 guardCenter;
    private Quaternion guardRatation;


    [Header("Patrol Setting")] 
    public float patrolRadius;
    //ins窗口等待的时间
    public float afterPatrolWaitTime;

    public float wayFindingInterval=5f;

    private float mWayFindingInterval;

    [Header("Chase Setting")] 
    
    //追击的速度
    public float cahseSpeed;
    //可视范围
    public float sightRadius;
    //射击范围
    public float shootRadius;
    //追击之后等待时间
    public float aftertChaseWaitTime;
    //被使用追击等待时间
    private float ChaseWaitTime;

    //等待的时间
    private float patrolWaitTime;
    //追击目标 只能是玩家
    private PlayerControl mAttackTarget;

    [Header("Shoot Setting")]
    //射速
    public float shootRate;
    //射击伤害
    public int shootDeamge;
    //单发or三连发
    public bool isSingleShoot;
    //射击的火光
    public Light shootLight;
    //射击的声音
    public AudioClip shootClip;
    
    private AudioSource mAudioSource;
    //上一次射击的时间
    private float lastShootTime;

    [Header("Blind Setting")] 
    public float blindWaitTime;
    
    
    //敌人状态
    private EnemyEnum mEnemyEnum;
    //自身组件
    private Animator mAnim;
    //navmesh agent
    private NavMeshAgent mAgent;
    //enemyState
    private EnemyState enemyState;

    private bool isBlind;
    private float blindTime;

    //动画控制
    //控制走路的动画
    private bool isWalk;
    //控制追击的动画
    private bool isChase;
    //控制死亡的动画
    private bool isDead;
    //游戏是否结束
    private bool isGameOver;

    private void Awake()
    {
        //获取组件
        mAgent = GetComponent<NavMeshAgent>();
        mAnim = GetComponent<Animator>();
        mAudioSource = GetComponent<AudioSource>();
        enemyState = GetComponent<EnemyState>();
        //设置速度
        mAgent.speed = normalSpeed;
        //设置巡逻中心
        guardCenter = transform.position;
        //设置默认旋转角度
        guardRatation = transform.rotation;
        
        //wait时间设置
        patrolWaitTime = afterPatrolWaitTime;
        ChaseWaitTime = aftertChaseWaitTime;
        blindTime = blindWaitTime;
        mWayFindingInterval = wayFindingInterval;

    }

   
    private void OnHealthChanged(int currHealth,int maxHealth)
    {
        Debug.Log("敌人当前生命值"+currHealth);
        if (currHealth ==0)
        {
            Debug.Log("当前生命值为0");
            mEnemyEnum = EnemyEnum.DEAD;
        }
    }

    public void init()
    {
        enemyState.CurrHealth = enemyState.MaxHealth;
        RegisterAndInit();
    }
    private void Start()
    {
        //数据初始化
        init();
        //如果是站岗敌人进入站岗状态
        if (isGuard)
        {
            mEnemyEnum = EnemyEnum.GUARD;
        }
        else
        {
            //创建一个路径
            CreateWayPoint();
            mEnemyEnum = EnemyEnum.PATROL;
            
        }
        
        //初始化上一次射击时间
        lastShootTime = Time.time;
    }

    private void Update()
    {
        SwitchAnim();
        if (foundPlayer()&& mEnemyEnum!=EnemyEnum.DEAD)
        {
            mEnemyEnum = EnemyEnum.CHASE;
        }
        switch (mEnemyEnum)
        {
            case EnemyEnum.GUARD:
                guardState();
                break;
            case EnemyEnum.PATROL:
                PatrolState();
                break;
            case EnemyEnum.CHASE:
                ChaseState();
                break;
            case EnemyEnum.Blind:
                BlindState();
                break;
            case EnemyEnum.DEAD:
                DeadState();
                break;
        }
    }
    public void SwitchAnim()
    {
        mAnim.SetBool("IsWalk",isWalk);
        mAnim.SetBool("IsChase",isChase);
        mAnim.SetBool("IsDead",isDead);  
    }

    public void guardState()
    {
        if (transform.position != guardCenter)
        {
            mAgent.destination = guardCenter;
            //TODO sqrmagnitude
            if (Vector3.SqrMagnitude(transform.position - guardCenter) <= mAgent.stoppingDistance)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, guardRatation, 0.01f);
            }
        }
    }
    
    public void PatrolState()
    {
        if (Vector3.Distance(transform.position, wayPoint) <= mAgent.stoppingDistance                )
        {
            //Debug.Log("到达目的地");
            isWalk = false;
            if (mWayFindingInterval >= 0)
            {
                mWayFindingInterval -= Time.deltaTime;
            }
            else
            {
                isWalk = true;
                CreateWayPoint();
                mAgent.destination = wayPoint;
            }
            if (patrolWaitTime >= 0)
            {
                patrolWaitTime -= Time.deltaTime;
            }
            else
            {
                patrolWaitTime = afterPatrolWaitTime;
                //Debug.Log("创建新的路径");
                CreateWayPoint();
            }
        }
        else
        {
            isWalk = true;
            mAgent.destination = wayPoint;
        }
    }

    public void BlindState()
    {
        Debug.Log("进入blind状态");
        isBlind = true;
        isWalk = false;
        isChase = false;
        if (blindTime >= 0)
        {
            blindTime -= Time.deltaTime;
        }
        else
        {
            isBlind = false;
            blindTime = blindWaitTime;
        }
    }
    public void ChaseState()
    {
        isWalk = false;
        isChase = true;
        mAgent.speed = cahseSpeed;
        if (!foundPlayer())
        {
            isChase = false;
            if (ChaseWaitTime >= 0)
            {
                //计算时间
                ChaseWaitTime -= Time.deltaTime;
            }
            else
            {
                //切换进入normal状态
                if (isGuard)
                {
                    mEnemyEnum = EnemyEnum.GUARD;
                }
                else
                { 
                    mEnemyEnum = EnemyEnum.PATROL;
                }
                //充值chase之后等待的时间
                ChaseWaitTime = aftertChaseWaitTime;

            }
        }
        else
        {
            //Debug.Log("与玩家的距离"+Vector3.Distance(transform.position, mAttackTarget.transform.position));
            if (Vector3.Distance(transform.position, mAttackTarget.transform.position) >=shootRadius)
            {
                //Debug.Log("未进入射程");
                mAgent.destination = mAttackTarget.transform.position;
            }
            else
            {
                //Debug.Log("进入射程");
                isChase = false;
                mAgent.destination =transform.position;
                transform.LookAt(mAttackTarget.transform);
                if (isSingleShoot)
                {
                    SingleShoot();
                }
                else
                {
                    ThreeShoot();
                }
            }
        }
    }
    public void DeadState()
    {
        Debug.Log("进入死亡状态");
        isDead = true;
        Destroy(gameObject,2f);
    }

    public void SingleShoot()
    {
        if (Time.time - lastShootTime > 1 / shootRate)
        {
            Ray ray = new Ray(transform.position,mAttackTarget.transform.position);
            int layer1 = LayerMask.NameToLayer("Map");
            int layer2 = LayerMask.NameToLayer("Player");
            int rayLayer = (1 << layer1) | (1 << layer2);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, shootRadius, rayLayer))
            {
                if (!hit.collider.CompareTag("Player"))
                {
                    return;
                }
            }
            lastShootTime = Time.time;
            mAudioSource.clip = shootClip;
            mAudioSource.Play();
            mAnim.Play("Fire",1,0f);
            if (Random.Range(0, 1) <= hitRate)
            {
                GameManager.Instance.CharacterState.CurrHealth -= enemyState.Damage;
            }
        }
    }
    public void ThreeShoot()
    {
        
    }

    public void setBlindState()
    {
        isBlind = true;
        mEnemyEnum = EnemyEnum.Blind;
    }
    public void CreateWayPoint()
    {
        float wayX = UnityEngine.Random.Range(-patrolRadius, patrolRadius);
        float wayZ = UnityEngine.Random.Range(-patrolRadius, patrolRadius);
        Vector3 newWayPoint = new Vector3(guardCenter.x + wayX, transform.position.y, guardCenter.z + wayZ);
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(newWayPoint, out hit, patrolRadius, 1) ? newWayPoint : transform.position;
        //Debug.Log("新目的地"+wayPoint);

    }
    //寻找
    bool foundPlayer()
    {
        if (isGameOver) return false;
        if (isBlind) return false;
        int layer = LayerMask.NameToLayer("Player");
        //用一个收集器 在视线范围内所有物体都会进入到var里面 var可以存任何类型object
        var colliders = Physics.OverlapSphere(transform.position, sightRadius,1<<layer);

        //遍历
        foreach (var target in colliders)
        {
            //如果有玩家
            if (target.CompareTag("Player"))
            {
                //设置攻击目标
                mAttackTarget = target.gameObject.GetComponent<PlayerControl>();
                //返回true
                return true;
            }
        }
        return false;
    }
    
    public void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.blue;
        Gizmos.DrawWireSphere(transform.position,sightRadius);
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position,shootRadius);
    }

    public void RegisterAndInit()
    {
        GameManager.Instance.RegisterEnemy(this);
    }
    //游戏结束执行的动作
    public void OverExecute()
    {
        Debug.Log("gameover+ name: "+gameObject.name);
        isGameOver = true;
        mAttackTarget = null;
        mEnemyEnum = EnemyEnum.GUARD;
    }

    private void OnEnable()
    {
        enemyState.OnHealthChanged += OnHealthChanged;
        if(GameManager.isInitialized())
            GameManager.Instance.RegisterEnemy(this);
    }

    private void OnDestroy()
    {
        enemyState.OnHealthChanged -= OnHealthChanged;
        if(GameManager.isInitialized())
            GameManager.Instance.UnRegisterEnemy(this);
    }

}
