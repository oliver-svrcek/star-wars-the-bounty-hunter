using System.Collections;
using System.Collections.Generic;
using Enemies;
using Enums;
using Managers;
using Other;
using UnityEngine;
using Utilities;

namespace PlayerScripts
{
    public class Flamethrower : MonoBehaviour
    {
        private Animator Animator { get; set; }
        private AudioManagement AudioManagement { get; set; }
        private PlayerWeapons PlayerWeapons { get; set; }
        private PlayerMovement PlayerMovement { get; set; }
        private CharacterMovementController CharacterMovementController { get; set; }
        private BarManagement FlamethrowerHeatBar { get; set; }
        private ParticleSystem ParticleSystem { get; set; }
        private BoxCollider2D BoxCollider2D { get; set; }
        public Coroutine FlameCoroutine { get; private set; }
        public Coroutine CoolingCoroutine { get; private set; }
        public float ParticleStartLifetime { get; set; }
        private int MaximumFlamethrowerHeat { get; set; }
        private int CurrentFlamethrowerHeat { get; set; }
        private int FlamethrowerHeatPoints { get; set; }
        public int FlamethrowerOverheatCoolingPoints { get; set; }
        public float FlameRange { get; set; }
        public int FlamethrowerDamage { get; set; }
        private List<Enemy> EnemiesInRange { get; set; }
    
        private void Awake()
        {
            Animator = Utils.GetComponentOrThrow<Animator>("Player");
            PlayerWeapons = Utils.GetComponentOrThrow<PlayerWeapons>("Player");
            PlayerMovement = Utils.GetComponentOrThrow<PlayerMovement>("Player");
            CharacterMovementController = Utils.GetComponentOrThrow<CharacterMovementController>("Player");
            AudioManagement = Utils.GetComponentInChildrenOrThrow<AudioManagement>("Player/Audio/Flamethrower");
            FlamethrowerHeatBar = Utils.GetComponentOrThrow<BarManagement>("Interface/MainCamera/UICanvas/HUD/BarsWrapper/FlamethrowerBar/Slider");
            ParticleSystem = Utils.GetComponentOrThrow<ParticleSystem>(this.gameObject);
            BoxCollider2D = Utils.GetComponentOrThrow<BoxCollider2D>(this.gameObject);
        }

        private void Start()
        {
            ParticleStartLifetime = 0.25f;
            MaximumFlamethrowerHeat = 100000;
            CurrentFlamethrowerHeat = 0;
            FlamethrowerHeatPoints = 500;
            FlamethrowerOverheatCoolingPoints = 50;
            FlameRange = 2.75f;
            FlamethrowerDamage = 11000;
            EnemiesInRange = new List<Enemy>();
            
            AudioManagement.Stop();
            
            var particleSystemMain = ParticleSystem.main;
            particleSystemMain.startLifetime = ParticleStartLifetime;
            ParticleSystem.Stop();
            
            BoxCollider2D.offset = new Vector2(0.1f, 0);
            BoxCollider2D.size = new Vector2(0.2f, 1.5f);
            BoxCollider2D.enabled = false;
            
            FlamethrowerHeatBar.SetBar(BarType.Increasing, MaximumFlamethrowerHeat, CurrentFlamethrowerHeat);
        }

        public void Reload()
        {
            if (FlameCoroutine is not null)
            {
                Animator.SetBool("IsUsingFlamethrower", false);
                
                StopCoroutine(FlameCoroutine);
                FlameCoroutine = null;
            }

            if (CoolingCoroutine is not null)
            {
                StopCoroutine(CoolingCoroutine);
                CoolingCoroutine = null;
            }

            var particleSystemMain = ParticleSystem.main;
            particleSystemMain.startLifetime = ParticleStartLifetime;
            
            CurrentFlamethrowerHeat = 0;
            FlamethrowerHeatBar.SetBar(BarType.Increasing, MaximumFlamethrowerHeat, CurrentFlamethrowerHeat);
        }

        public void Activate()
        {
            if (FlameCoroutine is null && CoolingCoroutine is null && CharacterMovementController.IsGrounded)
            {
                FlameCoroutine = StartCoroutine(Flame());
            }
            else
            {
                AudioManagement.PlayOneShot("AbilityNotAvailableSound");
            }
        }
    
