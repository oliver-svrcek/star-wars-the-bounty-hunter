using System;
using System.Collections;
using Items;
using UnityEngine;
using Utilities;

namespace Enemies
{
    public abstract class EnemyShooter : Enemy
    {
        private Transform Firepoint { get; set; }
        private GameObject BulletPrefab { get; set; }
        protected Coroutine ShootCoroutine { get; set; }
        protected Coroutine ShootStartDelayCoroutine { get; set; }
        [field: SerializeField] protected float ShootingRate { get; set; } = 1f;
        [field: SerializeField] protected int BulletsPerShot { get; set; } = 1;
        [field: SerializeField] protected int BulletDamage { get; set; } = 0;
        [field: SerializeField] protected float BulletSpeed { get; set; } = 20f;
        [field: SerializeField] private string BulletSound { get; set; } = "EnemyBlasterShotSound";
        [field: SerializeField] private float ViewRangeHorizontal { get; set; } = 11;
        [field: SerializeField] private float ViewRangeVertical { get; set; } = 1.8f;

        protected new void Awake()
        {
            base.Awake();

            Firepoint = Utils.GetGameObjectOrThrow(this.gameObject, "Firepoint").transform;
            BulletPrefab = Utils.GetResourceOrThrow<GameObject>("Prefabs/Objects/Bullet");
        }

        protected new void Start()
        {
            base.Start();
        }

        protected new void Update()
        {
            base.Update();
        
            DetectPlayer();
        }

        private void DetectPlayer()
        {
            var horizontalDistance = Player.transform.position.x - this.transform.position.x;
            var verticalDistance = Player.transform.position.y - this.transform.position.y;
        
        
            if (Math.Abs(horizontalDistance) < ViewRangeHorizontal &&
                Math.Abs(verticalDistance) < ViewRangeVertical)
            {
                if (ShootCoroutine is null && ShootStartDelayCoroutine is null)
                {
                    ShootCoroutine = StartCoroutine(Shoot());
                }
            }
            else if (ShootCoroutine is not null)
            {
                StopCoroutine(ShootCoroutine);
                ShootCoroutine = null;

                if (ShootStartDelayCoroutine is null)
                {
                    ShootStartDelayCoroutine = StartCoroutine(ShootStartDelay());
                }
            }
        }

        protected IEnumerator Shoot()
        {
            while (true)
            {
                for (var i = 0; i < BulletsPerShot; i++)
                {
                    SpawnBullet();
                    yield return new WaitForSeconds(0.1f);
                }
            
                yield return new WaitForSeconds(ShootingRate);
            }
        }
    
        private IEnumerator ShootStartDelay()
        {
            yield return new WaitForSeconds(ShootingRate);
            
            ShootStartDelayCoroutine = null;
        }

        protected void SpawnBullet()
        {
            var bullet = Instantiate(BulletPrefab, Firepoint.position, Firepoint.rotation);
            bullet.GetComponent<Bullet>().Initialize(this.gameObject, BulletDamage, BulletSpeed);
        
            AudioManagement.PlayOneShot(BulletSound);
        }
    }
}
