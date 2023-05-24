using System.Collections;
using PlayerScripts;
using UnityEngine;
using Utilities;

namespace Platforms
{
    public class MovingPlatform : MonoBehaviour
    {
        private Coroutine MoveStartDelayCoroutine { get; set; }
        private Vector3 MoveStartPosition { get; set; }
        private Vector3 MoveEndPosition { get; set; }
        private Vector3 MoveTargetPosition { get; set; }
        [field: SerializeField] private float Speed { get; set; } = 4f;
        [field: SerializeField] private float MoveStartDelayTime { get; set; } = 0f;

        protected void Awake()
        {
            MoveStartPosition = this.gameObject.transform.position;
            MoveEndPosition = Utils.GetGameObjectOrThrow(this.gameObject, "MoveEndPosition").transform.position;
            MoveTargetPosition = MoveEndPosition;
        }

        private void Start()
        {
            MoveStartDelayCoroutine = StartCoroutine(MoveStartDelay());
        }

        private void Update()
        {   
            if (MoveStartDelayCoroutine is null)
            {
                Move();
            }
        }

        private void Move()
        {
            if (Vector2.Distance(transform.position, MoveTargetPosition) < 0.01f)
            {
                if (MoveTargetPosition.Equals(MoveStartPosition))
                {
                    MoveTargetPosition = MoveEndPosition;
                }
                else
                {
                    MoveTargetPosition = MoveStartPosition;
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, MoveTargetPosition, Speed * Time.deltaTime);
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            other.transform.parent = this.transform;
        }
    
        private void OnTriggerExit2D(Collider2D other) 
        {
            other.transform.parent = null;
        }
    
        private IEnumerator MoveStartDelay()
        {
            yield return new WaitForSeconds(MoveStartDelayTime);
            MoveStartDelayCoroutine = null;
        }
    }
}
