using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Manager;
using UnityEngine;
using RootMotion.FinalIK;
using States.MonoBehavior;
using UnityEditor;
using UnityEngine.Serialization;
using Weapon;

public class PlayerControl : MonoBehaviour
{

    //控制设置
    [Header("Control Setting")]
    //移动摇杆
    public Joystick moveJoyStick;

    //视角摇杆
    public Joystick rotateJoyStick;

    [Header("Weapon Setting")]
    //主武器
    private MyWeapon MainWeapon;

    //副武器
    private MyWeapon SecondaryWeapon;

    //当前选择的武器
    private MyWeapon CurrWeapon;

    [Header("Base Setting")]
    //角色数据
    private CharacterState characterState;

    //private WeapoinState weapoinState;
    //跳跃悬空时间
    public float jumpTime;

    //跳跃速度
    public float jumpMoveSpeed;

    //跳跃高度
    public float jumpHeight;

    //血条bar
    public HPBar hpBar;

    public Transform capsuleCenter;

    //胶囊体长度
    private float capsuleRadius;
    private CapsuleCollider capsuleCollider;




    //摄像机设置
    [Header("Caemra Setting")] [SerializeField]
    //摄像机基底
    private PhotoGerpher photoGerpher;

    [SerializeField]
    //摄像机跟随目标
    private Transform cameraFollowTarget;

    //移动速度
    public float cameraMoveSpeed;

    //转换速度
    public float cameraTurnSpeed;

    //射击设置
    [Header("Shoot Settings")]
    //子弹发射点
    public Transform muzzlePoint;

    //子弹粒子特效
    public ParticleSystem muzzleflashParticles;

    //蛋壳特效
    public ParticleSystem casingflashParticles;

    //枪口光效
    public Light muzzleflashLight;

    //手雷设置
    [Header("Bomb Settings")]
    //手雷预制体
    public GameObject bombPrefab;

    //手雷生成点
    public Transform grenadePoint;

    //手雷扔出的速度大小
    [FormerlySerializedAs("velovity")] public float thorwVelovity;

    //动画开始之后多久扔出
    public float thorwTime;

    //抛物线宽度
    public float lineWidth;

    //抛物线的点
    private List<Vector3> Points = new List<Vector3>();

    [HideInInspector]
    //手雷的速度用于给ThrowGrenda脚本赋值
    public static Vector3 VelovityV3;


    //刚体
    private new Rigidbody rigidbody;

    //动画状态机
    private Animator anim;

    //动画判断的bool
    private bool isRun;
    private bool isJump;
    private bool isShoot;

    private bool isThrow;

    //用于判断是否处于开镜
    private bool isScope;

    //判断跳跃是否达到地面
    private bool isGround;

    //判断是否在换子弹
    private bool isReload;

    //判断角色是否死亡
    private bool isDead;

    //角色是否可以移动
    private bool isCanWalk = true;

    private bool isRevocering = false;

    public static bool isCheckV;

    //角色移动脚本的引用用于移动角色
    private CharacterMovement characterMovement;

    //声音
    private AudioSource audioSc;

    //扔手雷的控制脚本
    private ThorwGrenadeControl thorwGrenadeControl;

    //线渲染器
    private LineRenderer lineRenderer;

    //上一次开火的时间
    private float lastFired;

    private Vector3 dir;

    //动画状态机信息
    private AnimatorStateInfo animatorInfo;

    //瞄准时候给子弹的力的方向
    private Vector3 forceDir;

    [HideInInspector]
    //反向动力学控件
    public AimIK aimIK;

    private Camera mainCamera;

    private Animator cameraAnim;

    //当前恢复量
    private int currRecoverValue = 100;

    private Vector3 lastIkPos = Vector3.zero;

    protected void Awake()
    {
        //获取组件
        lineRenderer = GetComponent<LineRenderer>();
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        characterMovement = GetComponent<CharacterMovement>();
        audioSc = GetComponent<AudioSource>();
        aimIK = GetComponent<AimIK>();
        thorwGrenadeControl = GetComponent<ThorwGrenadeControl>();
        characterState = GetComponent<CharacterState>();
        mainCamera = Camera.main;
        cameraAnim = mainCamera.GetComponent<Animator>();

        capsuleRadius = GetComponent<CapsuleCollider>().radius;
        capsuleCollider = GetComponent<CapsuleCollider>();


    }

