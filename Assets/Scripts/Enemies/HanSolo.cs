using System.Collections;
using System.Collections.Generic;
using Items;
using Other;
using UnityEngine;
using Utilities;
using Random=System.Random;

namespace Enemies
{
    public class HanSolo : EnemyShooter
    {
        private Vector3 TopPosition { get; set; }
        private Vector3 MiddlePosition { get; set; }
        private Vector3 BottomPosition { get; set; }
        private GameObject DetoniteChargePrefab { get; set; }
        private GameObject Coin1GameObject { get; set; }
        private GameObject Coin2GameObject { get; set; }
        private GameObject DirectionArrowHolder { get; set; }
        private GameObject LevelEndGameObject { get; set; }
        private GameObject HanSoloSurrenderGameObject { get; set; }
        private List<Vector3> DetoniteChargePositions { get; set; }

        private new void Awake()
        {
            base.Awake();
        
            TopPosition = Utils.GetGameObjectOrThrow(this.gameObject, "Positions/Top").transform.position;
            MiddlePosition = Utils.GetGameObjectOrThrow(this.gameObject, "Positions/Middle").transform.position;
            BottomPosition = Utils.GetGameObjectOrThrow(this.gameObject, "Positions/Bottom").transform.position;
            DetoniteChargePrefab = Utils.GetResourceOrThrow<GameObject>("Prefabs/Objects/DetoniteCharge");
            Coin1GameObject = Utils.GetGameObjectOrThrow("Coin (1)");
            Coin2GameObject = Utils.GetGameObjectOrThrow("Coin (2)");
            DirectionArrowHolder = Utils.GetGameObjectOrThrow("DirectionArrowHolder");
            LevelEndGameObject = Utils.GetGameObjectOrThrow("LevelEnd");
            HanSoloSurrenderGameObject = Utils.GetGameObjectOrThrow("HanSoloSurrender");

            DetoniteChargePositions = new List<Vector3>();
            var detoniteChargePositionsGameObject = Utils.GetGameObjectOrThrow(this.gameObject, "DetoniteChargePositions");
            foreach (Transform child in detoniteChargePositionsGameObject.transform)
            {
                DetoniteChargePositions.Add(child.position);
            }
        }
    
        private new IEnumerator Start()
        {
            if (!UseOnlyEditorValues)
            {
                CanHeal = true;
                MaximumHealth = 200000;
                CurrentHealth = MaximumHealth;
                BulletDamage = 9999;
                BulletsPerShot = 1;
                BulletSpeed = 20f;
                ShootingRate = 1.25f;
                HealStartTime = 2f;
                HealPoints = 15;
                DeathSound = "HanSoloDeathSound";
            }
            
            base.Start();

            DirectionArrowHolder.SetActive(false);
            HanSoloSurrenderGameObject.SetActive(false);
        
            yield return new WaitUntil(() => LevelEndGameObject.GetComponent<LevelEnd>().IsInitialized);
            LevelEndGameObject.SetActive(false);
        
            yield return new WaitUntil(() => Coin1GameObject.GetComponent<Coin>().IsInitialized);
            if (Coin1GameObject != null)
            {
                Coin1GameObject.SetActive(false);
            }
        
            yield return new WaitUntil(() => Coin2GameObject.GetComponent<Coin>().IsInitialized);
            if (Coin2GameObject != null)
            {
                Coin2GameObject.SetActive(false);
            }
        
            ShootCoroutine = StartCoroutine(Shoot());
            StartCoroutine(ChangePosition());
            StartCoroutine(SpawnDetoniteCharges());
        }

        private new void Update()
        {
            LookAtPlayer();
        }

        private new IEnumerator ChangePosition()
        {
            var random = new Random();
            var lastPosition = BottomPosition;

            yield return new WaitForSeconds(5f);
        
            while (true)
            {
                var positions = new List<Vector3> {TopPosition, MiddlePosition, BottomPosition};
                positions.Remove(lastPosition);
            
                var randomPositionIndex = random.Next(positions.Count);
                var randomPosition = positions[randomPositionIndex];
            
                this.transform.position = randomPosition;
                lastPosition = randomPosition;
            
                StopCoroutine(ShootCoroutine);
                ShootCoroutine = null;
                yield return new WaitForSeconds(0.75f);
                ShootCoroutine = StartCoroutine(Shoot());
            
                yield return new WaitForSeconds(5f);
            }
        }
    
        private IEnumerator SpawnDetoniteCharges()
        {
            var random = new Random();
        
            while (true)
            {
                yield return new WaitForSeconds(25f);

                while (true)
                {
                    if (random.Next(3) == 1)
                    {
                        foreach (var detoniteChargePosition in DetoniteChargePositions)
                        {
                            var detoniteCharge = Instantiate(DetoniteChargePrefab, detoniteChargePosition, Quaternion.identity);
                            detoniteCharge.GetComponent<DetoniteCharge>().Activate();
                        }
                    
                        break;
                    }
                
                    yield return new WaitForSeconds(1f);
                }
            }
        }
    
        public void Surrender()
        {
            HanSoloSurrenderGameObject.SetActive(true);
        }
    }
}