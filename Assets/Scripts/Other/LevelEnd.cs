using System.Collections;
using Managers;
using PlayerScripts;
using UnityEngine;
using Utilities;

namespace Other
{
    public class LevelEnd : MonoBehaviour
    {
        private PlayerDataManagement PlayerDataManagement { get; set; }
        private SceneManagement SceneManagement { get; set; }
        private BoxCollider2D BoxCollider2D { get; set; }
        public bool IsInitialized { get; set; } = false;
        private void Awake()
        {
            SceneManagement = Utils.GetComponentOrThrow<SceneManagement>("Interface/MainCamera/SceneManager");
            BoxCollider2D = Utils.GetComponentOrThrow<BoxCollider2D>(this.gameObject);
        }
    
        private IEnumerator Start()
        {
            PlayerDataManagement = Utils.GetComponentOrThrow<Player>("Player").PlayerDataManagement;
            yield return new WaitUntil(() => PlayerDataManagement.PlayerData != null);
            
            IsInitialized = true;
        }

        private async void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

            PlayerDataManagement.PlayerData.SceneBuildIndex += 1;
            PlayerDataManagement.PlayerData.PositionAxisX = 0;
            PlayerDataManagement.PlayerData.PositionAxisY = 0;
            await PlayerDataManagement.SavePlayerData();
        
            BoxCollider2D.enabled = false;
            StartCoroutine(EndLevel());
        }


        private IEnumerator EndLevel()
        {
            yield return new WaitForSeconds(3f);
            SceneManagement.LoadSavedScene();
        }
    }
}
