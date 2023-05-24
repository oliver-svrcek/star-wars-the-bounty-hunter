using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerData
    {
        public string PlayerName { get; private set; }
        public int SceneBuildIndex { get; set; } = 0;
        public float PositionAxisX { get; set; } = 0;
        public float PositionAxisY { get; set; } = 0;
        public int HealthLevel { get; set; } = 1;
        public int BlasterLevel { get; set; } = 1;
        public int JetpackLevel { get; set; } = 1;
        public int FlamethrowerLevel { get; set; } = 1;
        public int CoinCount { get; set; } = 0;
        public Dictionary<string, List<string>> CollectedCoins { get; set; } = new Dictionary<string, List<string>>();

        public PlayerData(string playerName)
        {
            PlayerName = playerName;
        }

        public void ResetData()
        {
            SceneBuildIndex = 0;
            PositionAxisX = 0;
            PositionAxisY = 0;
            HealthLevel = 1;
            BlasterLevel = 1;
            JetpackLevel = 1;
            FlamethrowerLevel = 1;
            CoinCount = 0;
            CollectedCoins = new Dictionary<string, List<string>>();
        }
    }
}
