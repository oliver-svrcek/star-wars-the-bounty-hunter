using System;
using System.Collections;
using Pathfinding;
using PlayerScripts;
using UnityEngine;
using Utilities;

namespace Enemies
{
    public abstract class EnemyFlyer : Enemy
    {
        private AIPath AIPath { get; set; }
        private AIDestinationSetter AIDestinationSetter { get; set; }
        private Coroutine MeleeAttackCoroutine { get; set; }
        private Coroutine MeleeAttackStartDelayCoroutine { get; set; }
        [field: SerializeField] protected float ViewRangeHorizontal { get; set; } = 11f;
        [field: SerializeField] protected float ViewRangeVertical { get; set; } = 8f;
        protected int Damage { get; set; } = 0;
    
        protected new void Awake()
        {
            base.Awake();
        
            AIPath = Utils.GetComponentOrThrow<AIPath>(this.gameObject);
            AIDestinationSetter = Utils.GetComponentOrThrow<AIDestinationSetter>(this.gameObject);
        }

        protected new void Start()
        {
            base.Start();

            AIDestinationSetter.target = Player.transform;
            AIPath.canMove = false;
            AIPath.enabled = false;
        }

    
        protected new void Update()
        {
            base.Update();
        
            var horizontalDistance = Player.transform.position.x - this.transform.position.x;
            var verticalDistance = Player.transform.position.y - this.transform.position.y;

            if (Math.Abs(horizontalDistance) < ViewRangeHorizontal &&
                Math.Abs(verticalDistance) < ViewRangeVertical)
            {
                AIPath.enabled = true;
                AIPath.canMove = true;
            }
            else
            {
                AIPath.canMove = false;
                AIPath.enabled = false;
            }
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
