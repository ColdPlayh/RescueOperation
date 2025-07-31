using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;
using Weapon;

public class UIManager : SingleTon<UIManager>
{
    [Header("Canvas Setting")]
    //主武器
    private static MyWeapon MainWeapon;
    //副武器
    private static MyWeapon SecondaryWeapon;

    public Canvas mainCanvas;
    //角色控制画布
    public Canvas playerControlCanvas;
    //角色选择物品武器画布
    public Canvas playerAndWeaponCanvas;
    //设置画布
    public Canvas settingCanvas;
    //开镜画布
    public Canvas scopeCanvas;
    //其他东西画布
    public Canvas otherCanvas;
    //游戏胜利画布
    //拾取物品panel
    public GameObject PickUpPanel;
    //拾取物品按钮
    public Button PickUpButton;
    
    
    
    
    
    [Header("Weapon UI")]
    //主武器的子弹信息
    private Text MainWeaponText;
    //副武器的子弹信息
    private Text SecondaryWeaponText;

    //主武器图标
    public Image mainWeaponImage;
    //副武器图标
    public Image secondaryWeaponImage;

    [Header("Shoot Setting")]
    //瞄准点
    public Image aimPoint;
    //ui相机
    public Camera UICamera;

    //药物下拉框
    private Dropdown medicalDropDown;
    //投掷物下拉框
    private Dropdown throwableDropDown;
    //投掷按钮
    private Button throwBtn;
    //使用药物按钮
    private Button medicalBtn;
    //缩放的active
    private bool scopeCanvasActive;
    //是否缩放
    private bool isScope;
    //开镜ui
    private Animator scopeCanvasAnim;
    //伤害的text
    private string demageText;

    private bool isProcessed=false;

    [Header("Instantiate Prefab")] 
    //游戏结束
    public GameObject GameOverCanvas;
    //进度条
    public Image circleProcessBar;
    //解救人质按钮
    public Button rescueHostageBtn;

    public GameObject PauseCanvas;

    public VictoryCanvas victoryCanvas;

    public TipText tipText;

    [Header("Scope Setting")] 
    public float noScopeFov;

    public float ScopeFov = 5f;
    

    [Header("Fade Setting")] public Image fade;

  
    public string DamageText
    {
        get { return demageText; }
        set
        {
            demageText = value;
        }
    }

    public bool IsProcessed
    {
        get => isProcessed;
        set => isProcessed = value;
    }

    public bool ScopeCanvasActive => scopeCanvasActive;
    protected override void Awake()
    {
        base.Awake();
        
        scopeCanvasAnim = scopeCanvas.GetComponent<Animator>();
        medicalDropDown = playerAndWeaponCanvas.transform.GetChild(0).GetChild(3).GetComponent<Dropdown>();
        throwableDropDown=playerAndWeaponCanvas.transform.GetChild(0).GetChild(2).GetComponent<Dropdown>();

        throwBtn = playerControlCanvas.transform.GetChild(0).GetChild(4).GetComponent<Button>();
        medicalBtn = playerControlCanvas.transform.GetChild(0).GetChild(9).GetComponent<Button>();
        
        
        SecondaryWeaponText = playerAndWeaponCanvas.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetComponent<Text>();
        MainWeaponText = playerAndWeaponCanvas.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<Text>();

        noScopeFov = Camera.main.fieldOfView;
    }
    public void RegisterMainWeapon(MyWeapon weapon)
    {
        MainWeapon = weapon;
        mainWeaponImage.sprite = MainWeapon.uiImage;
        WeaponManager.Instance.MainWeapon = weapon;
        // MainWeapon.WeaponState.currMagazines = MainWeapon.WeaponState.magazines;
        // MainWeapon.WeaponState.currAmmunition = MainWeapon.WeaponState.spareAmmunition;
    }

    public void RegisterSecondaryWeapon(MyWeapon weapon)
    {
        SecondaryWeapon = weapon;
        secondaryWeaponImage.sprite = SecondaryWeapon.uiImage;
        WeaponManager.Instance.SecondaryWeapon = weapon;
        // SecondaryWeapon.WeaponState.currMagazines = SecondaryWeapon.WeaponState.magazines;
        // SecondaryWeapon.WeaponState.currAmmunition = SecondaryWeapon.WeaponState.spareAmmunition;
    }

    public Dropdown MedicalDropDown => medicalDropDown;
    public Dropdown ThrowableDropDown
    {
        get { return throwableDropDown; }
    }
    
    public void ChangeThrowImage()
    {
        throwBtn.image.sprite  = throwableDropDown.options[throwableDropDown.value].image;
    }

    public void ChangeMedicalImage()
    {
        medicalBtn.image.sprite = MedicalDropDown.options[medicalDropDown.value].image;
    }
    public void Update()
    {
        scopeCanvasAnim.SetBool("IsScope",isScope);
        
    }

    public void circleProcess(float time)
    {
        var circleprocess = Instantiate(circleProcessBar.transform, otherCanvas.transform);
        circleprocess.GetComponent<CircleBar>().setTime(time);
    }
    public void InitAmmunition()
    {

        Debug.Log(MainWeaponText+":"+SecondaryWeaponText+":"+MainWeapon);
        MainWeaponText.text = MainWeapon.WeaponState.currMagazines + "/" + MainWeapon.WeaponState.currAmmunition;
        SecondaryWeaponText.text = SecondaryWeapon.WeaponState.currMagazines + "/" + SecondaryWeapon.WeaponState.currAmmunition;
        
    }

    public void ShowScope()
    {
        UICamera.fieldOfView = ScopeFov;
        scopeCanvas.gameObject.SetActive(true);
        scopeCanvasActive = true;
        aimPoint.gameObject.SetActive(false);
        isScope = true;
        //scopeCanvasAnim.Play("ScopeCanvas",1);

    }

    public void HideScope()
    {

        StartCoroutine(HideScopeIe());

    }

    public IEnumerator HideScopeIe()
    {
        isScope = false;
        UICamera.fieldOfView = noScopeFov;
        yield return new WaitForSeconds(0.1f);
        scopeCanvas.gameObject.SetActive(false);
        scopeCanvasActive = false;
        aimPoint.gameObject.SetActive(true);
        
    }
    public void GameOver()
    {
        Instantiate(GameOverCanvas);
    }

    public void ShowPauseCanvas()
    {
        Instantiate(PauseCanvas);
    }

    public void Victory()
    {
        Instantiate(victoryCanvas, otherCanvas.transform);
    }

    public void TipText(string tip, Color color)
    {
        TipText Tipt = Instantiate(tipText,otherCanvas.transform);
        Tipt.Show(tip,color);
    }

}
