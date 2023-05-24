using System.Collections;
using Managers;
using PlayerScripts;
using UnityEngine;
using Utilities;

namespace Items
{
    public class Checkpoint : MonoBehaviour
    {
        private PlayerDataManagement PlayerDataManagement { get; set; }
        private AudioManagement AudioManagement { get; set; }
        private BoxCollider2D BoxCollider2D { get; set; }
        private Animator Animator { get; set; }
        public bool IsInitialized { get; private set; } = false;
        private void Awake()
        {
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>(this.gameObject);
            BoxCollider2D = Utils.GetComponentOrThrow<BoxCollider2D>(this.gameObject);
            Animator = Utils.GetComponentOrThrow<Animator>(this.gameObject);
        }

        private IEnumerator Start()
        {
            PlayerDataManagement = Utils.GetComponentOrThrow<Player>("Player").PlayerDataManagement;
            yield return new WaitUntil(() => PlayerDataManagement.PlayerData != null);

            Animator.enabled = false;
            BoxCollider2D.enabled = true;
        
            if (PlayerDataManagement.PlayerData.PositionAxisX - 1f > this.gameObject.transform.position.x)
            {
                // checkpoint is already passed
                this.gameObject.SetActive(false);
            }
            else if (Mathf.Abs(PlayerDataManagement.PlayerData.PositionAxisX - this.gameObject.transform.position.x) < 1f)
            {
                // checkpoint is active
                Animator.enabled = true;
                BoxCollider2D.enabled = false;
            }
        
            IsInitialized = true;
        }

        private async void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }
        
            AudioManagement.PlayClipAtPoint("CheckpointSound", this.gameObject.transform.position);
        
            PlayerDataManagement.PlayerData.PositionAxisX = this.gameObject.transform.position.x;
            PlayerDataManagement.PlayerData.PositionAxisY = this.gameObject.transform.position.y;
        
            await PlayerDataManagement.SavePlayerData();
        
            Animator.enabled = true;
            BoxCollider2D.enabled = false;
        }
    }
}
