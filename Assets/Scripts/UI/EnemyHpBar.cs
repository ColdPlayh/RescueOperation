using System;
using States.MonoBehavior;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
   [FormerlySerializedAs("enemyUIHpBarPreFab")] [Header("HpBar Setting")]
   //血条预制体
   public GameObject UIHpBarPreFab;
   //血条生成点
   public Transform hpBarPonit;
   //受到攻击后多久血条自动消失
   public float hpBarVisibleTime;
   //是否一直显示血条
   public bool isAlwaysVisible;

   //public bool isTower = false;
   //控制血条绿色部分的slider
   private Image healthSlider;
   //血条的trans
   private Transform hpBar;
   //miancamera的trans
   private Transform mainCameraTrans;
   //敌人数据
   private EnemyState enemyState;
   private DefenceBuildingState towerState;
   
   private float timeLeft;
   [FormerlySerializedAs("text")]
   [FormerlySerializedAs("tipText")]
   [FormerlySerializedAs("enemyDeamgeText")]
   [Header("Text Setting")]
   //text预制体
   public GameObject demageText;
   //伤害生成点
   public Transform demageTextPoint;



   private Transform canvasTrans;
   private Transform demageTrans;
      
   public Transform HpBar
   {
      get => hpBar;
   }

   private void Awake()
   {
      /*if (isTower)
      {
         towerState = GetComponent<DefenceBuildingState>();
         towerState.OnHealthChanged += OnHealthChanged;
      }
      else
      {
         enemyState = GetComponent<EnemyState>();
         enemyState.OnHealthChanged += OnHealthChanged;
      }*/
      
      //获取敌人数据
      enemyState = GetComponent<EnemyState>();
      //订阅委托
      enemyState.OnHealthChanged += OnHealthChanged;
   }
   
   private void OnEnable()
   {
      //获得主摄像机的transform
      mainCameraTrans = Camera.main.transform;

      //遍历所有canvas
      foreach (Canvas canvas in FindObjectsOfType<Canvas>())
      {
         //如果rendertype为worldspace
         if (canvas.renderMode==RenderMode.WorldSpace)
         {
            canvasTrans = canvas.transform;
            //生成血条预制件 并获得对应的transfrom
            hpBar = Instantiate(UIHpBarPreFab, canvas.transform).transform;
           
            
            //获得血条的slider
            healthSlider = hpBar.GetChild(0).GetComponent<Image>();
            //设置血条是否一直显示
            hpBar.gameObject.SetActive(isAlwaysVisible);
            
         }
      }
   }

   private void OnDestroy()
   {
      //删除委托
      //enemyState.OnHealthChanged -= OnHealthChanged;
   }
   
   /// <summary>
   /// 当敌人血量发生变化时候发出委托的实现方法
   /// </summary>
   /// <param name="currHealth"></param>
   /// <param name="maxHealth"></param>
   private void OnHealthChanged(int currHealth,int maxHealth)
   {
      //如果滴人死亡 销毁血条
      if (currHealth <= 0)
      {
         if(hpBar)
            Destroy(hpBar.gameObject);
         return;
      }
      //设置显示血条
      hpBar.gameObject.SetActive(true);
      //生成伤害字体预制体 并获得对应的transfrom
      demageTrans=Instantiate(demageText, canvasTrans).transform;
      demageTrans.GetComponent<DemageText>().setText(Constant.TEXT_DAMAGEENEMY,UIManager.Instance.DamageText);
      demageTrans.position = demageTextPoint.position;
      //生成伤害字体预制体 并获得对应的transfrom
      timeLeft = hpBarVisibleTime;
      //血条百分比
      float sliderPrecent=(float)currHealth / maxHealth;
      //更新血条
      healthSlider.fillAmount = sliderPrecent;

   }
   
   public void showStringText()
   {
      demageTrans=Instantiate(demageText, canvasTrans).transform;
      demageText.GetComponent<DemageText>().setText(Constant.TEXT_STRING,"无效");
      demageTrans.position = demageTextPoint.position;
   }

   public void LateUpdate()
   {
      if (hpBar!=null)
      {
         hpBar.position=hpBarPonit.position;
         hpBar.forward = -mainCameraTrans.forward;

         if (timeLeft <= 0 && !isAlwaysVisible)
         {
            hpBar.gameObject.SetActive(false);
         }
         else
         {
            timeLeft -= Time.deltaTime;
         }
      }
   }
}
