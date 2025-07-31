using UnityEngine;
using UnityEngine.UI;

namespace Weapon
{
    public class MyWeapon : MonoBehaviour
    {
        
        //public string WeaponType;
        
        [Header("BaseSetting")]
        public AnimatorOverrideController WeaponAnim;
        public WeaponState_So WeaponState;
        public Transform muzzlePoint;
        public Transform casingPoint;
        
        [Header("ShootSetting")]
        public GameObject bulletPrefab;
        public GameObject casingPrefab;
        public float bulletForce;
        public float fireRate;
        [Header("AudioSetting")]
        
        public AudioClip reloadAmmoClip;
        public AudioClip reloadOutClip;
        public AudioClip shootClip;
        public AudioClip shootSliencerClip;
        
        [Header("UISetting")]
        public Sprite uiImage;
        [Header("RecoilSetting")]
        
        public float RecoilOffset;
        private Vector3 recoilTrans;
        

       
        
        public virtual Vector3 GetRecoil(Transform muzzlePoint)
        {
            float offsetX = Random.Range(-RecoilOffset, RecoilOffset);
            float offsetY = Random.Range(-RecoilOffset, RecoilOffset);
            recoilTrans = new Vector3(muzzlePoint.position.x + offsetX,
                muzzlePoint.position.y + offsetY,
                muzzlePoint.position.z);
            return recoilTrans;
        }
    }
}