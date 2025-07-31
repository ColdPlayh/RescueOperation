using System;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

namespace Manager
{
    public class WeaponManager : SingleTon<WeaponManager> 
    {
        //储存人物使用的武器
        public Dictionary<string, MyWeapon> MyWeaponContainer = new Dictionary<string, MyWeapon>();
        //存储拾取的武器
        public Dictionary<string, GameObject> WeaponContainer = new Dictionary<string, GameObject>();
        //手榴弹
        [SerializeField]
        private FlashBangScript flashBang;
        [SerializeField]
        private GrenadeScript grenade;
        [SerializeField] 
        private GameObject[] wepaons;
        [SerializeField] 
        private MyWeapon[] myWeapons;
        [SerializeField]
        private MyWeapon mainWeapon;
        [SerializeField]
        private MyWeapon secondaryWeapon;
        [SerializeField]
        private MyWeapon currWeapon;

        public MyWeapon PickUpWepaon;
        public Action onMainChangeAction;
        public Action onSecondaryChangeAction;
        protected override void Awake()
        {
            base.Awake();
        }

        public FlashBangScript FlashBang => flashBang;
        public GrenadeScript Grenade => grenade;

        private void Start()
        {
            InitWeaponContainer();
            InitMyWeaponContainer();
        }
        
        public MyWeapon MainWeapon
        {
            get => mainWeapon;
            set
            {
                if (value != mainWeapon)
                {
                    mainWeapon = value;
                    onMainChangeAction?.Invoke();
                }
            }
        }
        
        public MyWeapon SecondaryWeapon
        {
            get => secondaryWeapon;
            set
            {
                if (value != secondaryWeapon)
                {
                    secondaryWeapon = value;
                    onSecondaryChangeAction?.Invoke();
                }
            }
        }
        
        public MyWeapon CurrWeapon
        {
            get => currWeapon;
            set
            {
                if (value != currWeapon)
                {
                    currWeapon = value;
                }
            }
        }
        
        public void InitWeaponContainer()
        {
            foreach (var i in wepaons)
            {
                var key = i.name;
                if (WeaponContainer.ContainsKey(key))
                {
                    WeaponContainer[key] = i;
                }
                else
                {
                    WeaponContainer.Add(key,i);
                }

                
            }
        }

        public void InitMyWeaponContainer()
        {
            foreach (var i in myWeapons)
            {
                var key = i.name;
                //初始化武器信息
                i.WeaponState.currMagazines = i.WeaponState.magazines;
                i.WeaponState.currAmmunition = i.WeaponState.currAmmunition;
                i.WeaponState.currAttack = i.WeaponState.baseAttack;
                if (MyWeaponContainer.ContainsKey(key))
                {
                    MyWeaponContainer[key] = i;
                }
                else
                {
                    MyWeaponContainer.Add(key,i);
                }
            }
            
        }
        
        public void SetPickUpAndRenderWeapon(string name)
        {
            WeaponContainer[name].SetActive(true);
            PickUpWepaon = MyWeaponContainer[name];
        }

        public void HideAllRenderWeapon()
        {
            foreach (var i in WeaponContainer.Keys)
            {
                WeaponContainer[i].SetActive(false);
            }
        }
    }
}