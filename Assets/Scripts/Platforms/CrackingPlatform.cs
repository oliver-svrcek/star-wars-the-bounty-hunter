using System.Collections;
using Managers;
using UnityEngine;
using Utilities;

namespace Platforms
{
    public class CrackingPlatform : MonoBehaviour
    {
        private AudioManagement AudioManagement { get; set; }
        private Animator Animator { get; set; }
        private Coroutine DestroyCoroutine { set; get; }
        private float DestroyTime { get; set; }

        private void Awake()
        {
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>(this.gameObject);
            Animator = Utils.GetComponentOrThrow<Animator>(this.gameObject);
            
            DestroyCoroutine = null;
        }
        
        private void Start()
        {
            DestroyTime = 0.5f;
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player") || DestroyCoroutine is not null)
            {
                return;
            }
            
            AudioManagement.PlayOneShot("CrackingPlatformSound");
            Animator.enabled = true;
            
            DestroyCoroutine = StartCoroutine(Destroy());
        }
    
        private IEnumerator Destroy()
        {
            yield return new WaitForSeconds(DestroyTime);
            
            AudioManagement.RemoveFromMainAudioManagement();
            Destroy(this.gameObject);
        }
    }
}
