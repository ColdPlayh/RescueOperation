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

    //��������
    [Header("Control Setting")]
    //�ƶ�ҡ��
    public Joystick moveJoyStick;

    //�ӽ�ҡ��
    public Joystick rotateJoyStick;

    [Header("Weapon Setting")]
    //������
    private MyWeapon MainWeapon;

    //������
    private MyWeapon SecondaryWeapon;

    //��ǰѡ�������
    private MyWeapon CurrWeapon;

    [Header("Base Setting")]
    //��ɫ����
    private CharacterState characterState;

    //private WeapoinState weapoinState;
    //��Ծ����ʱ��
    public float jumpTime;

    //��Ծ�ٶ�
    public float jumpMoveSpeed;

    //��Ծ�߶�
    public float jumpHeight;

    //Ѫ��bar
    public HPBar hpBar;

    public Transform capsuleCenter;

    //�����峤��
    private float capsuleRadius;
    private CapsuleCollider capsuleCollider;




    //���������
    [Header("Caemra Setting")] [SerializeField]
    //���������
    private PhotoGerpher photoGerpher;

    [SerializeField]
    //���������Ŀ��
    private Transform cameraFollowTarget;

    //�ƶ��ٶ�
    public float cameraMoveSpeed;

    //ת���ٶ�
    public float cameraTurnSpeed;

    //�������
    [Header("Shoot Settings")]
    //�ӵ������
    public Transform muzzlePoint;

    //�ӵ�������Ч
    public ParticleSystem muzzleflashParticles;

    //������Ч
    public ParticleSystem casingflashParticles;

    //ǹ�ڹ�Ч
    public Light muzzleflashLight;

    //��������
    [Header("Bomb Settings")]
    //����Ԥ����
    public GameObject bombPrefab;

    //�������ɵ�
    public Transform grenadePoint;

    //�����ӳ����ٶȴ�С
    [FormerlySerializedAs("velovity")] public float thorwVelovity;

    //������ʼ֮�����ӳ�
    public float thorwTime;

    //�����߿��
    public float lineWidth;

    //�����ߵĵ�
    private List<Vector3> Points = new List<Vector3>();

    [HideInInspector]
    //���׵��ٶ����ڸ�ThrowGrenda�ű���ֵ
    public static Vector3 VelovityV3;


    //����
    private new Rigidbody rigidbody;

    //����״̬��
    private Animator anim;

    //�����жϵ�bool
    private bool isRun;
    private bool isJump;
    private bool isShoot;

    private bool isThrow;

    //�����ж��Ƿ��ڿ���
    private bool isScope;

    //�ж���Ծ�Ƿ�ﵽ����
    private bool isGround;

    //�ж��Ƿ��ڻ��ӵ�
    private bool isReload;

    //�жϽ�ɫ�Ƿ�����
    private bool isDead;

    //��ɫ�Ƿ�����ƶ�
    private bool isCanWalk = true;

    private bool isRevocering = false;

    public static bool isCheckV;

    //��ɫ�ƶ��ű������������ƶ���ɫ
    private CharacterMovement characterMovement;

    //����
    private AudioSource audioSc;

    //�����׵Ŀ��ƽű�
    private ThorwGrenadeControl thorwGrenadeControl;

    //����Ⱦ��
    private LineRenderer lineRenderer;

    //��һ�ο����ʱ��
    private float lastFired;

    private Vector3 dir;

    //����״̬����Ϣ
    private AnimatorStateInfo animatorInfo;

    //��׼ʱ����ӵ������ķ���
    private Vector3 forceDir;

    [HideInInspector]
    //������ѧ�ؼ�
    public AimIK aimIK;

    private Camera mainCamera;

    private Animator cameraAnim;

    //��ǰ�ָ���
    private int currRecoverValue = 100;

    private Vector3 lastIkPos = Vector3.zero;

    protected void Awake()
    {
        //��ȡ���
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
        //ע���Լ���gamemanager
        GameManager.Instance.RegisterPlayer(this);
        //ע���Լ�������
        GameManager.Instance.RegisterCharacterState(characterState);
        //���ί��
        WeaponManager.Instance.onMainChangeAction += onMainWeaponChanged;
        WeaponManager.Instance.onSecondaryChangeAction += onSecondayWeaponChanged;
        GameManager.Instance.CharacterState.onCurrHealthChanged += OnCurrHealthChanged;
        //��ʼ���������Ŀ��
        photoGerpher.InitCamera(cameraFollowTarget);
        //ע�����

        Debug.Log("��ʼ��");
        //��ʼ������ϵͳ
        initWeaponSystem();
        //��ʼ������
        initHealth();
        //�����ٶȵ�Vector�����ӳ����׵��ǰ����������ٶ�
        VelovityV3 = grenadePoint.transform.forward * thorwVelovity;
        //��ʼ������
        isGround = true;
        //��һ�ο���ʱ��
        lastFired = Time.time;
        //��ʼ������������Ч
        muzzleflashLight.enabled = false;
        //��ȡanimator����Ϣ
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
        //����������
        MainWeapon = WeaponManager.Instance.MainWeapon;
        //���ø�����
        SecondaryWeapon = WeaponManager.Instance.SecondaryWeapon;
        //���õ�ǰ����
        CurrWeapon = MainWeapon;
        //���ý�ɫ�˺�
        GameManager.Instance.CharacterState.CurrDamage = CurrWeapon.WeaponState.currAttack;
        //ע��������
        UIManager.Instance.RegisterMainWeapon(MainWeapon);
        //ע�ḱ����
        UIManager.Instance.RegisterSecondaryWeapon(SecondaryWeapon);
        //��ʼ����������ui
        UIManager.Instance.InitAmmunition();

    }

    public void initHealth()
    {
        //��ʼ����ɫ����ֵ
        GameManager.Instance.CharacterState.CurrHealth = GameManager.Instance.CharacterState.MaxHealth;
        //����hpbar�ĵ�ǰ������
        hpBar.SetHp(GameManager.Instance.CharacterState.CurrHealth);
        //�������������
        hpBar.SetMaxHp(GameManager.Instance.CharacterState.MaxHealth);
    }

    private void Update()
    {
        //��������
        switchAnim();
        //��ȡ��ҡ�����벢���͸��ƶ��ű�
        UpdateMovementInput();
        if (rotateJoyStick.Vertical != 0 || rotateJoyStick.Horizontal != 0)
        {
            UpdateRotate();
        }

        if (isShoot)
        {
            //���
            Shooting();
        }

        //��׼
        rayAim();
        //����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("�����˿ո�");
            GameManager.Instance.CharacterState.CurrHealth -= 20;
        }


    }

    /// <summary>
    /// ��Ӧcharacter��currhealth������ί�� ����hpbar
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

    //������㲿��
    private void FixedUpdate()
    {
        if (isDead) return;
        //��Ծ
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

    //����ɫ�Ƿ��ڵ���
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

    //�ƶ���ɫ
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

    //�ƶ��ӽ�
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

    //�ж϶���
    public void switchAnim()
    {
        anim.SetBool("isRun", isRun);
        anim.SetBool("isShoot", isShoot);
        anim.SetBool("isThrow", isThrow);
        anim.SetBool("isDead", isDead);
        cameraAnim.SetBool("IsScope", isScope);
    }

    //��׼
    public void rayAim()
    {
        if (isDead) return;
        //����Ļ����ϵ��׼�ŷ���һ������
        Ray ray = UIManager.Instance.UICamera.ScreenPointToRay(
            //ui����ϵת���ӿ�����ϵ
            PositionConvert.UIPointToScreenPoint(
                //�������Ϊ׼�ĵ�ui����ϵ������
                UIManager.Instance.aimPoint.transform.position));
        //hit
        RaycastHit hit;

       
        if (Physics.Raycast(ray, out hit, 500, (1 << 8)))
        {
            Debug.DrawRay(ray.origin,hit.point,Color.red);
            if (hit.point != lastIkPos)
            {
                //���÷�����ѧik�ķ���
                aimIK.solver.target.transform.position = hit.point;

            }
            else
            {
                return;
            }

        }
        if (Physics.Raycast(ray, out hit, 500, ~(1 << 3)))
        {
            //�ӵ����ķ���
            forceDir = hit.point - CurrWeapon.muzzlePoint.position;
        }

    }

    //����
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

        //���ſ�������
        public IEnumerator ScopeIe()
        {
            isScope = true;
            yield return new WaitForSeconds(0.3f);
            UIManager.Instance.ShowScope();
        }

        //����
        public void ReLoad()
        {
            if (isDead) return;
            if (!isCanWalk) return;
            //�����ǰ�ӵ�Ϊ0������Ϊ0
            if (CurrWeapon.WeaponState.currMagazines == 0
                && CurrWeapon.WeaponState.currAmmunition != 0)
            {
                Debug.Log(anim);

                anim.Play("reload_out", 0, 0);
                audioSc.clip = CurrWeapon.reloadOutClip;
                audioSc.Play();
                //��ǰ��������һ�����ӵ�
                if (CurrWeapon.WeaponState.currAmmunition >= CurrWeapon.WeaponState.magazines)
                {
                    //��ǰ�ӵ�Ϊ30
                    //��������30
                    CurrWeapon.WeaponState.currAmmunition -=
                        CurrWeapon.WeaponState.magazines;
                    CurrWeapon.WeaponState.currMagazines =
                        CurrWeapon.WeaponState.magazines;
                }
                //�����ǰ����С��һ�����ӵ�
                else
                {
                    //��ǰ�ӵ����ڱ��� 
                    //��������Ϊ0
                    CurrWeapon.WeaponState.currMagazines = CurrWeapon.WeaponState.currAmmunition;
                    CurrWeapon.WeaponState.currAmmunition = 0;
                }

            }
            //�����ǰ�ӵ�Ϊ30
            else if (CurrWeapon.WeaponState.currMagazines ==
                     CurrWeapon.WeaponState.magazines)
            {
                Debug.Log(anim);
                //���������еĶ���
                anim.Play("reload_ammo", 0, 0);
                audioSc.clip = CurrWeapon.reloadAmmoClip;
                audioSc.Play();

            }
            //�����ǰ�ӵ�С����ʮ
            else
            {
                Debug.Log(anim);
                anim.Play("reload_out", 0, 0);
                audioSc.clip = CurrWeapon.reloadOutClip;
                audioSc.Play();
                //������Ҫ����ĵ�ҩ
                int needAmmunition = CurrWeapon.WeaponState.magazines -
                                     CurrWeapon.WeaponState.currMagazines;
                //������Ҫ�ĵ�ҩ�ͱ����Ĳ�
                int sub = CurrWeapon.WeaponState.currAmmunition -
                          needAmmunition;
                //��������0�������㹻
                if (sub >= 0)
                {
                    //���õ�ǰ���е��ڵ����������
                    CurrWeapon.WeaponState.currMagazines =
                        CurrWeapon.WeaponState.magazines;
                    //���ٱ���
                    CurrWeapon.WeaponState.currAmmunition -= needAmmunition;
                }
                else
                {
                    CurrWeapon.WeaponState.currMagazines += CurrWeapon.WeaponState.currAmmunition;
                }

            }

            //ˢ��ui
            UIManager.Instance.InitAmmunition();


        }

        //��Ծ ��ui������Ծbutton��eventtrigger��
        public void Jump()
        {
            if (isDead) return;
            isJump = true;
        }

        //����׼���ӳ� ����������
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

        //�ӳ�����
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

        //ִ���ӳ����񵯵�Я��
        private IEnumerator GrenadeSpawnDelay()
        {
            yield return new WaitForSeconds(thorwTime);
            Instantiate(bombPrefab,
                grenadePoint.transform.position,
                grenadePoint.transform.rotation);
            //ȡ��Ͷ������
            isThrow = false;
        }

        //����ķ���
        public void Shooting()
        {
            if (isDead) return;
            //���û���ӵ�
            if (CurrWeapon.WeaponState.currMagazines == 0)
            {
                if (CurrWeapon.WeaponState.currAmmunition == 0)
                {
                    //TODO:����û���ӵ��������Ч
                }
                else
                {
                    //ǿ�ƻ���
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


                //����UI
                CurrWeapon.WeaponState.currMagazines -= 1;
                UIManager.Instance.InitAmmunition();

            }
            else
            {

            }
        }

        //Я�� ǹ������
        IEnumerator MuzzleflashLight()
        {

            muzzleflashLight.enabled = true;
            yield return new WaitForSeconds(0.02f);
            muzzleflashLight.enabled = false;
        }

        //��Ѫdrodown�ķ���
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

        //����ɫ�ָ���Ӧ��Ѫ��
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

        //�滻���׶�Ӧ��Ԥ����
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



        //�������btn
        public void canShoot()
        {
            isShoot = true;
        }

        //�뿪���btn
        public void cantShoot()
        {
            isShoot = false;
        }

        //ѡ��������
        public void SelectMainWeapon()
        {
            if (isDead) return;
            //currWeaponType = MainWeapon.WeaponType;
            CurrWeapon = MainWeapon;
            GameManager.Instance.CharacterState.CurrDamage = MainWeapon.WeaponState.currAttack;
            Debug.Log("��ǰ�������˺�" + GameManager.Instance.CharacterState.CurrDamage);
            SecondaryWeapon.gameObject.SetActive(false);
            MainWeapon.gameObject.SetActive(true);
            //:TODO �л�ui��Ϣ
            anim.runtimeAnimatorController = MainWeapon.WeaponAnim;

        }

        //ѡ������
        public void SelectSecondaryWeapon()
        {
            if (isDead) return;
            CurrWeapon = SecondaryWeapon;
            GameManager.Instance.CharacterState.CurrDamage = SecondaryWeapon.WeaponState.currAttack;
            Debug.Log("��ǰ�������˺�" + GameManager.Instance.CharacterState.CurrDamage);
            MainWeapon.gameObject.SetActive(false);
            SecondaryWeapon.gameObject.SetActive(true);
            //:TODO �л�ui��Ϣ
            anim.runtimeAnimatorController = SecondaryWeapon.WeaponAnim;

        }

        //����linerender
        public void hideLine()
        {
            lineRenderer.startWidth = 0;
            lineRenderer.endWidth = 0;
        }

        //��ʾlinerender
        public void showLine()
        {
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
        }

        //����ʱע��ί��
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
