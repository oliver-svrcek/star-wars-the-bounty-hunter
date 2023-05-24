using Managers;
using UnityEngine;
using Utilities;

namespace Items
{
    public class HanSoloSurrender : MonoBehaviour
    {
        private AudioManagement AudioManagement { get; set; }
        private GameObject Coin1GameObject { get; set; }
        private GameObject Coin2GameObject { get; set; }
        private GameObject DirectionArrowHolder { get; set; }
        private GameObject LevelEndGameObject { get; set; }

        private void Awake()
        {
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>(this.gameObject);
            Coin1GameObject = Utils.GetGameObjectOrThrow("Coin (1)");
            Coin2GameObject = Utils.GetGameObjectOrThrow("Coin (2)");
            DirectionArrowHolder = Utils.GetGameObjectOrThrow("DirectionArrowHolder");
            LevelEndGameObject = Utils.GetGameObjectOrThrow("LevelEnd");
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player"))
            {
                return;
            }

            LevelEndGameObject.SetActive(true);
            DirectionArrowHolder.SetActive(true);
            Coin1GameObject.SetActive(true);
            Coin2GameObject.SetActive(true);
        
            AudioManagement.PlayClipAtPoint("CoinSpawnSound", this.gameObject.transform.position);
            AudioManagement.RemoveFromMainAudioManagement();
            Destroy(this.gameObject);
        }
    }
}
