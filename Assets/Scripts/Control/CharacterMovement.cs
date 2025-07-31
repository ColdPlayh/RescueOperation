using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Vector3 CurrInput { get; private set; }
    private Rigidbody _rigidbody;
    public float maxSpeed=3;
    public float turnspeed = 3;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (CurrInput.magnitude != 0)
        {

            Quaternion quaDir = Quaternion.LookRotation(CurrInput, Vector3.up);
            //缓慢转动到目标点
            transform.rotation = Quaternion.Lerp(transform.rotation, quaDir, Time.fixedDeltaTime * turnspeed);
        }
        _rigidbody.MovePosition(_rigidbody.position + CurrInput * maxSpeed*Time.fixedDeltaTime);
    }
    public void setMovementInput(Vector3 input)
    {
        CurrInput = Vector3.ClampMagnitude(input, 1).normalized;
    }
}
