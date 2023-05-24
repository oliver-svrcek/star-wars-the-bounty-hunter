using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class UpgradeLevels
    {
        public static Dictionary<int, Color32> UpgradeLevelsCollection { get; private set; }
        
        static UpgradeLevels()
        {
            UpgradeLevelsCollection = new Dictionary<int, Color32>();
            CreateUpgradeLevels();
        }
        
        private static void CreateUpgradeLevels()
        {
            LevelZero();
            LevelOne();
            LevelTwo();
            LevelThree();
            LevelFour();
        }

        private static void LevelZero()
        {
            UpgradeLevelsCollection.Add(0, new Color32(255, 255, 255, 255));
        }

        private static void LevelOne()
        {
            UpgradeLevelsCollection.Add(1, new Color32(150, 150, 150, 255));
        }
        
        private static void LevelTwo()
        {
            UpgradeLevelsCollection.Add(2, new Color32(0, 150, 255, 255));
        }
        
        private static void LevelThree()
        {
            UpgradeLevelsCollection.Add(3, new Color32(200, 0, 255, 255));
        }
        
        private static void LevelFour()
        {
            UpgradeLevelsCollection.Add(4, new Color32(255, 180, 0, 255));
        }
    }
}