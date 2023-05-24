using Managers;
using PlayerScripts;
using UnityEngine;
using Utilities;

namespace Platforms
{
    public class Spikes : MonoBehaviour
    {
        private AudioManagement AudioManagement { get; set; }
        private int Damage { get; set; }

        private void Awake()
        {
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>(this.gameObject);
        }
    
        private void Start()
        {
            Damage = int.MaxValue;
        }
    
        private void OnCollisionEnter2D(Collision2D other)
        {
            Health health;
            if ((health = other.gameObject.GetComponent<Health>()) is null)
            {
                return;
            }
        
            AudioManagement.PlayClipAtPoint("SpikeHitSound", other.gameObject.transform.position);
            health.TakeDamage(Damage);
        }
    }
}
