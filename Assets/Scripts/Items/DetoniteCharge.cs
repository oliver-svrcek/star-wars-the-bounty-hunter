using System.Collections;
using Managers;
using PlayerScripts;
using UnityEngine;
using Utilities;

namespace Items
{
    public class DetoniteCharge : MonoBehaviour
    {
        private Health PlayerHealth { get; set; }
        private AudioManagement AudioManagement { get; set; }
        private SpriteRenderer DetoniteChargeSprite { get; set; }
        private SpriteRenderer HitRadiusSprite { get; set; }
        private CircleCollider2D HitRadiusCollider  { get; set; }
        private ParticleSystem ParticleSystem { get; set; }
        private Animator Animator { get; set; }
        private int Damage { get; set; }
    
        private void Awake()
        {
            PlayerHealth = Utils.GetComponentOrThrow<Health>("Player");
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>(this.gameObject);
            DetoniteChargeSprite = Utils.GetComponentOrThrow<SpriteRenderer>(this.gameObject);
            HitRadiusSprite = Utils.GetComponentOrThrow<SpriteRenderer>(this.gameObject, "HitRadius");
            HitRadiusCollider = Utils.GetComponentOrThrow<CircleCollider2D>(this.gameObject);
            ParticleSystem = Utils.GetComponentOrThrow<ParticleSystem>(this.gameObject, "ExplosionParticles");
            Animator = Utils.GetComponentOrThrow<Animator>(this.gameObject);
        }
    
        private void Start()
        {
            Damage = int.MaxValue;
            ParticleSystem.Stop();
            Animator.enabled = false;
            HitRadiusSprite.enabled = false;
            HitRadiusCollider.enabled = false;
        }

        public void Activate()
        {
            StartCoroutine(Explode());
        }
    
        public IEnumerator Explode()
        {
            var hitRadiusBlinkingCoroutine = StartCoroutine(HitRadiusBlinking());
            Animator.enabled = true;
            AudioManagement.PlayClipAtPoint("DetoniteChargeSound", this.gameObject.transform.position);
        
            yield return new WaitForSeconds(2.5f);
        
            StopCoroutine(hitRadiusBlinkingCoroutine);
            DetoniteChargeSprite.enabled = false;
            HitRadiusSprite.enabled = false;
            ParticleSystem.Play();
            HitRadiusCollider.enabled = true;

            yield return new WaitForSeconds(0.8f);
        
            AudioManagement.RemoveFromMainAudioManagement();
            Destroy(this.gameObject);
        }

        private IEnumerator HitRadiusBlinking()
        {
            while (true)
            {
                HitRadiusSprite.enabled = !HitRadiusSprite.enabled;
                yield return new WaitForSeconds(0.1f);
            }
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerStay2D(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }
        
            Health health;
            if ((health = other.gameObject.GetComponent<Health>()) is null)
            {
                return;
            }
        
            AudioManagement.PlayClipAtPoint("HitmarkerSound", other.transform.position);
            health.TakeDamage(Damage);
            AudioManagement.RemoveFromMainAudioManagement();
        }
    }
}
