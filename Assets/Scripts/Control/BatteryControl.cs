using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Manager;
using States.MonoBehavior;
using UnityEditor;
using UnityEngine;

public class BatteryControl : MonoBehaviour,IGameOver,IEnemy
{
    public enum BatteryEnum
    {
        PATROL,ATTACK,DEAD
    }

    [Header("Patrol Setting")] 
    //可视范围
    public float sightRadius;

    private EnemyState enemyState;
    //原始角度
    private Quaternion orginalRotation;

    [Header("Attack Setting")] 
    //射速
    public float fireRate;
    //line宽度
    public float lineWidth;
    //子弹预制体
    public GameObject bulletPreFab;
    //子弹发射的力
    public float bulletForce;
    //多久攻击
    public float powerStorageTime;
    //渲染多少个点line
    public float pointCount;



    //linerender
    private LineRenderer lineRenderer;
    //储存linerender的点
    private List<Vector3> pointsList=new List<Vector3>();
    //起始点
    private Vector3 startPos;
    //末尾点
    private Vector3 endPos;
    //上一次的末尾点
    private Vector3 lastEndPos;
    //生成子弹的位置
    private Transform shootPoint;
    
    //状态枚举
    private BatteryEnum mBatteryEnum;
    //头部对象
    private GameObject batteryHead;
    //攻击目标
    private PlayerControl mAttackTarget;
    //私有多久攻击
    private float mPowerStorageTime;
    //游戏是否结束
    private bool isGameOver;
    
    private void Awake()
    {
        batteryHead = transform.GetChild(1).gameObject;
        shootPoint = transform.GetChild(1).GetChild(1).GetChild(1).transform;
        lineRenderer = GetComponent<LineRenderer>();
        enemyState = GetComponent<EnemyState>();
        
        orginalRotation = batteryHead.transform.rotation;
        mPowerStorageTime = powerStorageTime;

    }

    private void Start()
    {
        //隐藏line
        HideLine();
        //初始化
        init();
    }

    public void init()
    {
        enemyState.CurrHealth = enemyState.MaxHealth;
        GameManager.Instance.RegisterEnemy(this);
    }
    private void Update()
    {
     
        //更新状态
        switch (mBatteryEnum)
        {
            case BatteryEnum.PATROL:
                PatrolState();
                break;
            case BatteryEnum.ATTACK:
                AttackState();
                break;
            case BatteryEnum.DEAD:
                break;
        }
    }

    //守卫
    public void PatrolState()
    {
        Restore();
        if (foundPlayer())
        {
            mBatteryEnum = BatteryEnum.ATTACK;
        }
    }

    public void AttackState()
    {
        
        startPos = shootPoint.position;
        endPos = mAttackTarget.transform.GetChild(7).transform.position;
        Ray ray = new Ray(startPos,endPos-startPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                Debug.Log("resotre");
                Restore();
            }
            else
            {
                PowerStoreageAttack(startPos,endPos);
            }
        }

        if (!foundPlayer())
        {
            mBatteryEnum = BatteryEnum.PATROL;
        }
    }
    public void PowerStoreageAttack(Vector3 startPoint,Vector3 endPoint)
    {
        
        lastEndPos = endPoint;
        batteryHead.transform.LookAt(mAttackTarget.transform);
        batteryHead.transform.GetChild(1).forward = endPos-batteryHead.transform.GetChild(1).position;
        
        lineRenderer.positionCount = (int) pointCount;
        
        lineRenderer.SetPositions(GetPoints(startPoint,endPoint).ToArray());
        
        lineRenderer.startWidth = lineWidth;
        
        lineRenderer.endWidth = lineWidth;

        if (mPowerStorageTime > 0)
        {
            mPowerStorageTime -= Time.deltaTime;
        }
        else
        {
            var bullet=Instantiate(bulletPreFab,shootPoint.position,shootPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = (endPoint - startPoint).normalized * bulletForce;
            mPowerStorageTime = powerStorageTime;
        }


    }
    public void Restore()
    {
        HideLine();
        mPowerStorageTime = powerStorageTime;
        batteryHead.transform.rotation = Quaternion.Lerp(batteryHead.transform.rotation , orginalRotation, 0.01f);
        
    }
    bool foundPlayer()
    {

        if (isGameOver) return false;
        //用一个收集器 在视线范围内所有物体都会进入到var里面 var可以存任何类型object
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        //遍历
        foreach (var target in colliders)
        {
            //如果有玩家
            if (target.CompareTag("Player"))
            {
                Debug.Log("找到玩家");
                //设置攻击目标
                mAttackTarget = target.gameObject.GetComponent<PlayerControl>();
                //返回true
                return true;
            }
        }
        return false;
    }
    //获取linerender的点
    public List<Vector3> GetPoints(Vector3 startPoint,Vector3 endPoint)
    { 
        
        
        pointsList.Clear();
        for (int i = 0; i < pointCount; i++)
        {
          
            if (i == pointCount - 1)
            {
                pointsList.Add(endPos);
            }
            else
            {
                pointsList.Add(Vector3.Lerp(startPoint, endPoint,i/pointCount));
            }
        }
        return pointsList;
    }
    //隐藏linerender
    public void HideLine()
    {
        lineRenderer.startWidth = 0;
        lineRenderer.endWidth = 0;
    }
    //显示linerender
    
    public void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.blue;
        Gizmos.DrawWireSphere(transform.position,sightRadius);
    }

    public void OverExecute()
    {
        Debug.Log("gameover+ name: "+gameObject.name);
        isGameOver = true;
        mAttackTarget = null;
        HideLine();
        mBatteryEnum = BatteryEnum.PATROL;
    }

    public void RegisterAndInit()
    {
        GameManager.Instance.RegisterEnemy(this);
    }
    private void OnEnable()
    {
        enemyState.OnHealthChanged += OnHealthChanged;
        if(GameManager.isInitialized())
            GameManager.Instance.RegisterEnemy(this);
    }

    private void OnHealthChanged(int currHealth, int maxHealth)
    {
        if (currHealth == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(GameManager.isInitialized())
        GameManager.Instance.UnRegisterEnemy(this);
    }
}
