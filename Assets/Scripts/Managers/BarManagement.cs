using Enums;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Managers
{
    public class BarManagement : MonoBehaviour
    {
        private Slider Slider { get; set; }
        private Image Fill { get; set; }
        private Gradient Gradient { get; set; }
        private Quaternion FixedRotation { get; set; }
    
        private void Awake()
        {
            Slider = Utils.GetComponentOrThrow<Slider>(this.gameObject);
            Fill = Utils.GetComponentOrThrow<Image>(this.gameObject, "Fill");
        }

        private void Start()
        {
            FixedRotation = transform.rotation;
        }

        private void LateUpdate()
        {
            // Fix rotation of bar slider
            transform.rotation = FixedRotation;
        }
    
        public void SetBar(BarType barType, float maxValue, float value)
        {
            SetGradient(barType);
            SetMaxValue(maxValue);
            SetValue(value);
        }
    
        public float GetMaxValue()
        {
            return Slider.maxValue;
        }

        public void SetMaxValue(float value)
        {
            Slider.maxValue = value;
            Fill.color = Gradient.Evaluate(1f);
        }

        public float GetValue()
        {
            return Slider.value;
        }
    
        public void SetValue(float value)
        {
            Slider.value = value;
            Fill.color = Gradient.Evaluate(Slider.normalizedValue);
        }

        public void SetGradient(BarType barType)
        {
            if (!Gradients.GradientCollection.TryGetValue(barType, out var gradient))
            {
                Debug.LogError($"Gradient for {barType} not found.");
                return;
            }
        
            Gradient = gradient;
        
            Slider = this.gameObject.GetComponent<Slider>();
            Fill = this.gameObject.transform.Find("Fill").GetComponent<Image>();
        
            Fill.color = Gradient.Evaluate(Slider.normalizedValue);
        }
    }
}