using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWeapon : MonoBehaviour
{
    GameObject weaponType;
    Vector3 recoilTrans;
    public float recoilPos;
    public virtual Vector3 GetRecoil(Transform muzzlePoint)
    {
        float offsetX = Random.Range(-recoilPos, recoilPos);
        float offsetY = Random.Range(-recoilPos, recoilPos);
        recoilTrans = new Vector3(muzzlePoint.position.x + offsetX,
            muzzlePoint.position.y + offsetY,
            muzzlePoint.position.z);
        return recoilTrans;
    }
}
