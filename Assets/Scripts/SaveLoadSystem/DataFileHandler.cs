using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayerScripts;

namespace SaveLoadSystem
{
    public class DataFileHandler
    {
        public string DataPath { get; private set; }
        public string DataFileName { get; private set; }
        
        public DataFileHandler(string dataPath, string dataFileName)
        {
            DataPath = dataPath;
            DataFileName = dataFileName;
        }
        
        public async Task SaveDataAsync(PlayerData playerData)
        {
            var dataFilePath = Path.Combine(DataPath, playerData.PlayerName, DataFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(dataFilePath));
            
            var json = JsonConvert.SerializeObject(playerData, Formatting.Indented);

            await using var fileStream = new FileStream(dataFilePath, FileMode.Create);
            await using var streamWriter = new StreamWriter(fileStream);

            await streamWriter.WriteAsync(json);
        }
        
        public async Task<PlayerData> LoadDataAsync(string playerName)
        {
            var dataFilePath = Path.Combine(DataPath, playerName, DataFileName);
            
            await using var fileStream = new FileStream(dataFilePath, FileMode.Open);
            using var streamReader = new StreamReader(fileStream);
            
            var json = await streamReader.ReadToEndAsync();

            return JsonConvert.DeserializeObject<PlayerData>(json);
        }
        
        public async Task<List<PlayerData>> LoadAllDataAsync()
        {
            var playerDataList = new List<PlayerData>();
            
            foreach (var directory in Directory.GetDirectories(DataPath))
            {
                var dataFilePath = Path.Combine(directory, DataFileName);
                
                await using var fileStream = new FileStream(dataFilePath, FileMode.Open);
                using var streamReader = new StreamReader(fileStream);
                
                var json = await streamReader.ReadToEndAsync();
                
                playerDataList.Add(JsonConvert.DeserializeObject<PlayerData>(json));
            }

            return playerDataList;
        }

        public async Task DeleteDataAsync(string playerName, bool deleteDir = true)
        {
            var dataDirPath = Path.Combine(DataPath, playerName);
            var dataFilePath = Path.Combine(DataPath, playerName, DataFileName);
            
            if (deleteDir)
            {
                await Task.Run(() => Directory.Delete(dataDirPath, true));
            }
            else
            {
                await Task.Run(() => File.Delete(dataFilePath));
            }
        }
        
        public async Task DeleteAllDataAsync(bool deleteDirs = true)
        {
            foreach (var directory in Directory.GetDirectories(DataPath))
            {
                if (deleteDirs)
                {
                    await Task.Run(() => Directory.Delete(directory, true));
                }
                else
                {
                    var tasks = new List<Task>();
                    
                    foreach (var file in Directory.GetFiles(directory))
                    {
                        if (Path.GetFileName(file) == DataFileName)
                        {
                            tasks.Add(Task.Run(() => File.Delete(file)));
                        }
                    }
                    
                    await Task.WhenAll(tasks);
                }
            }
        }
        
        public async Task<bool> DataExistsAsync(string playerName)
        {
            var dataFilePath = Path.Combine(DataPath, playerName, DataFileName);
            
            return await Task.Run(() => File.Exists(dataFilePath));
        }
    }
}