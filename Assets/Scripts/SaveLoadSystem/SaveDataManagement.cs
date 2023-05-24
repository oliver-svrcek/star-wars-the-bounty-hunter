using System.Collections.Generic;
using System.Threading.Tasks;
using PlayerScripts;
using UnityEngine;

namespace SaveLoadSystem
{
    public static class SaveDataManagement
    {
        private static string FileName { get; set; }
        private static DataFileHandler DataFileHandler { get; set; }
        
        static SaveDataManagement()
        {
            FileName = "PlayerData.json";
            DataFileHandler = new DataFileHandler(Application.persistentDataPath, FileName);
        }
        
        public static async Task SavePlayerDataAsync(PlayerData playerData)
        {
            await DataFileHandler.SaveDataAsync(playerData);
        }
        
        public static async Task<PlayerData> LoadPlayerDataAsync(string playerName)
        {
            return await DataFileHandler.LoadDataAsync(playerName);
        }
        
        public static async Task<List<PlayerData>> LoadAllPlayerDataAsync()
        {
            return await DataFileHandler.LoadAllDataAsync();
        }
        
        public static async Task DeletePlayerDataAsync(string playerName, bool deletePlayerDir = true)
        {
            await DataFileHandler.DeleteDataAsync(playerName, deletePlayerDir);
        }
        
        public static async Task DeleteAllPlayerDataAsync(bool deletePlayerDirs = true)
        {
            await DataFileHandler.DeleteAllDataAsync(deletePlayerDirs);
        }
        
        public static async Task<PlayerData> ResetPlayerDataAsync(string playerName)
        {
            var playerData = await DataFileHandler.LoadDataAsync(playerName);
            playerData.ResetData();
            await DataFileHandler.SaveDataAsync(playerData);
            return playerData;
        }
        
        public static async Task<bool> PlayerDataExistsAsync(string playerName)
        {
            return await DataFileHandler.DataExistsAsync(playerName);
        }
    }
}