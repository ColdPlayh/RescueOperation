using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpticalSight : MonoBehaviour
{
    [Header("����")]
    public Transform ��ǰ���;
    public Transform pIP��Ƭ;
    public Transform ��׼��;
    public Camera pIP���;
    public float ��ʼPIP���FOV = 60f;
    [Header("PIP")]
    public float ��СPIP���� = 1f;
    public float ���PIP���� = 1f;
    public float ��ǰPIP����=1f;
    [Tooltip("����x ��z���С")]
    public float pIP��Ƭ��ʼ��С;
    [Header("�ڿ�")]
    public Transform �ڱ�;
    public float �ڱ߳�ʼ��С;
    public float �ڱ���Сֵ = 1f;

    float range,scale;
    // Start is called before the first frame update
    void OnEnable()
    {
        range = Vector3.Distance(transform.position, ��׼��.position);
    }

    // Update is called once per frame
    void Update()
    {
        float scale = Vector3.Distance(��ǰ���.position, transform.position) / range;
        transform.forward = transform.position - ��ǰ���.position;
        if (��ǰPIP���� <= ���PIP���� && ��ǰPIP���� >= ��СPIP����)
        {
            pIP���.fieldOfView = ��ʼPIP���FOV * ��ǰPIP����;   
        }
        pIP��Ƭ.localScale = new Vector3(pIP��Ƭ��ʼ��С * scale, pIP��Ƭ��ʼ��С * scale, pIP��Ƭ.localScale.z);
        if (�ڱ߳�ʼ��С / scale < �ڱ���Сֵ)
        {
            scale = 1 / �ڱ���Сֵ;
        }
        �ڱ�.localScale = new Vector3(�ڱ߳�ʼ��С / scale, �ڱ߳�ʼ��С / scale, �ڱ�.localScale.z);
        �ڱ�.forward = transform.forward;
    }
}
