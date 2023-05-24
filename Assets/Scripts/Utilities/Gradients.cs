using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Utilities
{
    public static class Gradients
    {
        public static Dictionary<BarType, Gradient> GradientCollection { get; private set; }
        
        static Gradients()
        {
            GradientCollection = new Dictionary<BarType, Gradient>();
            CreateGradients();
        }

        private static void CreateGradients()
        {
            IncreasingGradient();
            DecreasingGradient();
            RechargingGradient();
            EnemyHealthGradient();
        }
        
        private static void IncreasingGradient()
        {
            var increasingGradient = new Gradient();

            var colorKey = new GradientColorKey[3];
            colorKey[0].color = new Color32(0, 255, 0, 255);
            colorKey[0].time = 0f;
            colorKey[1].color = new Color32(255, 255, 0, 255);
            colorKey[1].time = 0.5f;
            colorKey[2].color = new Color32(255, 0, 0, 255);
            colorKey[2].time = 1f;
            
            var alphaKey = new GradientAlphaKey[3];
            alphaKey[0].alpha = 1f;
            alphaKey[0].time = 0f;
            alphaKey[1].alpha = 1f;
            alphaKey[1].time = 0.5f;
            alphaKey[2].alpha = 1f;
            alphaKey[2].time = 1f;
            
            increasingGradient.SetKeys(colorKey, alphaKey);
            GradientCollection.Add(BarType.Increasing, increasingGradient);
        }
        
        private static void DecreasingGradient()
        {
            var decreasingGradient = new Gradient();
            
            var colorKey = new GradientColorKey[3];
            colorKey[0].color = new Color32(255, 0, 0, 255);
            colorKey[0].time = 0f;
            colorKey[1].color = new Color32(255, 255, 0, 255);
            colorKey[1].time = 0.5f;
            colorKey[2].color = new Color32(0, 255, 0, 255);
            colorKey[2].time = 1f;
            
            var alphaKey = new GradientAlphaKey[3];
            alphaKey[0].alpha = 1f;
            alphaKey[0].time = 0f;
            alphaKey[1].alpha = 1f;
            alphaKey[1].time = 0.5f;
            alphaKey[2].alpha = 1f;
            alphaKey[2].time = 1f;
            
            decreasingGradient.SetKeys(colorKey, alphaKey);
            GradientCollection.Add(BarType.Decreasing, decreasingGradient);
        }
        
        private static void RechargingGradient()
        {
            var rechargingGradient = new Gradient();
            
            var colorKey = new GradientColorKey[1];
            colorKey[0].color = new Color32(140, 0, 0, 255);
            colorKey[0].time = 0f;
            
            var alphaKey = new GradientAlphaKey[1];
            alphaKey[0].alpha = 1f;
            alphaKey[0].time = 0f;
            
            rechargingGradient.SetKeys(colorKey, alphaKey);
            GradientCollection.Add(BarType.Recharging, rechargingGradient);
        }

        private static void EnemyHealthGradient()
        {
            var enemyHealthGradient = new Gradient();

            var colorKey = new GradientColorKey[1];
            colorKey[0].color = new Color32(255, 0, 0, 255);
            colorKey[0].time = 0f;

            var alphaKey = new GradientAlphaKey[1];
            alphaKey[0].alpha = 1f;
            alphaKey[0].time = 0f;

            enemyHealthGradient.SetKeys(colorKey, alphaKey);
            GradientCollection.Add(BarType.EnemyHealth, enemyHealthGradient);
        }
    }
}