        private IEnumerator Flame()
        {
            Animator.SetBool("IsUsingFlamethrower", true);
        
            PlayerMovement.SetFreeze(true);
            PlayerMovement.enabled = false;
            PlayerWeapons.enabled = false;
            
            ParticleSystem.Play();
            BoxCollider2D.enabled = true;
            
            var increaseRangeCoroutine = StartCoroutine(IncreaseRange());
            var damageEnemiesCoroutine = StartCoroutine(DamageEnemies());

            AudioManagement.Play("FlameSoundLong", true);

            while (CurrentFlamethrowerHeat < MaximumFlamethrowerHeat)
            {
                FlamethrowerHeatBar.SetValue(CurrentFlamethrowerHeat);
                CurrentFlamethrowerHeat += FlamethrowerHeatPoints;
                
                yield return new WaitForSeconds(0.01f);
            }

            CurrentFlamethrowerHeat = MaximumFlamethrowerHeat;
            FlamethrowerHeatBar.SetValue(CurrentFlamethrowerHeat);
        
            Animator.SetBool("IsUsingFlamethrower", false);
        
            ParticleSystem.Stop();
            
            StopCoroutine(increaseRangeCoroutine);
            
            AudioManagement.Stop();
            
            this.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
            
            StopCoroutine(damageEnemiesCoroutine);
            
            BoxCollider2D.offset = new Vector2(0.1f, 0);
            BoxCollider2D.size = new Vector2(0.2f, 1.5f);
            BoxCollider2D.enabled = false;
            
            EnemiesInRange = new List<Enemy>();
            
            CoolingCoroutine = StartCoroutine(Cooling());
            
            PlayerWeapons.enabled = true;
            PlayerMovement.enabled = true;
            PlayerMovement.SetFreeze(false);
            
            FlameCoroutine = null;
        }
    
        private IEnumerator Cooling()
        {
            CurrentFlamethrowerHeat = MaximumFlamethrowerHeat;
            FlamethrowerHeatBar.SetBar(BarType.Recharging, MaximumFlamethrowerHeat, CurrentFlamethrowerHeat);
         
            while (CurrentFlamethrowerHeat > FlamethrowerOverheatCoolingPoints)
            {
                yield return new WaitForSeconds(0.01f);
                
                CurrentFlamethrowerHeat -= FlamethrowerOverheatCoolingPoints;
                FlamethrowerHeatBar.SetValue(CurrentFlamethrowerHeat);
            }
         
            AudioManagement.PlayOneShot("AbilityRechargedSound");
            
            CurrentFlamethrowerHeat = 0;
            FlamethrowerHeatBar.SetBar(BarType.Increasing, MaximumFlamethrowerHeat, CurrentFlamethrowerHeat);
            
            CoolingCoroutine = null;
        }
    
        private IEnumerator IncreaseRange()
        {
            while (BoxCollider2D.size.x < FlameRange)
            {
                BoxCollider2D.size = new Vector2(BoxCollider2D.size.x + 0.14f, BoxCollider2D.size.y);
                BoxCollider2D.offset = new Vector2(BoxCollider2D.size.x / 2, BoxCollider2D.offset.y);
                
                yield return new WaitForSeconds(0.01f);
            }
        
            BoxCollider2D.size = new Vector2(FlameRange, BoxCollider2D.size.y);
            BoxCollider2D.offset = new Vector2(BoxCollider2D.size.x / 2, BoxCollider2D.offset.y);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerStay2D(other);
        }
    
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.isTrigger || other.CompareTag("Player"))
            {
                return;
            }
        
            Enemy enemy;
            if ((enemy = other.gameObject.GetComponentInParent<Enemy>()) is null)
            {
                return;
            }
            
            if (!EnemiesInRange.Contains(enemy))
            {
                EnemiesInRange.Add(enemy);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.isTrigger || other.CompareTag("Player"))
            {
                return;
            }
        
            Enemy enemy;
            if ((enemy = other.gameObject.GetComponentInParent<Enemy>()) is not null)
            {
                EnemiesInRange.Remove(enemy);
            }
        }

        private IEnumerator DamageEnemies()
        {
            while (true)
            {
                foreach (var enemy in EnemiesInRange.ToArray())
                {
                    if (enemy == null)
                    {
                        continue;
                    }
                    
                    AudioManagement.PlayOneShot("HitmarkerSound");
                    enemy.TakeDamage(FlamethrowerDamage);
                }
        
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
