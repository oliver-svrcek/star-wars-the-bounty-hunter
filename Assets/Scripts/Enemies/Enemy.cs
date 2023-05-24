using System.Collections;
using System.Text.RegularExpressions;
using Enums;
using Managers;
using PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [field: SerializeField] protected bool UseOnlyEditorValues { get; set; } = false;
        protected Player Player { get; private set; }
        protected AudioManagement AudioManagement { get; private set; }
        private SpriteRenderer SpriteRenderer { get; set; }
        protected CapsuleCollider2D BodyCollider { get; set; }
        private GameObject HealthBarGameObject { get; set; }
        private BarManagement HealthBar { get; set; }
        private TextMeshProUGUI BossName { get; set; }
        private Coroutine HealCoroutine { get; set; }
        private Coroutine BleedCoroutine { get; set; }
        [field: SerializeField] protected int MaximumHealth { get; set; } = 0;
        [field: SerializeField] protected int CurrentHealth { get; set; } = 0;
        [field: SerializeField] protected float HealStartTime { get; set; } = 0f;
        [field: SerializeField] protected int HealPoints { get; set; } = 0;
        [field: SerializeField] protected string DeathSound { get; set; } = "GenericDeathSound";
        [field: SerializeField] protected bool CanHeal { get; set; } = false;
        private bool IsLookingRight { get; set; }
    
        [Header("Events")]
        [Space]
        public UnityEvent OnDeathEvent;
        [System.Serializable]
        public class BoolEvent : UnityEvent<bool> { }

        protected void Awake()
        {
            Player = Utils.GetComponentOrThrow<Player>("Player");
            AudioManagement = Utils.GetComponentInChildrenOrThrow<AudioManagement>(this.gameObject);
            SpriteRenderer = Utils.GetComponentOrThrow<SpriteRenderer>(this.gameObject);
            BodyCollider = Utils.GetComponentOrThrow<CapsuleCollider2D>(this.gameObject);

            if (this.gameObject.CompareTag("Enemy"))
            {
                HealthBar = Utils.GetComponentInChildrenOrThrow<BarManagement>(this.gameObject);
            }
            else if (this.gameObject.CompareTag("Boss"))
            {
                HealthBarGameObject = GameObject.Find("Interface/MainCamera/UICanvas/HUD/BossHealthBar");
                HealthBarGameObject.SetActive(true);
            
                HealthBar = Utils.GetComponentOrThrow<BarManagement>("Interface/MainCamera/UICanvas/HUD/BossHealthBar/Slider");
                BossName = Utils.GetComponentOrThrow<TextMeshProUGUI>("Interface/MainCamera/UICanvas/HUD/BossHealthBar/BossName");
            }
        
            if (OnDeathEvent == null)
            {
                OnDeathEvent = new UnityEvent();	
            }
        
            IsLookingRight = true;
        }

        protected void Start()
        {
            HealthBar.SetBar(BarType.EnemyHealth, MaximumHealth, CurrentHealth);
        
            if (this.gameObject.CompareTag("Boss"))
            {
                BossName.text = Regex.Replace(this.gameObject.name, "(\\B[A-Z])", " $1");
            }
        }

        protected void Update()
        {
            LookAtPlayer();
        }

        protected void LookAtPlayer()
        {
            if ((Player.transform.position.x > (this.transform.position.x + BodyCollider.size.x) && !IsLookingRight)
                || (Player.transform.position.x < (this.transform.position.x - BodyCollider.size.x) && IsLookingRight))
            {
                transform.Rotate(0f, 180f, 0f);
                IsLookingRight = !IsLookingRight;
            }
        }

        private IEnumerator Heal()
        {
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
                AudioManagement.PlayClipAtPoint(DeathSound, this.gameObject.transform.position);
                AudioManagement.RemoveFromMainAudioManagement();

                if (this.gameObject.CompareTag("Boss"))
                {
                    HealthBarGameObject.SetActive(false);
                }
            
                OnDeathEvent.Invoke();
                Destroy(this.gameObject);
            }
            else if (CanHeal)
            {
                HealCoroutine = StartCoroutine(Heal());
            }
        }
    
        private IEnumerator Bleed()
        {
            var hitColor = new Color32(255, 100, 100, 255);
            var originalColor = new Color32(255, 255, 255, 255);
        
            SpriteRenderer.color = hitColor;
            yield return new WaitForSeconds(0.2f);
            SpriteRenderer.color = originalColor;
            BleedCoroutine = null;
        }
    }
}
