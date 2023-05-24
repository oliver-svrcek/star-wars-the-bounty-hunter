using System;
using Managers;
using PlayerScripts;
using SaveLoadSystem;
using TMPro;
using UnityEngine;
using Utilities;

namespace UI.Menus
{
    public class PlayerLoginMenuManagement : MonoBehaviour
    {
        private AudioManagement AudioManagement { get; set; }
        private TMP_InputField PlayerNameInputField { get; set; }
        private GameObject PrimaryMenuGameObject { get; set; }
        private GameObject PlayerMainMenuGameObject { get; set; }
        private GameObject UserTypeMenuGameObject { get; set; }
        private GameObject PlayerDataNotSavedWarningGameObject { get; set; }
        private GameObject PlayerDataNotLoadedWarningGameObject { get; set; }
        private GameObject PlayerNotFoundWarningGameObject { get; set; }
        private GameObject PlayerAlreadyExistsWaringGameObject { get; set; }
        private GameObject EmptyPlayerNameWarningGameObject { get; set; }
    
        private void Awake()
        {
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>("Interface/MainCamera/Audio/Sounds");
            PlayerNameInputField = Utils.GetComponentOrThrow<TMP_InputField>("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PrimaryMenu/PlayerNameInputField");
            PrimaryMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PrimaryMenu");
            PlayerMainMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerMainMenu/PrimaryMenu");
            UserTypeMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PrimaryMenu");
            PlayerDataNotSavedWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerDataNotSavedWarning");
            PlayerDataNotLoadedWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerDataNotLoadedWarning");
            PlayerNotFoundWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerNotFoundWarning");
            PlayerAlreadyExistsWaringGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerAlreadyExistsWarning");
            EmptyPlayerNameWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/EmptyPlayerNameWarning");
        }
    
        public async void PlayerRegister()
        {
            var playerName = PlayerNameInputField.text;
    
            if (playerName == "")
            {
                ShowErrorMessage(EmptyPlayerNameWarningGameObject);
            
                return;
            }

            bool playerExists;
            try
            {
                playerExists = await SaveDataManagement.PlayerDataExistsAsync(playerName);
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                ShowErrorMessage(PlayerDataNotLoadedWarningGameObject);
                
                return;
            }
            if (playerExists)
            {
                ShowErrorMessage(PlayerAlreadyExistsWaringGameObject);
            
                return;
            }
        
            try
            {
                await SaveDataManagement.SavePlayerDataAsync(new PlayerData(playerName));
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                ShowErrorMessage(PlayerDataNotSavedWarningGameObject);
            
                return;
            }
        
            AudioManagement.PlayOneShot("ButtonSound");
        
            PlayerPrefs.SetString("ActivePlayer", playerName);
        
            SwitchToPlayerMainMenu();
        }

        public async void PlayerLogin()
        {
            var playerName = PlayerNameInputField.text;
    
            if (playerName == "")
            {
                ShowErrorMessage(EmptyPlayerNameWarningGameObject);
            
                return;
            }
        
            bool playerExists;
            try
            {
                playerExists = await SaveDataManagement.PlayerDataExistsAsync(playerName);
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                ShowErrorMessage(PlayerDataNotLoadedWarningGameObject);
                
                return;
            }
            if (!playerExists)
            {
                ShowErrorMessage(PlayerNotFoundWarningGameObject);
            
                return;
            }
        
            AudioManagement.PlayOneShot("ButtonSound");
        
            PlayerPrefs.SetString("ActivePlayer", playerName);
        
            SwitchToPlayerMainMenu();
        }
    
        private void ShowErrorMessage(GameObject warningGameObject)
        {
            AudioManagement.PlayOneShot("ErrorSound");
        
            PrimaryMenuGameObject.SetActive(false);
            warningGameObject.SetActive(true);
        
            PlayerNameInputField.text = "";
        }

        private void SwitchToPlayerMainMenu()
        {
            PrimaryMenuGameObject.SetActive(false);
            PlayerMainMenuGameObject.SetActive(true);
        
            PlayerNameInputField.text = "";
        }
    
        public void SwitchBackToPrimaryMenu()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            PlayerDataNotSavedWarningGameObject.SetActive(false);
            PlayerNotFoundWarningGameObject.SetActive(false);
            PlayerAlreadyExistsWaringGameObject.SetActive(false);
            EmptyPlayerNameWarningGameObject.SetActive(false);
        
            PrimaryMenuGameObject.SetActive(true);
        }
    
        public void ExitToUserTypeMenu()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            PrimaryMenuGameObject.SetActive(false);
            UserTypeMenuGameObject.SetActive(true);
        
            PlayerNameInputField.text = "";
        }
    }
}
