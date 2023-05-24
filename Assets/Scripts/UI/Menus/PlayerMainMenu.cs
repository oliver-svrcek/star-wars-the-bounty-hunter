using System;
using Enums;
using Managers;
using PlayerScripts;
using SaveLoadSystem;
using UnityEngine;
using Utilities;

namespace UI.Menus
{
    public class PlayerMainMenu : MonoBehaviour
    {
        private AudioManagement AudioManagement { get; set; }
        private SceneManagement SceneManagement { get; set; }
        private GameObject PrimaryMenuGameObject { get; set; }
        private GameObject PlayerLoginMenuGameObject { get; set; }
        private GameObject PlayerDataNotSavedWarningGameObject { get; set; }
        private GameObject PlayerDataNotLoadedWarningGameObject { get; set; }
        private GameObject GameSaveNotFoundWarningGameObject { get; set; }
        private GameObject GameSaveOverwriteWarningGameObject { get; set; }
    
        private void Awake()
        {
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>("Interface/MainCamera/Audio/Sounds");
            SceneManagement = Utils.GetComponentOrThrow<SceneManagement>("Interface/MainCamera/SceneManager");
            PrimaryMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerMainMenu/PrimaryMenu");
            PlayerLoginMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PrimaryMenu");
            PlayerDataNotSavedWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerDataNotSavedWarning");
            PlayerDataNotLoadedWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerDataNotLoadedWarning");
            GameSaveNotFoundWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerMainMenu/GameSaveNotFoundWarning");
            GameSaveOverwriteWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerMainMenu/GameSaveOverwriteWarning");
        }

        public async void ContinueGame()
        {
            PlayerData playerData;
            try
            {
                playerData = await SaveDataManagement.LoadPlayerDataAsync(PlayerPrefs.GetString("ActivePlayer"));
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                ShowErrorMessage(PlayerDataNotLoadedWarningGameObject);
            
                return;
            }
            if (playerData is null)
            {
                ShowErrorMessage(PlayerDataNotLoadedWarningGameObject);
                return;
            }
        
            if (playerData.SceneBuildIndex == 0)
            {
                ShowErrorMessage(GameSaveNotFoundWarningGameObject);
                return;
            }
        
            AudioManagement.PlayOneShot("ButtonSound");
        
            SceneManagement.LoadSavedScene();
        }

        public async void NewGame()
        {
            PlayerData playerData;
            try
            {
                playerData = await SaveDataManagement.LoadPlayerDataAsync(PlayerPrefs.GetString("ActivePlayer"));
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                ShowErrorMessage(PlayerDataNotLoadedWarningGameObject);
            
                return;
            }
            if (playerData is null)
            {
                Debug.LogError("PlayerData is null", this);
                ShowErrorMessage(PlayerDataNotLoadedWarningGameObject);
            
                return;
            }
        
            AudioManagement.PlayOneShot("ButtonSound");
        
            if (playerData.SceneBuildIndex != 0)
            {
                PrimaryMenuGameObject.SetActive(false);
                GameSaveOverwriteWarningGameObject.SetActive(true);
            
                return;
            }

            playerData.SceneBuildIndex = 1;
            try
            {
                await SaveDataManagement.SavePlayerDataAsync(playerData);
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                ShowErrorMessage(PlayerDataNotSavedWarningGameObject);
            
                return;
            }
        
            SceneManagement.LoadSavedScene();
        }
    
        public async void OverwriteGame()
        {
            PlayerData playerData;
            try
            {
                playerData = await SaveDataManagement.ResetPlayerDataAsync(PlayerPrefs.GetString("ActivePlayer"));
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                ShowErrorMessage(PlayerDataNotSavedWarningGameObject);
            
                return;
            }
        
            AudioManagement.PlayOneShot("ButtonSound");
        
            playerData.SceneBuildIndex = 1;
            try
            {
                await SaveDataManagement.SavePlayerDataAsync(playerData);
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                ShowErrorMessage(PlayerDataNotSavedWarningGameObject);
            
                return;
            }
        
            SceneManagement.LoadSavedScene();
        }
    
        private void ShowErrorMessage(GameObject warningGameObject)
        {
            AudioManagement.PlayOneShot("ErrorSound");
        
            PrimaryMenuGameObject.SetActive(false);
            warningGameObject.SetActive(true);
        }
    
        public void SwitchBackToPrimaryMenu()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            GameSaveNotFoundWarningGameObject.SetActive(false);
            GameSaveOverwriteWarningGameObject.SetActive(false);
        
            PrimaryMenuGameObject.SetActive(true);
        }
    
        public void ExitPlayerLoginMenu()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            PrimaryMenuGameObject.SetActive(false);
            PlayerLoginMenuGameObject.SetActive(true);
        }
    }
}
