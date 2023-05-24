using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Newtonsoft.Json;
using Other;
using PlayerScripts;
using SaveLoadSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Managers
{
    public class DialogueManagement : MonoBehaviour
    {
        private AudioManagement AudioManagement { get; set; }
        private SceneManagement SceneManagement { get; set; }
        private RectTransform DialogueBoxRectTransform { get; set; }
        private TextMeshProUGUI CharacterNameText { get; set; }
        private TextMeshProUGUI DialogueLineText { get; set; }
        private GameObject PrevButtonDeactivatorGameObject { get; set; }
        private GameObject NextButtonDeactivatorGameObject { get; set; }
        private List<DialogueLine> DialogueLines { get; set; }
        private Coroutine PrintLettersCoroutine { get; set; }
        private int CurrentLineIndex { get; set; }

        private void Awake()
        {
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>("Interface/MainCamera/Audio/Sounds");
            SceneManagement = Utils.GetComponentOrThrow<SceneManagement>("Interface/MainCamera/SceneManager");
            DialogueBoxRectTransform = Utils.GetComponentOrThrow<RectTransform>("Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/DialogueBox");
            CharacterNameText = Utils.GetComponentOrThrow<TextMeshProUGUI>("Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/CharacterNameText");
            DialogueLineText = Utils.GetComponentOrThrow<TextMeshProUGUI>("Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/DialogueLineText");
            PrevButtonDeactivatorGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/ButtonsArea/PrevLineWrapper/ButtonDeactivator");
            NextButtonDeactivatorGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/ButtonsArea/NextLineWrapper/ButtonDeactivator");
        
            var textAsset = Utils.GetResourceOrThrow<TextAsset>("Text/Dialogue/" + SceneManager.GetActiveScene().name);
            if ((DialogueLines = JsonConvert.DeserializeObject<List<DialogueLine>>(textAsset.ToString())) is null)
            {
                Debug.LogError($"Failed to deserialize dialogue lines from {textAsset.name}!", this);
            }
        }

        private void Start()
        {
            CurrentLineIndex = 0;
        
            PrintLine();
        }

        public void PrevLine()
        {
            AudioManagement.PlayOneShot("ButtonSound");
            CurrentLineIndex -= 1;
        
            PrintLine();
        }
    
        public void NextLine()
        {
            AudioManagement.PlayOneShot("ButtonSound");
            CurrentLineIndex += 1;
        
            PrintLine();
        }

        public async void ContinueToLevel()
        {
            PlayerData playerData;
            try
            {
                playerData = await SaveDataManagement.LoadPlayerDataAsync(PlayerPrefs.GetString("ActivePlayer"));
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                return;
            }
            if (playerData is null)
            {
                return;
            }
        
            AudioManagement.PlayOneShot("ButtonSound");
        
            playerData.SceneBuildIndex += 1;
            try
            {
                await SaveDataManagement.SavePlayerDataAsync(playerData);
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                return;
            }
        
            SceneManagement.LoadSceneByType(SceneType.Saved);
        }

        private void PrintLine()
        {
            PrevButtonDeactivatorGameObject.gameObject.SetActive(false);
            NextButtonDeactivatorGameObject.gameObject.SetActive(false);

            if (CurrentLineIndex == 0)
            {
                PrevButtonDeactivatorGameObject.gameObject.SetActive(true);
            }
        
            if (CurrentLineIndex == (DialogueLines.Count - 1))
            {
                NextButtonDeactivatorGameObject.gameObject.SetActive(true);
            }
        
            RotateDialogueBox(DialogueLines[CurrentLineIndex].CharacterName);
            CharacterNameText.text = DialogueLines[CurrentLineIndex].CharacterName;

            if (PrintLettersCoroutine is not null)
            {
                StopCoroutine(PrintLettersCoroutine);
            }

            PrintLettersCoroutine = StartCoroutine(PrintLetters(DialogueLines[CurrentLineIndex].LineText));
        }

        private IEnumerator PrintLetters(string line)
        {
            DialogueLineText.text = "";
            foreach (var letter in line)
            {
                DialogueLineText.text += letter;
                yield return new WaitForSeconds(0.02f);
            }
        }

        private void RotateDialogueBox(string characterName)
        {
            if (characterName == "Boba Fett")
            {
                DialogueBoxRectTransform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            else
            {   
                DialogueBoxRectTransform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }
    }
}
