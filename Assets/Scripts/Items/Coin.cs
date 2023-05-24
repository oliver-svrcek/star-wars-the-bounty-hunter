using System.Collections;
using System.Collections.Generic;
using Managers;
using PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Items
{
    public class Coin : MonoBehaviour
    {
        private PlayerDataManagement PlayerDataManagement { get; set; }
        private AudioManagement AudioManagement { get; set; }
        public bool IsInitialized { get; private set; } = false;

        private void Awake()
        {
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>(this.gameObject);
        }

        private IEnumerator Start()
        {
            PlayerDataManagement = Utils.GetComponentOrThrow<Player>("Player").PlayerDataManagement;
            yield return new WaitUntil(() => PlayerDataManagement.PlayerData != null);

            var levelName = SceneManager.GetActiveScene().name;
            if (!PlayerDataManagement.PlayerData.CollectedCoins.ContainsKey(levelName))
            {
                PlayerDataManagement.PlayerData.CollectedCoins.Add(levelName, new List<string>());
            }
            else if (PlayerDataManagement.PlayerData.CollectedCoins[levelName].Contains(this.gameObject.name))
            {
                AudioManagement.RemoveFromMainAudioManagement();
                Destroy(this.gameObject);  
            }
        
            IsInitialized = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;    
            }
        
            AudioManagement.PlayClipAtPoint("CoinPickupSound", this.gameObject.transform.position);
        
            PlayerDataManagement.PlayerData.CoinCount += 1;
            PlayerDataManagement.PlayerData.CollectedCoins[SceneManager.GetActiveScene().name].Add(this.gameObject.name);
        
            AudioManagement.RemoveFromMainAudioManagement();
            Destroy(this.gameObject);
        }
    }
}
