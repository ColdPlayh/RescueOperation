using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpticalSight : MonoBehaviour
{
    [Header("基础")]
    public Transform 当前相机;
    public Transform pIP镜片;
    public Transform 瞄准点;
    public Camera pIP相机;
    public float 初始PIP相机FOV = 60f;
    [Header("PIP")]
    public float 最小PIP倍数 = 1f;
    public float 最大PIP倍数 = 1f;
    public float 当前PIP倍数=1f;
    [Tooltip("输入x 或z轴大小")]
    public float pIP镜片初始大小;
    [Header("黑框")]
    public Transform 黑边;
    public float 黑边初始大小;
    public float 黑边最小值 = 1f;

    float range,scale;
    // Start is called before the first frame update
    void OnEnable()
    {
        range = Vector3.Distance(transform.position, 瞄准点.position);
    }

    // Update is called once per frame
    void Update()
    {
        float scale = Vector3.Distance(当前相机.position, transform.position) / range;
        transform.forward = transform.position - 当前相机.position;
        if (当前PIP倍数 <= 最大PIP倍数 && 当前PIP倍数 >= 最小PIP倍数)
        {
            pIP相机.fieldOfView = 初始PIP相机FOV * 当前PIP倍数;   
        }
        pIP镜片.localScale = new Vector3(pIP镜片初始大小 * scale, pIP镜片初始大小 * scale, pIP镜片.localScale.z);
        if (黑边初始大小 / scale < 黑边最小值)
        {
            scale = 1 / 黑边最小值;
        }
        黑边.localScale = new Vector3(黑边初始大小 / scale, 黑边初始大小 / scale, 黑边.localScale.z);
        黑边.forward = transform.forward;
    }
}
