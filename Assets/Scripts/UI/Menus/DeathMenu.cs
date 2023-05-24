using System.Collections;
using Enums;
using Managers;
using PlayerScripts;
using UnityEngine;
using Utilities;

namespace UI.Menus
{
    public class DeathMenu : MonoBehaviour
    {
        private Player Player { get; set; }
        private SceneManagement SceneManagement { get; set; }
        private AudioManagement AudioManagement { get; set; }
        private GameObject PrimaryMenuGameObject { get; set; }
        private GameObject GameTipGameObject { get; set; }
        private GameObject RespawnButtonGameObject { get; set; }
        private GameObject MainMenuButtonGameObject { get; set; }
        private GameObject RespawnTimerGameObject { get; set; }
        private BarManagement RespawnTimerBar { get; set; }
        private PauseMenu PauseMenu { get; set; }
        public Coroutine ActivateCoroutine { get; private set; }
        private float RespawnTimer { get; set; }

        private void Awake()
        {
            Player = Utils.GetComponentOrThrow<Player>("Player");
            SceneManagement = Utils.GetComponentOrThrow<SceneManagement>("Interface/MainCamera/SceneManager");
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>("Interface/MainCamera/Audio/Sounds");
            PrimaryMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu");
            GameTipGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/GameTip");
            RespawnButtonGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/RespawnButton");
            MainMenuButtonGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/MainMenuButton");
            PauseMenu = Utils.GetComponentOrThrow<PauseMenu>("Interface/MainCamera/UICanvas/PauseMenu");
            RespawnTimerGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/RespawnTimerBar");
            RespawnTimerBar = Utils.GetComponentOrThrow<BarManagement>("Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/RespawnTimerBar/Slider");
        }

        private void Start()
        {
            RespawnTimer = 5f;
            RespawnTimerBar.SetBar(BarType.Recharging, RespawnTimer, RespawnTimer);
        
            PrimaryMenuGameObject.SetActive(true);
            PrimaryMenuGameObject.SetActive(false);
        }

        public void Activate()
        {
            if (ActivateCoroutine is null)
            {
                ActivateCoroutine = StartCoroutine(ActivateRoutine());
            }
        }
    
        private IEnumerator ActivateRoutine()
        {
            PauseMenu.Resume();
            PauseMenu.CanPause = false;

            yield return new WaitForSecondsRealtime(0.5f);
        
            Time.timeScale = 0f;
            MainAudioManagement.StopAll();
            Cursor.visible = true;
        
            PrimaryMenuGameObject.SetActive(true);
            RespawnTimerBar.SetBar(BarType.Recharging, RespawnTimer, RespawnTimer);
        
            StartCoroutine(RespawnCountdown());
            yield return new WaitForSecondsRealtime(RespawnTimer);

            GameTipGameObject.SetActive(false);
            RespawnTimerGameObject.SetActive(false);
            RespawnButtonGameObject.SetActive(true);
        
            MainMenuButtonGameObject.SetActive(true);
        }
    
        private IEnumerator RespawnCountdown()
        {
            var elapsedTime = 0f;

            while (elapsedTime < RespawnTimer)
            {
                elapsedTime += Time.unscaledDeltaTime;
                RespawnTimerBar.SetValue(RespawnTimer - elapsedTime);
        
                yield return null;
            }
    
            RespawnTimerBar.SetValue(0f);
        }

        public void Respawn()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            SceneManagement.LoadSavedScene();
        }
    
        public void ExitToMainMenu()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            SceneManagement.LoadSceneByType(SceneType.MainMenu);
        }
    }
}