    void Start()
    {
        Test();
        //注册自己到gamemanager
        GameManager.Instance.RegisterPlayer(this);
        //注册自己的数据
        GameManager.Instance.RegisterCharacterState(characterState);
        //添加委托
        WeaponManager.Instance.onMainChangeAction += onMainWeaponChanged;
        WeaponManager.Instance.onSecondaryChangeAction += onSecondayWeaponChanged;
        GameManager.Instance.CharacterState.onCurrHealthChanged += OnCurrHealthChanged;
        //初始化相机跟随目标
        photoGerpher.InitCamera(cameraFollowTarget);
        //注册玩家

        Debug.Log("初始化");
        //初始化武器系统
        initWeaponSystem();
        //初始化生命
        initHealth();
        //手雷速度的Vector等于扔出手雷点的前面乘上手雷速度
        VelovityV3 = grenadePoint.transform.forward * thorwVelovity;
        //初始化变量
        isGround = true;
        //上一次开火时间
        lastFired = Time.time;
        //初始化闪光粒子特效
        muzzleflashLight.enabled = false;
        //获取animator的信息
        animatorInfo = anim.GetCurrentAnimatorStateInfo(0);
        hideLine();

    }

    public bool IsCanWalk
    {
        get => isCanWalk;
        set => isCanWalk = value;
    }

    public void Test()
    {

    }

    public void onMainWeaponChanged()
    {
        UIManager.Instance.InitAmmunition();
        MainWeapon.gameObject.SetActive(false);
        MainWeapon = WeaponManager.Instance.MainWeapon;
        SelectMainWeapon();
    }

    public void onSecondayWeaponChanged()
    {
        UIManager.Instance.InitAmmunition();
        SecondaryWeapon.gameObject.SetActive(false);
        SecondaryWeapon = WeaponManager.Instance.SecondaryWeapon;
        SelectSecondaryWeapon();
    }

    public void initWeaponSystem()
    {
        //设置主武器
        MainWeapon = WeaponManager.Instance.MainWeapon;
        //设置副武器
        SecondaryWeapon = WeaponManager.Instance.SecondaryWeapon;
        //设置当前武器
        CurrWeapon = MainWeapon;
        //设置角色伤害
        GameManager.Instance.CharacterState.CurrDamage = CurrWeapon.WeaponState.currAttack;
        //注册主武器
        UIManager.Instance.RegisterMainWeapon(MainWeapon);
        //注册副武器
        UIManager.Instance.RegisterSecondaryWeapon(SecondaryWeapon);
        //初始化武器部分ui
        UIManager.Instance.InitAmmunition();

    }

    public void initHealth()
    {
        //初始化角色生命值
        GameManager.Instance.CharacterState.CurrHealth = GameManager.Instance.CharacterState.MaxHealth;
        //设置hpbar的当前生命条
        hpBar.SetHp(GameManager.Instance.CharacterState.CurrHealth);
        //设置最大生命条
        hpBar.SetMaxHp(GameManager.Instance.CharacterState.MaxHealth);
    }

