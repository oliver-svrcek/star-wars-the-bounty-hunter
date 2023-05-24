using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Managers
{
    public class FadeManagement : MonoBehaviour
    {
        private Image Image { get; set; }
        private Animator Animator { get; set; }
        public bool IsFading { get; private set; } = false;

        private void Awake()
        {
            Image = Utils.GetComponentOrThrow<Image>("Interface/MainCamera/FadeCanvas/Fade");
            Animator = Utils.GetComponentOrThrow<Animator>("Interface/MainCamera/FadeCanvas/Fade");
        
            FadeIn();
        }
    
        public void FadeIn()
        {
            IsFading = true;
        
            Animator.SetBool(nameof(FadeOut), false);
            Animator.SetBool(nameof(FadeIn), true);
        }

        public void FadeOut()
        {
            IsFading = true;
            Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        
            Animator.SetBool(nameof(FadeIn), false);
            Animator.SetBool(nameof(FadeOut), true);
        }
    
        public void ResetAnimation()
        {
            Animator.SetBool(nameof(FadeIn), false);
            Animator.SetBool(nameof(FadeOut), false);
        
            Image.enabled = false;
        }

        public void SetImageAlpha(byte alpha)
        {
            Image.color = new Color32(0, 0, 0, alpha); 
        }

        public void IsRunning(string isFading)
        {
            // unity animation event does not support bool parameter
            // therefore string parameter is used instead

            isFading = isFading.ToLower();
        
            switch (isFading)
            {
                case "true":
                    IsFading = true;
                    Image.enabled = true;
                    break;
                case "false":
                    IsFading = false;
                    Image.enabled = false;
                    break;
                default:
                    Debug.LogError($"Unknown block parameter: {isFading}!");
                    break;
            }
        }
    }
}
