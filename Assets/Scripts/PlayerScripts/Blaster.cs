using System.Collections;
using Enums;
using Items;
using Managers;
using UnityEngine;
using Utilities;

namespace PlayerScripts
{
    public class Blaster : MonoBehaviour
    {
        private Animator Animator { get; set; }
        private AudioManagement AudioManagement { get; set; } 
        private BarManagement BlasterHeatBar { get; set; }
        private GameObject BulletPrefab { get; set; }
        public Coroutine ShootCoroutine { get; private set; }
        public Coroutine CoolingCoroutine { get; private set; }
        public Coroutine OverheatCoroutine { get; private set; }
        public int MaximumBlasterHeat { get; set; }
        private int CurrentBlasterHeat { get; set; }
        public int BlasterHeatPerShot { get; set; }
        public float BlasterCoolingStartTime { get; set; }
        public int BlasterCoolingPower { get; set; }
        public float BlasterOverheatCoolingStartTime { get; set; }
        public int BlasterOverheatCoolingPower { get; set; }
        public int BulletDamage { get; set; }
        private float BulletSpeed { get; set; }
        private string BulletSound { get; set; }

        private void Awake()
        {
            Animator = Utils.GetComponentOrThrow<Animator>("Player");
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>("Player/Audio/Blaster");
            BlasterHeatBar = Utils.GetComponentOrThrow<BarManagement>("Interface/MainCamera/UICanvas/HUD/BarsWrapper/BlasterBar/Slider");
            BulletPrefab = Utils.GetResourceOrThrow<GameObject>("Prefabs/Objects/Bullet");
        }
    
        private void Start()
        {
            MaximumBlasterHeat = 10000;
            CurrentBlasterHeat = 0;
            BlasterHeatPerShot = 2000;
            BlasterCoolingStartTime = 0.2f;
            BlasterCoolingPower = 100;
            BlasterOverheatCoolingStartTime = 0.6f;
            BlasterOverheatCoolingPower = 60;
            BulletDamage = 2000;
            BulletSpeed = 20f;
            ShootCoroutine = null;
            CoolingCoroutine = null;
            OverheatCoroutine = null;
            BulletSound = "PlayerBlasterShotSound";
        
            BlasterHeatBar.SetBar(BarType.Increasing, MaximumBlasterHeat, CurrentBlasterHeat);
        }
    
        public void Reload()
        {
            if (ShootCoroutine is not null)
            {
                Animator.SetBool("IsShooting", false);
            
                StopCoroutine(ShootCoroutine);
                ShootCoroutine = null;
            }
        
            if (CoolingCoroutine is not null)
            {
                StopCoroutine(CoolingCoroutine);
                CoolingCoroutine = null;
            }
        
            if (OverheatCoroutine is not null)
            {
                StopCoroutine(OverheatCoroutine);
                OverheatCoroutine = null;
            }

            BlasterHeatBar.SetBar(BarType.Increasing, MaximumBlasterHeat, CurrentBlasterHeat);
        }
    
        public void Activate()
        {
            if (OverheatCoroutine is null)
            {
                if (ShootCoroutine != null)
                {
                    StopCoroutine(ShootCoroutine);
                }
            
                ShootCoroutine = StartCoroutine(Shoot());
            }
            else
            {
                AudioManagement.PlayOneShot("AbilityNotAvailableSound");
            }
        }

        private IEnumerator Shoot()
        {
            Animator.SetBool("IsShooting", true);
        
            if (CoolingCoroutine is not null)
            {
                StopCoroutine(CoolingCoroutine);
            }

            for (var i = 0; i < 3; i++)
            {
                SpawnBullet();
            
                CurrentBlasterHeat += BlasterHeatPerShot;
                BlasterHeatBar.SetValue(CurrentBlasterHeat);

                if (CurrentBlasterHeat >= MaximumBlasterHeat)
                {
                    Animator.SetBool("IsShooting", false);
                
                    CurrentBlasterHeat = MaximumBlasterHeat;
                
                    OverheatCoroutine = StartCoroutine(Overheat());
                
                    AudioManagement.PlayOneShot("PlayerBlasterOverheatSound");
                
                    yield break;
                }

                yield return new WaitForSeconds(0.1f);
            }
        
            Animator.SetBool("IsShooting", false);
        
            CoolingCoroutine = StartCoroutine(Cooling());
        
            ShootCoroutine = null;
        }

        private IEnumerator Cooling()
        {
            yield return new WaitForSeconds(BlasterCoolingStartTime);

            while (CurrentBlasterHeat > BlasterCoolingPower)
            {
                yield return new WaitForSeconds(0.01f);
            
                CurrentBlasterHeat -= BlasterCoolingPower;
                BlasterHeatBar.SetValue(CurrentBlasterHeat);
            }
        
            CurrentBlasterHeat = 0;
            BlasterHeatBar.SetValue(CurrentBlasterHeat);
        
            CoolingCoroutine = null;
        }

        private IEnumerator Overheat()
        {
            AudioManagement.PlayOneShot("PlayerBlasterOverheatSound");
        
            CurrentBlasterHeat = MaximumBlasterHeat;
            BlasterHeatBar.SetBar(BarType.Recharging, MaximumBlasterHeat, MaximumBlasterHeat);

            yield return new WaitForSeconds(BlasterOverheatCoolingStartTime);

            while (CurrentBlasterHeat > BlasterOverheatCoolingPower)
            {
                yield return new WaitForSeconds(0.01f);
            
                CurrentBlasterHeat -= BlasterOverheatCoolingPower;
                BlasterHeatBar.SetValue(CurrentBlasterHeat);
            }

            AudioManagement.PlayOneShot("PlayerBlasterRechargedSound");
        
            CurrentBlasterHeat = 0;
            BlasterHeatBar.SetBar(BarType.Increasing, MaximumBlasterHeat, CurrentBlasterHeat);
        
            OverheatCoroutine = null;
        }

 
        private void SpawnBullet()
        {
            var bullet = Instantiate(
                BulletPrefab,
                this.gameObject.transform.position,
                this.gameObject.transform.rotation
            );
            bullet.GetComponent<Bullet>().Initialize(this.gameObject, BulletDamage, BulletSpeed);
        
            AudioManagement.PlayOneShot(BulletSound);
        }
    }
}
