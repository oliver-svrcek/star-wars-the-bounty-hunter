using Enemies;
using Managers;
using PlayerScripts;
using UnityEngine;
using Utilities;

namespace Items
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody2D Rigidbody2D { get; set; }
        private AudioManagement AudioManagement { get; set; }
        private string ParentTag { get; set; }
        private Vector3 ParentPosition { get; set; }
        private int Damage { get; set; }
        private float Speed { get; set; }
        private float SelfDestructDistance { get; set; }
        public bool IsInitialized { get; private set; } = false;

        private void Awake()
        {
            Rigidbody2D = Utils.GetComponentOrThrow<Rigidbody2D>(this.gameObject);
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>(this.gameObject);
            
            ParentTag = "";
        }

        private void Start()
        {
            Speed = 20f;
            SelfDestructDistance = 30;

            Move();
        }

        public void Initialize(GameObject parent, int damage, float speed)
        {
            if (!IsInitialized)
            {
                Damage = damage;
                Speed = speed;
                ParentTag = parent.gameObject.tag;
                ParentPosition = parent.transform.position;
            }
            else
            {
                Debug.LogError("Bullet already initialized!", this);
            }
        }

        private void Update()
        {
            if (Vector3.Distance(this.transform.position, ParentPosition) > SelfDestructDistance)
            {
                AudioManagement.RemoveFromMainAudioManagement();
                Destroy(this.gameObject);
            }
        }

        private void Move()
        {
            Rigidbody2D.velocity = this.gameObject.transform.right * Speed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("OtherSolid"))
            {
                AudioManagement.RemoveFromMainAudioManagement();
                Destroy(this.gameObject);
                
                return;
            }

            if (other.isTrigger)
            {
                return;
            }

            if (ParentTag == "Player" || ParentTag == "Friendly")
            {
                if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Friendly"))
                {
                    return;
                }

                Enemy enemy;
                if ((enemy = other.gameObject.GetComponentInParent<Enemy>()) is not null)
                {
                    AudioManagement.PlayClipAtPoint("HitmarkerSound", this.gameObject.transform.position);
                    enemy.TakeDamage(Damage);
                }
            }
            else
            {
                if (other.gameObject.GetComponentInParent<Enemy>() is not null)
                {
                    return;
                }

                if (other.gameObject.CompareTag("Player"))
                {
                    Health playerHealth;
                    if ((playerHealth = other.gameObject.GetComponentInParent<Health>()) is not null)
                    {
                        AudioManagement.PlayClipAtPoint("HitmarkerSound", this.gameObject.transform.position);
                        playerHealth.TakeDamage(Damage);
                    }
                }
            }

            AudioManagement.RemoveFromMainAudioManagement();
            Destroy(this.gameObject);
        }
    }
}