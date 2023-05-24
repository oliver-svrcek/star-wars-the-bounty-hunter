using System;
using System.Collections.Generic;
using System.Reflection;
using Managers;
using PlayerScripts;
using SaveLoadSystem;
using TMPro;
using UnityEngine;
using Utilities;

namespace UI.Menus
{
    public class AdminMenu : MonoBehaviour
    {
        private AudioManagement AudioManagement { get; set; }
        private TMP_InputField PlayerNameInputField { get; set; }
        private TextMeshProUGUI PlayerDataTextArea { get; set; }
        private GameObject PrimaryMenuGameObject { get; set; }
        private GameObject UserTypeMenuGameObject { get; set; }
        private GameObject PlayerDataNotSavedWarningGameObject { get; set; }
        private GameObject PlayerDataNotLoadedWarningGameObject { get; set; }
        private GameObject PlayerDataNotDeletedWarningGameObject { get; set; }
        private GameObject PlayerNotFoundWarningGameObject { get; set; }
        private GameObject PlayerAlreadyExistsWaringGameObject { get; set; }
        private GameObject EmptyPlayerNameWarningGameObject { get; set; }
        private GameObject ConfirmDeleteWarningGameObject { get; set; }
        private GameObject ConfirmDeleteAllWarningGameObject { get; set; }

        private void Awake()
        {
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>("Interface/MainCamera/Audio/Sounds");
            PlayerNameInputField = Utils.GetComponentOrThrow<TMP_InputField>("Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/PlayerNameInputField");
            PlayerDataTextArea = Utils.GetComponentOrThrow<TextMeshProUGUI>("Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/PlayerStats/PlayerData/Viewport/Content");
            PrimaryMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu");
            UserTypeMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PrimaryMenu");
            PlayerDataNotLoadedWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PlayerDataNotSavedWarning");
            PlayerDataNotLoadedWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PlayerDataNotLoadedWarning");
            PlayerDataNotDeletedWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PlayerDataNotDeletedWarning");
            PlayerNotFoundWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PlayerNotFoundWarning");
            PlayerAlreadyExistsWaringGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PlayerAlreadyExistsWarning");
            EmptyPlayerNameWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/EmptyPlayerNameWarning");
            ConfirmDeleteWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/ConfirmDeleteWarning");
            ConfirmDeleteAllWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/ConfirmDeleteAllWarning");
        }
    
        private void Start()
        {
            PlayerNameInputField.text = "";
            LoadAllPlayerData();
        }
    
        public async void SearchPlayer()
        {
            if (PlayerNameInputField.text == "")
            {
                // load all if no name is given
            
                AudioManagement.PlayOneShot("ButtonSound");

                LoadAllPlayerData();
                return;
            }
            
            bool playerExists;
            try
            {
                playerExists = await SaveDataManagement.PlayerDataExistsAsync(PlayerNameInputField.text);
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                ShowErrorMessage(PlayerDataNotLoadedWarningGameObject);
                
                return;
            }
            if (!playerExists)
            {
                LoadAllPlayerData();
            
                ShowErrorMessage(PlayerNotFoundWarningGameObject);
            
                return;
            }

            AudioManagement.PlayOneShot("ButtonSound");
        
            LoadSinglePlayerData();
            PlayerNameInputField.text = "";
        }
    
