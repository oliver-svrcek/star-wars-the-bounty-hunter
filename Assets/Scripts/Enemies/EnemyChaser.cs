using System;
using System.Collections;
using Other;
using PlayerScripts;
using UnityEngine;
using Utilities;

namespace Enemies
{
    public abstract class EnemyChaser : Enemy
    {
        private Transform ObstacleDetectionPoint { get; set; }
        private Coroutine MeleeAttackCoroutine { get; set; }
        private Coroutine MeleeAttackStartDelayCoroutine { get; set; }
        private CharacterMovementController CharacterMovementController { get; set; }
        private Rigidbody2D Rigidbody2D { get; set; }
        private Rigidbody2D PlayerRigidbody2D { get; set; }
        private float HorizontalMove { get; set; }
        private bool Jump { get; set; }
        [field: SerializeField] protected float ViewRangeHorizontal { get; set; } = 11f;
        [field: SerializeField] protected float ViewRangeVertical { get; set; } = 7f;
        [field: SerializeField] protected int Damage { get; set; } = 0;
        [field: SerializeField] protected float Speed { get; set; } = 30f;

        protected new void Awake()
        {
            base.Awake();
            
            ObstacleDetectionPoint = Utils.GetGameObjectOrThrow(this.gameObject, "ObstacleDetectionPoint").transform;
            CharacterMovementController = Utils.GetComponentOrThrow<CharacterMovementController>(this.gameObject);
            Rigidbody2D = Utils.GetComponentOrThrow<Rigidbody2D>(this.gameObject);
            PlayerRigidbody2D = Utils.GetComponentOrThrow<Rigidbody2D>("Player");
        }

        protected new void Start()
        {
            base.Start();

            MeleeAttackCoroutine = null;
            MeleeAttackStartDelayCoroutine = null;
            HorizontalMove = 0f;
            Jump = false;
        }

        protected new void Update()
        {
            HorizontalMove = 0 * Speed;
        
            ChasePlayer();
        }

        private void ChasePlayer()
        {
            var horizontalDistance = Player.transform.position.x - this.transform.position.x;
            var verticalDistance = Player.transform.position.y - this.transform.position.y;

            if (!(Math.Abs(horizontalDistance) < ViewRangeHorizontal) ||
                !(Math.Abs(horizontalDistance) > BodyCollider.size.x) ||
                !(Math.Abs(verticalDistance) < ViewRangeVertical))
            {
                return;
            }
            
            if (horizontalDistance > 0)
            {
                HorizontalMove = 1 * Speed;
            }
            else if (horizontalDistance < 0)
            {
                HorizontalMove = -1 * Speed;
            }

            // Jump if obstacle is in front of enemy
            var hit = Physics2D.Raycast(ObstacleDetectionPoint.position, ObstacleDetectionPoint.right, 0.1f);
            
            if (hit.collider is not null && !hit.collider.isTrigger &&
                (hit.transform.tag == "Tilemap" || hit.transform.tag == "OtherSolid") && 
                Math.Abs(Rigidbody2D.velocity.x) < 0.001f)
            {
                Jump = true;
            }

            // Jump if player jumps over enemy
            if (!(Math.Abs(horizontalDistance) < 1.5f) || !(Math.Abs(verticalDistance) < 2f))
            {
                return;
            }
            
            if (PlayerRigidbody2D.velocity.y > 0.1f &&
                ((Rigidbody2D.velocity.x < 0 && PlayerRigidbody2D.velocity.x > 0) ||
                 (Rigidbody2D.velocity.x > 0 && PlayerRigidbody2D.velocity.x < 0)))
            {
                Jump = true;
            }
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
