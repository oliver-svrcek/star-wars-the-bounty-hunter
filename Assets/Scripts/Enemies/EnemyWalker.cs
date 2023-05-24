using System;
using System.Collections;
using Other;
using PlayerScripts;
using UnityEngine;
using Utilities;

namespace Enemies
{
    public abstract class EnemyWalker : Enemy
    {
        private Coroutine MeleeAttackCoroutine { get; set; }
        private Coroutine MeleeAttackStartDelayCoroutine { get; set; }
        private Vector3 PatrolStartPosition { get; set; }
        private Vector3 PatrolEndPosition { get; set; }
        private Vector3 PatrolTargetPosition { get; set; }
        private CharacterMovementController CharacterMovementController { get; set; }
        private float HorizontalMove { get; set; }
        private bool Jump { get; set; }
        [field: SerializeField] protected int Damage { get; set; } = 0;
        [field: SerializeField] protected float Speed { get; set; } = 30f;
    
        protected new void Awake()
        {
            base.Awake();

            CharacterMovementController = Utils.GetComponentOrThrow<CharacterMovementController>(this.gameObject);
            PatrolEndPosition = Utils.GetGameObjectOrThrow(this.gameObject, "PatrolEndPosition").transform.position;
        }

        protected new void Start()
        {
            base.Start();
     
            HorizontalMove = 0f;
            Jump = false;
        
            PatrolStartPosition = this.gameObject.transform.position;
            PatrolEndPosition = this.gameObject.transform.Find("PatrolEndPosition").position;
            PatrolTargetPosition = PatrolEndPosition;
        }

        protected new void Update()
        { 
            HorizontalMove = 0 * Speed;
        
            Patrol();
        }

        private void Patrol()
        {
            if (PatrolTargetPosition.Equals(PatrolEndPosition))
            {
                HorizontalMove = 1 * Speed;
            }
            else
            {
                HorizontalMove = -1 * Speed;
            }

            if (!(Math.Abs(this.gameObject.transform.position.x - PatrolTargetPosition.x) < 0.1f))
            {
                return;
            }
        
            PatrolTargetPosition = PatrolTargetPosition.Equals(PatrolStartPosition) ? PatrolEndPosition : PatrolStartPosition;
        }

        private void FixedUpdate()
        {
            Move(); 
            Jump = false;
        }

        private void Move()
        { 
            CharacterMovementController.Move(HorizontalMove * Time.fixedDeltaTime, Jump);
        }
    
        private void OnCollisionEnter2D(Collision2D other)
        {
            OnCollisionStay2D(other);
        }
    
        private void OnCollisionStay2D(Collision2D other)
        {
            Health health;
            if (!other.gameObject.CompareTag("Player")
                || (health = other.gameObject.GetComponent<Health>()) is null
                || MeleeAttackCoroutine is not null
                || MeleeAttackStartDelayCoroutine is not null)
            {
                return;
            }
        
            MeleeAttackCoroutine = StartCoroutine(MeleeAttack(health));
        }
    
        private void OnCollisionExit2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")
                || MeleeAttackCoroutine is null
                || MeleeAttackStartDelayCoroutine is not null)
            {
                return;
            }
        
            StopCoroutine(MeleeAttackCoroutine);
            MeleeAttackCoroutine = null;
        
            MeleeAttackStartDelayCoroutine = StartCoroutine(MeleeAttackStartDelay());
        }

        private IEnumerator MeleeAttack(Health health)
        {
            while (true)
            {
                AudioManagement.PlayOneShot("MeleeHitSound");
                health.TakeDamage(Damage);
                
                yield return new WaitForSeconds(0.3f);   
            }
        }
    
        private IEnumerator MeleeAttackStartDelay()
        {
            yield return new WaitForSeconds(0.3f);
            MeleeAttackStartDelayCoroutine = null;
        }
    }
}