        public async void AddPlayer()
        {
            if (PlayerNameInputField.text == "")
            {
                ShowErrorMessage(EmptyPlayerNameWarningGameObject);
                return;
            }
            
            bool playerExists;
            try
            {
                playerExists = await SaveDataManagement.PlayerDataExistsAsync(PlayerNameInputField.text);
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
        
            AudioManagement.PlayOneShot("ButtonSound");
            
            try
            {
                await SaveDataManagement.SavePlayerDataAsync(new PlayerData(PlayerNameInputField.text));
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                ShowErrorMessage(PlayerDataNotSavedWarningGameObject);
                
                return;
            }
        
            LoadAllPlayerData();
            PlayerNameInputField.text = "";
        }
    
        public async void DeletePlayer()
        {
            if (PlayerNameInputField.text == "")
            {
                // ask to delete all if no name is given
            
                AudioManagement.PlayOneShot("ButtonSound");
            
                PrimaryMenuGameObject.SetActive(false);
                ConfirmDeleteAllWarningGameObject.SetActive(true);
            
                return;
            }
        
            bool playerExists;
            try
            {
                playerExists = await SaveDataManagement.PlayerDataExistsAsync(PlayerNameInputField.text);
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
        
            PrimaryMenuGameObject.SetActive(false);
            ConfirmDeleteWarningGameObject.SetActive(true);
        }
    
        public async void DeletePlayerConfirm()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            try
            {
                await SaveDataManagement.DeletePlayerDataAsync(PlayerNameInputField.text);
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                ShowErrorMessage(PlayerDataNotDeletedWarningGameObject);
                
                return;
            }
        
            ConfirmDeleteWarningGameObject.SetActive(false);
            PrimaryMenuGameObject.SetActive(true);
        
            LoadAllPlayerData();
            PlayerNameInputField.text = "";
        }
    
        public async void DeleteAllPlayersConfirm()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            try
            {
                await SaveDataManagement.DeleteAllPlayerDataAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                ShowErrorMessage(PlayerDataNotDeletedWarningGameObject);
                
                return;
            }
        
            ConfirmDeleteAllWarningGameObject.SetActive(false);
            PrimaryMenuGameObject.SetActive(true);
        
            LoadAllPlayerData();
            PlayerNameInputField.text = "";
        }

        private async void LoadSinglePlayerData()
        {
            PlayerDataTextArea.text = "";

            PlayerData playerData;
            try
            {
                playerData = await SaveDataManagement.LoadPlayerDataAsync(PlayerNameInputField.text);
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
        
            PrintEntryLine(playerData);
        }
    
        private async void LoadAllPlayerData()
        {
            PlayerDataTextArea.text = "";
        
            List<PlayerData> playersData;
            try
            {
                playersData = await SaveDataManagement.LoadAllPlayerDataAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                ShowErrorMessage(PlayerDataNotLoadedWarningGameObject);
                
                return;
            }

            foreach (var playerData in playersData)
            {
                PrintEntryLine(playerData);
                PlayerDataTextArea.text += "\n";
            }
        }
    
        private void PrintEntryLine(PlayerData playerData)
        {
            if (playerData is null)
            {
                Debug.LogError("PlayerData is null", this);
                return;
            }
            
            var properties = typeof(PlayerData).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
            foreach (var property in properties)
            {
                if (IsPrimitiveType(property.PropertyType))
                {
                    PlayerDataTextArea.text += property.GetValue(playerData).ToString().PadRight(20);
                }
            }
        }
    
        private bool IsPrimitiveType(Type type)
        {
            return type.IsPrimitive || type == typeof(string);
        }
    
        private void ShowErrorMessage(GameObject warningGameObject)
        {
            AudioManagement.PlayOneShot("ErrorSound");
        
            PrimaryMenuGameObject.SetActive(false);
            warningGameObject.SetActive(true);
        
            PlayerNameInputField.text = "";
        }

        public void SwitchBackToPrimaryMenu()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            PlayerDataNotLoadedWarningGameObject.SetActive(false);
            PlayerNotFoundWarningGameObject.SetActive(false);
            PlayerAlreadyExistsWaringGameObject.SetActive(false);
            EmptyPlayerNameWarningGameObject.SetActive(false);
            ConfirmDeleteWarningGameObject.SetActive(false);
            ConfirmDeleteAllWarningGameObject.SetActive(false);
        
            PrimaryMenuGameObject.SetActive(true);
        }
    
        public void ExitToUserTypeMenu()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            PrimaryMenuGameObject.SetActive(false);
            UserTypeMenuGameObject.SetActive(true);
        
            LoadAllPlayerData();
            
            PlayerNameInputField.text = "";
        }
    }
}
