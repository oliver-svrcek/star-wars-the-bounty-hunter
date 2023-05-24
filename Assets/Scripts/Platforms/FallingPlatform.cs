using Managers;
using UnityEngine;
using Utilities;

namespace Platforms
{
    public class FallingPlatform : MonoBehaviour
    {
        private AudioManagement AudioManagement { get; set; }
        private Rigidbody2D Rigidbody2D { get; set; }
        private GameObject DetectionAreaGameObject { get; set; }
        private GameObject SpikesGameObject { get; set; }
    
        private void Awake()
        {
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>(this.gameObject);
            Rigidbody2D = Utils.GetComponentOrThrow<Rigidbody2D>(this.gameObject);
            DetectionAreaGameObject = Utils.GetGameObjectOrThrow(this.gameObject, "DetectionArea");
            SpikesGameObject = Utils.GetGameObjectOrThrow(this.gameObject, "SpikesLong");
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }
            
            DetectionAreaGameObject.SetActive(false);
            AudioManagement.PlayOneShot("FallingPlatformSound");
            Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            SpikesGameObject.SetActive(true);
        }
    }
}
