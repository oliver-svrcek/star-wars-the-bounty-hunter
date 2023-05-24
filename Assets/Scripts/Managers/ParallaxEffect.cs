using UnityEngine;
using Utilities;

namespace Managers
{
    public class ParallaxEffect : MonoBehaviour
    {
        [field: SerializeField] private float ParallaxEffectSpeed { get; set; } = 1f;
        private float Length { get; set; }
        private float StartPosition { get; set; }
        private GameObject MainCamera { get; set; }
        private SpriteRenderer SpriteRenderer { get; set; }

        private void Awake()
        {
            MainCamera = Utils.GetGameObjectOrThrow("Interface/MainCamera");
            SpriteRenderer = Utils.GetComponentOrThrow<SpriteRenderer>(this.gameObject);
        }

        private void Start()
        {
            StartPosition = this.gameObject.transform.position.x;
            Length = SpriteRenderer.bounds.size.x;
        }

        private void Update()
        {
            var temp = (MainCamera.transform.position.x * (1 - ParallaxEffectSpeed));
            var distance = (MainCamera.transform.position.x * ParallaxEffectSpeed) ;
        
            this.gameObject.transform.position = new Vector3(StartPosition + distance, this.gameObject.transform.position.y, this.gameObject.transform.position.z);

            if (temp > StartPosition + Length)
            {
                StartPosition += Length;
            }
            else if (temp < StartPosition - Length)
            {
                StartPosition -= Length;
            }
        }
    }
}
