using System;
using System.Collections;
using  UnityEngine;
using  UnityEngine.UI;
public class DemageText : MonoBehaviour
{

    public int refreshFrame=0;
    public int currFrame;
    public bool haveDestryTime=false;
    
    /// <summary>
    /// 滚动速度
    /// </summary>
    private float speed = 1.5f;
 
    /// <summary>
    /// 计时器
    /// </summary>
    private float timer = 0f;
 
    /// <summary>
    /// 销毁时间
    /// </summary>
    public  float destoryTime = 0.4f;

    private Text demageText;

    private Transform mainCameraTrans;

    private Color currColor;

    private bool isInit=false;

    private void Awake()
    {
        currFrame = 0;
    }

    private void Start()
    {
        if(haveDestryTime)
            Destroy(gameObject,destoryTime);
    }

    public void setText(int type,string text,Color newColor)
    {
        demageText = GetComponent<Text>();
        mainCameraTrans = Camera.main.transform;
        switch (type)
        {
            case Constant.TEXT_RECOVE:
                demageText.color = newColor;
                demageText.text = "+" + text;
                
                break;
            case Constant.TEXT_DAMAGEENEMY:
                demageText.color = newColor;
                demageText.text = "-" + text;
                break;
            case Constant.TEXT_DAMAGETOWER:
                demageText.color = newColor;
                demageText.text = "-" + text;
                break;
            case Constant.TEXT_STRING:
                demageText.color = newColor;
                demageText.text = text;
                break;
        }
        currColor = demageText.color;
        isInit = true;
    }
    public void setText(int type,string text)
    {
        demageText = GetComponent<Text>();
        mainCameraTrans = Camera.main.transform;
        switch (type)
        {
            case Constant.TEXT_RECOVE:
                demageText.color = Color.green;
                demageText.text = "+" + text;
                
                break;
            case Constant.TEXT_DAMAGEENEMY:
                demageText.color = Color.white;
                demageText.text = "-" + text;
                break;
            case Constant.TEXT_DAMAGETOWER:
                demageText.color = Color.red;
                demageText.text = "-" + text;
                break;
            case Constant.TEXT_STRING:
                demageText.color = Color.white;
                demageText.text = text;
                break;
        }
        currColor = demageText.color;
        isInit = true;
    }

    private void Update()
    {
        if(isInit)
            Scroll();
    }

    /// <summary>
    /// 冒泡效果
    /// </summary>
    private void Scroll()
    {
        transform.forward = mainCameraTrans.forward;
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (currColor.a > 0)
        {
            if (currFrame == 0)
            {
                timer += Time.deltaTime;
                //字体缩小
                demageText.fontSize--;
                //字体渐变透明
                demageText.color = new Color(currColor.r,currColor.g,currColor.b,1 - timer);

                currFrame = refreshFrame;
            }
            else
            {
                currFrame--;
            }
           
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}