    private void Update()
    {
        //监听动画
        switchAnim();
        //获取左摇杆输入并传送给移动脚本
        UpdateMovementInput();
        if (rotateJoyStick.Vertical != 0 || rotateJoyStick.Horizontal != 0)
        {
            UpdateRotate();
        }

        if (isShoot)
        {
            //射击
            Shooting();
        }

        //瞄准
        rayAim();
        //测试
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("按下了空格");
            GameManager.Instance.CharacterState.CurrHealth -= 20;
        }


    }

    /// <summary>
    /// 对应character中currhealth发出的委托 更新hpbar
    /// </summary>
    public void OnCurrHealthChanged()
    {

        if (GameManager.Instance.CharacterState.CurrHealth <= 0)
        {
            DeadState();
        }

        hpBar.SetHp(GameManager.Instance.CharacterState.CurrHealth);
    }

    public void DeadState()
    {
        isDead = true;
        GameManager.Instance.GameOver();
    }

    //物理计算部分
    private void FixedUpdate()
    {
        if (isDead) return;
        //跳跃
        if (isJump)
        {

            if (isGround)
            {
                Vector3 x = new Vector3(moveJoyStick.Horizontal, moveJoyStick.Vertical, 0);
                //Vector3 y = new Vector3(rotateJoyStick.Horizontal, rotateJoyStick.Vertical, 0);
                GetComponent<Rigidbody>().velocity += Vector3.up * jumpHeight;
                GetComponent<Rigidbody>().AddForce(Vector3.up + x * jumpMoveSpeed);
                isGround = false;
                Debug.Log("I am Pressing Jump");
            }
            else
            {
                if (CheckGround())
                {
                    isJump = false;
                    isGround = true;
                }
            }
        }
    }

    //检测角色是否在地面
    public bool CheckGround()
    {
        Vector3 center = transform.localToWorldMatrix.MultiplyPoint(capsuleCollider.center);
        int intLayer = LayerMask.NameToLayer("Map");
        LayerMask mask = 1 << intLayer;
        Vector3 pointTop = center + (capsuleCollider.height * 0.5f - capsuleRadius) * Vector3.up;
        Vector3 pointButton = center + (capsuleCollider.height * 0.5f - capsuleRadius + 0.01f) * Vector3.down;
        var colliders = Physics.OverlapCapsule(pointButton, pointTop, capsuleRadius, mask);
        Debug.Log("jump" + colliders.Length);
        if (colliders.Length != 0)
        {
            return true;
        }

        return false;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 3);
    }

    //移动角色
    public void UpdateMovementInput()
    {
        if (isDead) return;
        if (!isCanWalk) return;
        if (moveJoyStick.Horizontal != 0 || moveJoyStick.Vertical != 0)
        {
            aimIK.Disable();
            Quaternion rot = Quaternion.Euler(0, photoGerpher.Yaw, 0);
            isRun = true;
            characterMovement.setMovementInput(
                rot * Vector3.forward * moveJoyStick.Vertical
                + rot * Vector3.right * moveJoyStick.Horizontal);
            MiniMapControl.Instance.FollowPlayerPosition();

        }
        else
        {
            characterMovement.setMovementInput(new Vector3(0, 0, 0));
            //transform.rotation = Quaternion.Lerp(transform.rotation,photoGerpher.transform.rotation,1);
            isRun = false;
        }
    }

    //移动视角
    public void UpdateRotate()
    {
        if (rotateJoyStick.Horizontal != 0)
        {
            aimIK.enabled = true;
            //Debug.Log(photoGerpher.transform.rotation.eulerAngles.y);
            VelovityV3 = grenadePoint.transform.forward * thorwVelovity;
            Quaternion quaternion = Quaternion.Euler(0, photoGerpher.transform.rotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, quaternion, Time.fixedDeltaTime * cameraTurnSpeed);
            MiniMapControl.Instance.FollowPlayerRotation();
        }
    }

    //判断动画
    public void switchAnim()
    {
        anim.SetBool("isRun", isRun);
        anim.SetBool("isShoot", isShoot);
        anim.SetBool("isThrow", isThrow);
        anim.SetBool("isDead", isDead);
        cameraAnim.SetBool("IsScope", isScope);
    }

    //瞄准
    public void rayAim()
    {
        if (isDead) return;
        //从屏幕坐标系的准信发出一条射线
        Ray ray = UIManager.Instance.UICamera.ScreenPointToRay(
            //ui坐标系转到视口坐标系
            PositionConvert.UIPointToScreenPoint(
                //传输参数为准心的ui坐标系的坐标
                UIManager.Instance.aimPoint.transform.position));
        //hit
        RaycastHit hit;

       
        if (Physics.Raycast(ray, out hit, 500, (1 << 8)))
        {
            Debug.DrawRay(ray.origin,hit.point,Color.red);
            if (hit.point != lastIkPos)
            {
                //设置反向动力学ik的方向
                aimIK.solver.target.transform.position = hit.point;

            }
            else
            {
                return;
            }

        }
        if (Physics.Raycast(ray, out hit, 500, ~(1 << 3)))
        {
            //子弹力的方向
            forceDir = hit.point - CurrWeapon.muzzlePoint.position;
        }

    }

    //开镜
        public void Scope()
        {
            if (isDead) return;
            if (!UIManager.Instance.ScopeCanvasActive)
            {
                StartCoroutine(ScopeIe());
            }
            else
            {
                UIManager.Instance.HideScope();
                isScope = false;
            }

        }

        //播放开镜动画
        public IEnumerator ScopeIe()
        {
            isScope = true;
            yield return new WaitForSeconds(0.3f);
            UIManager.Instance.ShowScope();
        }

        //换弹
        public void ReLoad()
        {
            if (isDead) return;
            if (!isCanWalk) return;
            //如果当前子弹为0备弹不为0
            if (CurrWeapon.WeaponState.currMagazines == 0
                && CurrWeapon.WeaponState.currAmmunition != 0)
            {
                Debug.Log(anim);

                anim.Play("reload_out", 0, 0);
                audioSc.clip = CurrWeapon.reloadOutClip;
                audioSc.Play();
                //当前备弹大于一梭子子弹
                if (CurrWeapon.WeaponState.currAmmunition >= CurrWeapon.WeaponState.magazines)
                {
                    //当前子弹为30
                    //备弹减少30
                    CurrWeapon.WeaponState.currAmmunition -=
                        CurrWeapon.WeaponState.magazines;
                    CurrWeapon.WeaponState.currMagazines =
                        CurrWeapon.WeaponState.magazines;
                }
                //如果当前备弹小于一梭子子弹
                else
                {
                    //当前子弹等于备弹 
                    //备弹减少为0
                    CurrWeapon.WeaponState.currMagazines = CurrWeapon.WeaponState.currAmmunition;
                    CurrWeapon.WeaponState.currAmmunition = 0;
                }

            }
            //如果当前子弹为30
            else if (CurrWeapon.WeaponState.currMagazines ==
                     CurrWeapon.WeaponState.magazines)
            {
                Debug.Log(anim);
                //播放满弹夹的动画
                anim.Play("reload_ammo", 0, 0);
                audioSc.clip = CurrWeapon.reloadAmmoClip;
                audioSc.Play();

            }
            //如果当前子弹小于三十
            else
            {
                Debug.Log(anim);
                anim.Play("reload_out", 0, 0);
                audioSc.clip = CurrWeapon.reloadOutClip;
                audioSc.Play();
                //计算需要补充的弹药
                int needAmmunition = CurrWeapon.WeaponState.magazines -
                                     CurrWeapon.WeaponState.currMagazines;
                //计算需要的弹药和备弹的差
                int sub = CurrWeapon.WeaponState.currAmmunition -
                          needAmmunition;
                //如果差大于0代表备弹足够
                if (sub >= 0)
                {
                    //设置当前弹夹等于弹夹最大容量
                    CurrWeapon.WeaponState.currMagazines =
                        CurrWeapon.WeaponState.magazines;
                    //减少备弹
                    CurrWeapon.WeaponState.currAmmunition -= needAmmunition;
                }
                else
                {
                    CurrWeapon.WeaponState.currMagazines += CurrWeapon.WeaponState.currAmmunition;
                }

            }

            //刷新ui
            UIManager.Instance.InitAmmunition();


        }

        //跳跃 在ui部分跳跃button的eventtrigger中
        public void Jump()
        {
            if (isDead) return;
            isJump = true;
        }

        //手雷准备扔出 绘制抛物线
        public void ThrowGrendade_Start()
        {
            if (isDead) return;
            isCheckV = true;
            anim.Play("Grenade_Throw_Start", 0, 0);
            Points = thorwGrenadeControl.GetPoints();
            lineRenderer.positionCount = (int) thorwGrenadeControl.pointsCount;
            lineRenderer.SetPositions(Points.ToArray());
            showLine();

        }

        //扔出手雷
        public void ThrowGrendade()
        {
            if (isDead) return;
            isCheckV = false;
            isThrow = true;
            StartCoroutine(GrenadeSpawnDelay());
            anim.Play("grenade_throw_release", 0, 0);
            Points.Clear();
            hideLine();
        }

        //执行扔出手榴弹的携程
        private IEnumerator GrenadeSpawnDelay()
        {
            yield return new WaitForSeconds(thorwTime);
            Instantiate(bombPrefab,
                grenadePoint.transform.position,
                grenadePoint.transform.rotation);
            //取消投掷动画
            isThrow = false;
        }

        //射击的方法
        public void Shooting()
        {
            if (isDead) return;
            //如果没有子弹
            if (CurrWeapon.WeaponState.currMagazines == 0)
            {
                if (CurrWeapon.WeaponState.currAmmunition == 0)
                {
                    //TODO:播放没有子弹发射的音效
                }
                else
                {
                    //强制换弹
                    ReLoad();
                }

                return;
            }

            if (Time.time - lastFired > 1 / CurrWeapon.fireRate)
            {
                lastFired = Time.time;
                audioSc.clip = CurrWeapon.shootClip;
                audioSc.Play();
                anim.Play("Fire", 0, 0f);


                muzzleflashParticles.Emit(1);
                casingflashParticles.Emit(1);
                StartCoroutine(MuzzleflashLight());
                var bullet = Instantiate(
                        CurrWeapon.bulletPrefab,
                        GetComponent<IWeapon>().GetRecoil(CurrWeapon.muzzlePoint),
                        CurrWeapon.muzzlePoint.transform.rotation)
                    .transform;
                bullet.GetComponent<Rigidbody>().velocity =
                    forceDir * CurrWeapon.bulletForce;
                if (CurrWeapon.casingPrefab != null)
                {
                    Instantiate(CurrWeapon.casingPrefab,
                        CurrWeapon.casingPoint.transform.position,
                        CurrWeapon.casingPoint.transform.rotation);

                }


                //更新UI
                CurrWeapon.WeaponState.currMagazines -= 1;
                UIManager.Instance.InitAmmunition();

            }
            else
            {

            }
        }

        //携程 枪口闪光
        IEnumerator MuzzleflashLight()
        {

            muzzleflashLight.enabled = true;
            yield return new WaitForSeconds(0.02f);
            muzzleflashLight.enabled = false;
        }

        //回血drodown的方法
        public void SelectMedical()
        {
            if (isDead) return;
            int option = UIManager.Instance.MedicalDropDown.value;
            switch (option)
            {
                case Constant.MEDICAL_KIT:
                    currRecoverValue = 100;
                    UIManager.Instance.ChangeMedicalImage();
                    break;
                case Constant.MEDICAL_KITS:
                    currRecoverValue = 50;
                    UIManager.Instance.ChangeMedicalImage();
                    break;
                case Constant.MEDICAL_BANDAGE:
                    currRecoverValue = 20;
                    UIManager.Instance.ChangeMedicalImage();
                    break;
            }
        }

        //给角色恢复对应的血量
        public void Recover()
        {
            if (!isRevocering)
                StartCoroutine(Porcess(5f));
        }

        IEnumerator Porcess(float time)
        {
            isRevocering = true;
            UIManager.Instance.circleProcess(5f);
            while (!UIManager.Instance.IsProcessed)
            {
                yield return null;
            }

            if (currRecoverValue > GameManager.Instance.CharacterState.MaxHealth -
                GameManager.Instance.CharacterState.CurrHealth)
            {
                GameManager.Instance.CharacterState.CurrHealth = GameManager.Instance.CharacterState.MaxHealth;
            }
            else
            {
                GameManager.Instance.CharacterState.CurrHealth += currRecoverValue;
            }

            isRevocering = false;
            UIManager.Instance.IsProcessed = false;
            yield break;
        }

        //替换手雷对应的预制体
        public void selectThorableThing()
        {
            if (isDead) return;
            int option = UIManager.Instance.ThrowableDropDown.value;
            switch (option)
            {
                case Constant.THROWABLE_GREDNADE:
                    UIManager.Instance.ChangeThrowImage();
                    bombPrefab = WeaponManager.Instance.Grenade.gameObject;
                    break;
                case Constant.THROWABLE_FLASH:
                    UIManager.Instance.ChangeThrowImage();
                    bombPrefab = WeaponManager.Instance.FlashBang.gameObject;
                    break;
                case Constant.THROWABLE_BURNING:
                    UIManager.Instance.ChangeThrowImage();
                    //bombPrefab=
                    break;
            }
        }



        //长按射击btn
        public void canShoot()
        {
            isShoot = true;
        }

        //离开射击btn
        public void cantShoot()
        {
            isShoot = false;
        }

        //选择主武器
        public void SelectMainWeapon()
        {
            if (isDead) return;
            //currWeaponType = MainWeapon.WeaponType;
            CurrWeapon = MainWeapon;
            GameManager.Instance.CharacterState.CurrDamage = MainWeapon.WeaponState.currAttack;
            Debug.Log("当前主武器伤害" + GameManager.Instance.CharacterState.CurrDamage);
            SecondaryWeapon.gameObject.SetActive(false);
            MainWeapon.gameObject.SetActive(true);
            //:TODO 切换ui信息
            anim.runtimeAnimatorController = MainWeapon.WeaponAnim;

        }

        //选择副武器
        public void SelectSecondaryWeapon()
        {
            if (isDead) return;
            CurrWeapon = SecondaryWeapon;
            GameManager.Instance.CharacterState.CurrDamage = SecondaryWeapon.WeaponState.currAttack;
            Debug.Log("当前副武器伤害" + GameManager.Instance.CharacterState.CurrDamage);
            MainWeapon.gameObject.SetActive(false);
            SecondaryWeapon.gameObject.SetActive(true);
            //:TODO 切换ui信息
            anim.runtimeAnimatorController = SecondaryWeapon.WeaponAnim;

        }

        //隐藏linerender
        public void hideLine()
        {
            lineRenderer.startWidth = 0;
            lineRenderer.endWidth = 0;
        }

        //显示linerender
        public void showLine()
        {
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
        }

        //销毁时注销委托
        private void OnDestroy()
        {
            if (WeaponManager.isInitialized())
            {
                WeaponManager.Instance.onMainChangeAction -= onMainWeaponChanged;
                WeaponManager.Instance.onSecondaryChangeAction -= onSecondayWeaponChanged;
            }

            if (GameManager.isInitialized())
                GameManager.Instance.CharacterState.onCurrHealthChanged -= OnCurrHealthChanged;
        }
    
}
