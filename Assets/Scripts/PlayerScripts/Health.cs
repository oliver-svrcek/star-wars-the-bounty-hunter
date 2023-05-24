using System;
using System.Collections;
using Enums;
using Managers;
using UI.Menus;
using UnityEngine;
using Utilities;

namespace PlayerScripts
{
    public class Health : MonoBehaviour
    {
        private PlayerMovement PlayerMovement { get; set; }
        private PlayerWeapons PlayerWeapons { get; set; }
        private SpriteRenderer SpriteRenderer { get; set; }
        private Animator Animator { get; set; }
        private CapsuleCollider2D BodyCollider { get; set; }
        private Rigidbody2D Rigidbody2D { get; set; }
        private AudioManagement AudioManagement { get; set; }
        private DeathMenu DeathMenu { get; set; }
        private BarManagement HealthBar { get; set; }
        public Coroutine HealCoroutine { get; private set; }
        private Coroutine BleedCoroutine { get; set; }
        public int MaximumHealth { get; set; }
        private int CurrentHealth { get; set; }
        public float HealStartTime { get; set; }
        public int HealPoints { get; set; }
        private string DeathSound { get; set; }
        private bool CanHeal { get; set; }

        private void Awake()
        {
            PlayerMovement = Utils.GetComponentOrThrow<PlayerMovement>("Player");
            PlayerWeapons = Utils.GetComponentOrThrow<PlayerWeapons>("Player");
            SpriteRenderer = Utils.GetComponentOrThrow<SpriteRenderer>("Player");
            Animator = Utils.GetComponentOrThrow<Animator>("Player");
            BodyCollider = Utils.GetComponentOrThrow<CapsuleCollider2D>("Player");
            Rigidbody2D = Utils.GetComponentOrThrow<Rigidbody2D>("Player");
            AudioManagement = Utils.GetComponentInChildrenOrThrow<AudioManagement>("Player");
            DeathMenu = Utils.GetComponentOrThrow<DeathMenu>(Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/DeathMenu"));
            HealthBar = Utils.GetComponentOrThrow<BarManagement>(Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/HUD/BarsWrapper/HealthBar/Slider"));
        }

        private void Start()
        {
            HealCoroutine = null;
            BleedCoroutine = null;
            MaximumHealth = 10000;
            CurrentHealth = MaximumHealth;
            HealStartTime = 4.5f;
            HealPoints = 10;
            DeathSound = "PlayerDeathSound";
            CanHeal = true;
            
            Reload();
        }
        
        public void Reload()
        {
            if (HealCoroutine is not null)
            {
                StopCoroutine(HealCoroutine);
                HealCoroutine = null;
            }
        
            if (BleedCoroutine is not null)
            {
                StopCoroutine(BleedCoroutine);
                BleedCoroutine = null;
            }
        
            CurrentHealth = MaximumHealth;
            HealthBar.SetBar(BarType.Decreasing, MaximumHealth, CurrentHealth);
        }

        private IEnumerator Heal()
        {
            if (DeathMenu.ActivateCoroutine is not null)
            {
                yield break;
            }
        
            yield return new WaitForSeconds(HealStartTime);

            while (CurrentHealth < (MaximumHealth - HealPoints))
            {
                CurrentHealth += HealPoints;
                HealthBar.SetValue(CurrentHealth);
                
                yield return new WaitForSeconds(0.01f);
            }

            CurrentHealth = MaximumHealth;
            HealthBar.SetValue(CurrentHealth);
            
            HealCoroutine = null;
        }
    
        public void TakeDamage(int damage)
        {
            if (DeathMenu.ActivateCoroutine is not null)
            {
                return;
            }
        
            if (HealCoroutine is not null)
            {
                StopCoroutine(HealCoroutine);
            }
            if (BleedCoroutine is not null)
            {
                StopCoroutine(BleedCoroutine);
            }
            BleedCoroutine = StartCoroutine(Bleed());
        
            CurrentHealth -= damage;
            HealthBar.SetValue(CurrentHealth);

            if (CurrentHealth <= 0)
            {
                AudioManagement.PlayOneShot(DeathSound);
                
                PlayerMovement.SetFreeze(true);
                PlayerMovement.enabled = false;
                PlayerWeapons.enabled = false;
                
                Rigidbody2D.bodyType = RigidbodyType2D.Static;
                BodyCollider.enabled = false;
                Animator.enabled = false;
                
                if (BleedCoroutine is not null)
                {
                    StopCoroutine(BleedCoroutine);
                    BleedCoroutine = null;
                }
                
                SpriteRenderer.color = new Color32(255, 50, 50, 200);
                
                DeathMenu.Activate();
            }
            else if (CanHeal)
            {
                HealCoroutine = StartCoroutine(Heal());
            }
        }

        private IEnumerator Bleed()
        {
            if (DeathMenu.ActivateCoroutine is not null)
            {
                yield break;
            }
            
            var hitColor = new Color32(255, 100, 100, 255);
            var originalColor = new Color32(255, 255, 255, 255);
        
            SpriteRenderer.color = hitColor;
            yield return new WaitForSeconds(0.2f);
            SpriteRenderer.color = originalColor;
            BleedCoroutine = null;
        }
    }
}