using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollingUI : MonoBehaviour
{
    [Header("Base Setting")]
    //父物体
    public RectTransform parentTrans;
    //存放所有子物体
    public RectTransform[] itemRectTransforms;
    //字典 
    public Dictionary<RectTransform, Vector3> itemDic = new Dictionary<RectTransform, Vector3>();
    //每一个rect的width和height
    public float rectWidth;
    public float rectHeight;
    
    [Header("Circle Setting")]
    //圆心
    public Vector3 circleCenterPos=Vector3.zero;
    //半径
    public float radius = 5;
    //y的offset
    public float offsetYVlaue=20f;

    [Range(0.8f,1f)]
    public float maxAlpha = 1;
    [Range(0.1f,0.6f)]
    public float minAlpha = 0.2f;
    
    public float rollingSpeed=1f;
    
    [Header("Level Setting")] 
    public Texture notHaveTexture;
    private Coroutine currCoroutine;
    [HideInInspector]
    public RectTransform currRect;
    public Text informationText;

    void Start()
    {
        Init();
        setLevelImage();
        SetCurrInformation();
        setItemsWidthAndHeight();
        setItemsPos();
        setItemsAlpha();
        
    }

    private void Update()
    {
        //Test();   
    }
    
    public void Test()
    {
        setItemsWidthAndHeight();
    }
    
    public void LeftBtnOnClick()
    {
        StartCoroutine(MoveLeft());
//        Debug.Log("select"+currRect.GetComponent<CheckPointState>().IsOpen);
        
        
    }

    public void RightBtnOnClick()
    {
        StartCoroutine(MoveRight());
//        Debug.Log("select"+currRect.GetComponent<CheckPointState>().IsOpen);
    }

    IEnumerator MoveLeft()
    {
        if (currCoroutine != null)
        {
            yield return currCoroutine;
        }
        //由于第一个会先移动所以最后一个移动的时候位置会出现问题 所以记录第一个位置
        Vector3 startPos=itemDic[itemRectTransforms[0]];
        for (int i = 0; i < itemRectTransforms.Length; i++)
        {
            //得到下一个位置
            Vector3 nextPos= itemRectTransforms[(i + 1 )% itemRectTransforms.Length].anchoredPosition3D;;
            //如果是最后一个
            if (i == itemRectTransforms.Length - 1)
            {
                nextPos = startPos;
            }
            //传当前的recttrans 和下一个rect的pos
            currCoroutine=StartCoroutine(MoveToTarget(itemRectTransforms[i], nextPos));
        }
    }
    IEnumerator MoveRight()
    {
        if (currCoroutine != null)
        {
            yield return currCoroutine;
        }
        //由于第一个会先移动所以最后一个移动的时候位置会出现问题 所以记录第一个位置
        Vector3 startPos=itemDic[itemRectTransforms[itemRectTransforms.Length-1]];
        for (int i = itemRectTransforms.Length-1; i >=0; i--)
        {
            //得到下一个位置
            Vector3 nextPos= itemRectTransforms[(i+itemRectTransforms.Length-1 )% itemRectTransforms.Length].anchoredPosition3D;;
            //如果是最后一个
            if (i == 0)
            {
                nextPos = startPos;
            }
            //传当前的recttrans 和下一个rect的pos
            currCoroutine=StartCoroutine(MoveToTarget(itemRectTransforms[i], nextPos));
        }
    }
    //移动携程
    IEnumerator MoveToTarget(RectTransform rect,Vector3 nextPos)
    {
        var distance = (nextPos - rect.anchoredPosition3D).magnitude;
        while (rect.anchoredPosition3D!=nextPos)
        {
            rect.anchoredPosition3D=Vector3.MoveTowards(
                rect.anchoredPosition3D, nextPos,
                Time.deltaTime*distance*rollingSpeed);
            
            yield return null;
        }
        //移动完成
        itemDic[rect] = nextPos;
        //后续的操作跟当前的逻辑冲突的情况
        yield return null;
        setItemsSiblingIndex();
        setItemsAlpha();
        SetCurrInformation();
        

    }

    
    public void Init()
    {
        if (parentTrans == null)
            return;
        List<RectTransform> temp = new List<RectTransform>();
        //获取自身和子物体的所有组件
        var rects=parentTrans.GetComponentsInChildren<RectTransform>();
        itemRectTransforms = new RectTransform[rects.Length - 1];
        //剔除子物体
        for (int i = 0; i < itemRectTransforms.Length; i++)
        {
            if (rects[i].gameObject.CompareTag("Level"))
            {
                temp.Add(rects[i]);
            }
        }

        itemRectTransforms = temp.ToArray();

        currRect = itemRectTransforms[0];
    }

    public void setItemsWidthAndHeight()
    {
        foreach (var item in itemRectTransforms)
        {
            item.sizeDelta = new Vector2(rectWidth, rectHeight);
        }
    }
    
    public void setItemsPos()
    {
        float angle = 0;
        for (int i = 0; i < itemRectTransforms.Length; i++)
        {
            //计算角度 代码是按照面前第一个开始
            angle = i*360f / itemRectTransforms.Length;
            //角度换弧度
            float radian = (angle/180) * Mathf.PI;
            //正余弦值    
            float sinValue = Mathf.Sin(radian) * radius;
            float cosValue = Mathf.Cos(radian) * radius;
            //获取目标位置
            Vector3 tartgetPos = circleCenterPos + new Vector3(sinValue, 0, -cosValue);

            if (i != 0)
            {
                
                if (i > itemRectTransforms.Length / 2)
                {
                    tartgetPos.y = (itemRectTransforms.Length - i) * offsetYVlaue;
                }
                else if (i == itemRectTransforms.Length / 2)
                {
                    tartgetPos.y = itemRectTransforms[0].anchoredPosition.y;
                }
                else
                {
                    tartgetPos.y = offsetYVlaue * i;
                }
            }
            //设置对应位置
            itemRectTransforms[i].anchoredPosition3D = tartgetPos;
            //加入字典
            itemDic.Add(itemRectTransforms[i],tartgetPos);
        }
        //设置优先级
        setItemsSiblingIndex();
    }

    public void setItemsSiblingIndex()
    {
        Dictionary<RectTransform, int> orderDic = new Dictionary<RectTransform, int>();
        for (int i = 0; i < itemDic.Count; i++)
        {
            RectTransform thisRect = new RectTransform();
            float maxValue = float.MinValue;
            //找到当前最大的
            foreach (var item in itemDic)
            {
                //排除之前找到的
                if (!orderDic.ContainsKey(item.Key))
                {
                    //寻找当前最大的
                    if (item.Value.z > maxValue)
                    {
                        maxValue = item.Value.z;
                        thisRect = item.Key;
                    }
                }
            }

            if (i == itemDic.Count-1)
            {
                Debug.Log(thisRect);
                currRect = thisRect;
            }
            //加入排序字典
            orderDic.Add(thisRect, i);
        }

        foreach (var order in orderDic )
        {
            order.Key.SetSiblingIndex(order.Value);
        }
        
        
    }

    public void setItemsAlpha()
    {
        float startValue = circleCenterPos.z - radius;
        foreach (var item in itemDic)
        {
            float alpha=(Mathf.Abs(item.Value.z - startValue))/(2*radius)*minAlpha+
                        1-Mathf.Abs(item.Value.z - startValue)/(2*radius)*maxAlpha;
            if (item.Key.TryGetComponent<RawImage>(out var rawImage))
            {
                rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, alpha);
            }
        }
    }

    public void setLevelImage()
    {
        foreach (var level in itemRectTransforms)
        {
            if (level.TryGetComponent<CheckPointState>(out CheckPointState checkPointState))
            {
                if (checkPointState.IsOpen)
                {
                    level.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    level.GetChild(0).gameObject.SetActive(true);
                }
            }
            else
            {
                level.GetComponent<RawImage>().texture = notHaveTexture;
            }
        }
    }

    public void SetCurrInformation()
    {
        if (currRect.TryGetComponent<CheckPointState>(out CheckPointState checkPointState))
        {
            if (checkPointState.IsOpen)
            {
                informationText.text = checkPointState.levelName;
            }
            else
            {
                informationText.text = "尚未解锁";
            }
            
        }
        else
        {
            informationText.text = "敬请期待";
        }
        
    }
    
}
