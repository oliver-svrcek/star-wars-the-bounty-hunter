using System;
using Enums;
using SaveLoadSystem;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class GameEndingCreditsManagement : MonoBehaviour
    {
        private SceneManagement SceneManagement { get; set; }
    
        private void Awake()
        {
            SceneManagement = Utils.GetComponentOrThrow<SceneManagement>("Interface/MainCamera/SceneManager");
        }
    
        public async void EndGame()
        {
            try
            {
                await SaveDataManagement.ResetPlayerDataAsync(PlayerPrefs.GetString("ActivePlayer"));
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                return;
            }
        
            SceneManagement.LoadSceneByType(SceneType.MainMenu);
        }
    }
}
