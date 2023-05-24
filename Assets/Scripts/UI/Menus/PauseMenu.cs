using Enums;
using Managers;
using UnityEngine;
using Utilities;

namespace UI.Menus
{
    public class PauseMenu : MonoBehaviour
    {
        private AudioManagement AudioManagement { get; set; }
        private SceneManagement SceneManagement { get; set; }
        private GameObject PrimaryMenuGameObject { get; set; }
        private GameObject UpgradesMenuGameObject { get; set; }
        private GameObject ExitWarningGameObject { get; set; }
        private UpgradesMenu UpgradesMenu { get; set; }
        public bool CanPause { get; set; }
        public bool IsPaused { get; private set; }

        private void Awake()
        {
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>("Interface/MainCamera/Audio/Sounds");
            SceneManagement = Utils.GetComponentOrThrow<SceneManagement>("Interface/MainCamera/SceneManager");
            PrimaryMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/PauseMenu/PrimaryMenu");
            UpgradesMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu");
            ExitWarningGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/PauseMenu/ExitWarning");
            UpgradesMenu = Utils.GetComponentOrThrow<UpgradesMenu>("Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu");
        }

        private void Start()
        {
            CanPause = true;
            IsPaused = false;
        }

        public void ResumeClick()
        {
            Resume();
            AudioManagement.PlayOneShot("ButtonSound");
        }
    
        public void Resume()
        {
            Cursor.visible = false;
        
            PrimaryMenuGameObject.SetActive(false);
            UpgradesMenuGameObject.SetActive(false);
            ExitWarningGameObject.SetActive(false);
        
            MainAudioManagement.SetPauseAll(false);
            Time.timeScale = 1f;
            IsPaused = false;
            CanPause = true;
        }
    
        public void Pause()
        {
            if (IsPaused)
            {
                Resume();
                return;
            }
        
            if (!CanPause)
            {
                return;
            }

            Cursor.visible = true;
        
            PrimaryMenuGameObject.SetActive(true);
        
            MainAudioManagement.SetPauseAll(true);
            Time.timeScale = 0f;
            IsPaused = true;
            CanPause = false;
        }

        public void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }
    
        public void SwitchBackToPrimaryMenu()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            UpgradesMenuGameObject.SetActive(false);
            ExitWarningGameObject.SetActive(false);
        
            PrimaryMenuGameObject.SetActive(true);
        }

        public void SwitchToUpgradesMenu()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            UpgradesMenu.RefreshGearLevels();
        
            PrimaryMenuGameObject.SetActive(false);
        
            UpgradesMenuGameObject.SetActive(true);
        }

        public void ExitToMainMenu()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            PrimaryMenuGameObject.SetActive(false);
        
            ExitWarningGameObject.SetActive(true);
        }

        public void ConfirmExitToMainMenu()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            SceneManagement.LoadSceneByType(SceneType.MainMenu);
        }
    }
}
