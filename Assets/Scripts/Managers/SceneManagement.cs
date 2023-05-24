using System;
using System.Collections;
using Enums;
using PlayerScripts;
using SaveLoadSystem;
using UI.Menus;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Managers
{
    public class SceneManagement : MonoBehaviour
    {
        private static FadeManagement FadeManagement { get; set; }
        private static PauseMenu PauseMenu { get; set; }
     
        private void Awake()
        {
            FadeManagement = Utils.GetComponentOrThrow<FadeManagement>("Interface/MainCamera/FadeCanvas/Fade");
            if (IsPlayableScene(SceneManager.GetActiveScene().name))
            {
                PauseMenu = Utils.GetComponentOrThrow<PauseMenu>("Interface/MainCamera/UICanvas/PauseMenu");
            }
        }
        
        private void Start()
        {
            Time.timeScale = 1f;
            StartCoroutine(WaitForAnimation());
        }
     
        public void LoadSceneByType(SceneType sceneType)
        {
            switch (sceneType)
            { 
                case SceneType.Saved: 
                    LoadSavedScene(); 
                    break; 
                case SceneType.Next: 
                    LoadNextScene(); 
                    break; 
                case SceneType.MainMenu: 
                    LoadSceneByBuildIndex(0); 
                    break;
                default:
                    Debug.LogError($"Unknown SceneType: {sceneType}!");
                    break;
            }
        }

        public async void LoadSavedScene()
        {
            PlayerData playerData;
            try
            {
                playerData = await SaveDataManagement.LoadPlayerDataAsync(PlayerPrefs.GetString("ActivePlayer"));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                 
                return;
            }
            if (playerData is null)
            {
                Debug.LogError("PlayerData is null");
                 
                return;
            }
         
            LoadSceneByBuildIndex(playerData.SceneBuildIndex);
        }
     
        public void LoadNextScene()
        {
            var sceneBuildIndex = SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings
                ? 0
                : SceneManager.GetActiveScene().buildIndex + 1;
            LoadSceneByBuildIndex(sceneBuildIndex);
        }

        public void LoadSceneByName(string sceneName)
        {
            var sceneBuildIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
            LoadSceneByBuildIndex(sceneBuildIndex);
        }

        public void LoadSceneByBuildIndex(int sceneBuildIndex)
        {
            Cursor.visible = !IsPlayableScene(SceneUtility.GetScenePathByBuildIndex(sceneBuildIndex));
            FadeManagement.FadeOut();
            StartCoroutine(WaitForAnimation(sceneBuildIndex));
        }
        
        private IEnumerator WaitForAnimation(int? sceneBuildIndex = null)
        {
            if (IsPlayableScene(SceneManager.GetActiveScene().name))
            {
                PauseMenu.CanPause = false;
            }

            yield return new WaitUntil(() => !FadeManagement.IsFading);
            
            if (IsPlayableScene(SceneManager.GetActiveScene().name))
            {
                PauseMenu.CanPause = true;
            }
            
            if (sceneBuildIndex.HasValue)
            {
                SceneManager.LoadSceneAsync(sceneBuildIndex.Value);
            }
        }

        public bool IsPlayableScene(string sceneName)
        {
            return sceneName.Contains("Level") || sceneName.Contains("Boss");
        }
    }
}
