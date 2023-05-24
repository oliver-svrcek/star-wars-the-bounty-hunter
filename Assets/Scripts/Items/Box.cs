using System.Collections;
using Managers;
using Other;
using UnityEngine;
using Utilities;

namespace Items
{
    public class Box : MonoBehaviour
    {
        private AudioManagement AudioManagement { get; set; }
        private GameObject CoinGameObject { get; set; }
        private GameObject DirectionArrowHolder { get; set; }
        private GameObject LevelEndGameObject { get; set; }

        private void Awake()
        {
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>(this.gameObject);
            CoinGameObject = Utils.GetGameObjectOrThrow("Coin (1)");
            DirectionArrowHolder = Utils.GetGameObjectOrThrow("DirectionArrowHolder");
            LevelEndGameObject = Utils.GetGameObjectOrThrow("LevelEnd");
        }

        private IEnumerator Start()
        {
            DirectionArrowHolder.SetActive(false);
        
            yield return new WaitUntil(() => LevelEndGameObject.GetComponent<LevelEnd>().IsInitialized);
            LevelEndGameObject.SetActive(false);
        
            yield return new WaitUntil(() => CoinGameObject.GetComponent<Coin>().IsInitialized);
            if (CoinGameObject != null)
            {
                CoinGameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }
            
            DirectionArrowHolder.SetActive(true);
            LevelEndGameObject.SetActive(true);
            if (CoinGameObject != null)
            {
                CoinGameObject.SetActive(true);
            }
        
            AudioManagement.PlayClipAtPoint("CoinSpawnSound", this.gameObject.transform.position);
            AudioManagement.RemoveFromMainAudioManagement();
        
            Destroy(this.gameObject);
        }
    }
}
