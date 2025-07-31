using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoGerpher : MonoBehaviour
{
    public float Pitch { get; private set; }
    public float Yaw { get; private set; }

    public Joystick rotateJoyStick;

    public float speed=80f;


    public float zOffset=0;

    [SerializeField] private AnimationCurve armLengthCurve;

    private Transform target;
    private new Transform camera;

    private float cameraYSpeed = 5;
    private float turnspeed;

    public Transform scopePonit;
    private void Awake()
    {
        camera = transform.GetChild(0);
    }
    void Update()
    {
        UpdateRotate();
        UpdatePosition();
        UpdateArmLengthCurve();
    }
    public void InitCamera(Transform input)
    {
        target = input;
        //target = scopePonit;
        transform.position = target.position;
    }
    public void UpdateArmLengthCurve()
    {
        camera.localPosition = new Vector3(0, 0, armLengthCurve.Evaluate(Pitch)*-1);
    }
    public void UpdatePosition()
    {
        float newY = Mathf.Lerp(transform.position.y, 
            target.position.y, 
            Time.deltaTime * cameraYSpeed);
        transform.position = new Vector3(target.position.x, newY, target.position.z+zOffset);
    }
    public void UpdateRotate()
    {
        Yaw += rotateJoyStick.Horizontal * speed * Time.deltaTime;
        Pitch += rotateJoyStick.Vertical * speed * Time.deltaTime;
        Pitch = Mathf.Clamp(Pitch, -40, 50);
        transform.rotation = Quaternion.Euler(-Pitch, Yaw, zOffset);
    }
}
