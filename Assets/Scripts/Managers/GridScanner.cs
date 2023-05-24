using System.Collections;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class GridScanner : MonoBehaviour
    {
        private AstarPath AstarPath { get; set; } 
        public Coroutine ScanContinuouslyCoroutine { get; private set; }

        private void Awake()
        {
            AstarPath = Utils.GetComponentOrThrow<AstarPath>(this.gameObject);
        }

        private void Start()
        {
            ScanContinuously(true);
        }

        public void ScanOnce()
        {
            AstarPath.ScanAsync(AstarPath.graphs[0]);
        }
    
        public void ScanContinuously(bool scanOn)
        {
            if (scanOn && ScanContinuouslyCoroutine == null)
            {
                ScanContinuouslyCoroutine = StartCoroutine(ScanContinuously());
            }
            else if (!scanOn && ScanContinuouslyCoroutine != null)
            {
                StopCoroutine(ScanContinuouslyCoroutine);
                ScanContinuouslyCoroutine = null;
            }
        }
    
        private IEnumerator ScanContinuously()
        {
            while (true)
            {
                AstarPath.ScanAsync(AstarPath.graphs[0]);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
