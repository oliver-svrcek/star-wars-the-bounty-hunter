using System;
using System.Threading.Tasks;
using SaveLoadSystem;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerDataManagement
    {
        public PlayerData PlayerData { get; private set; }
        
        public PlayerDataManagement(string playerName)
        {
            InitializePlayerData(playerName);
        }
        
        private async void InitializePlayerData(string playerName)
        {
            try 
            {
                PlayerData = await SaveDataManagement.LoadPlayerDataAsync(playerName);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            if (PlayerData != null)
            {
                return;
            }
            
            PlayerData = new PlayerData(playerName);
            try
            {
                await SaveDataManagement.SavePlayerDataAsync(PlayerData);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        public async Task SavePlayerData()
        {
            try
            {
                await SaveDataManagement.SavePlayerDataAsync(PlayerData);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        public async Task LoadPlayerData()
        {
            try 
            {
                PlayerData = await SaveDataManagement.LoadPlayerDataAsync(PlayerData.PlayerName);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            if (PlayerData == null)
            {
                Debug.LogError($"PlayerData is null.");
            }
        }

        public async Task DeletePlayerData(bool deletePlayerDir = true)
        { 
            try 
            {
                await SaveDataManagement.DeletePlayerDataAsync(PlayerData.PlayerName, deletePlayerDir);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        public async Task ResetPlayerData()
        {
            try
            {
                PlayerData = await SaveDataManagement.ResetPlayerDataAsync(PlayerData.PlayerName);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            if (PlayerData == null)
            {
                Debug.LogError($"PlayerData is null.");
            }
        }
    }
}