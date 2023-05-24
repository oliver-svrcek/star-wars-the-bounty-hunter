using System.Collections;
using UI.Menus;
using UnityEngine;
using Utilities;

namespace PlayerScripts
{
    public class Player : MonoBehaviour
    {
        public PlayerDataManagement PlayerDataManagement { get; private set; }
        public PlayerGear PlayerGear { get; private set; }
        private Health PlayerHealth { get; set; }
        private PlayerMovement PlayerMovement { get; set; }
        private PlayerWeapons PlayerWeapons { get; set; }
        private PauseMenu PauseMenu { get; set; }
        private DeathMenu DeathMenu { get; set; }

        private void Awake()
        {
            PlayerDataManagement = new PlayerDataManagement(PlayerPrefs.GetString("ActivePlayer"));
            
            PlayerHealth = Utils.GetComponentOrThrow<Health>(this.gameObject);
            PlayerMovement = Utils.GetComponentOrThrow<PlayerMovement>(this.gameObject);
            PlayerWeapons = Utils.GetComponentOrThrow<PlayerWeapons>(this.gameObject);
            PauseMenu = Utils.GetComponentOrThrow<PauseMenu>("Interface/MainCamera/UICanvas/PauseMenu");
            DeathMenu = Utils.GetComponentOrThrow<DeathMenu>("Interface/MainCamera/UICanvas/DeathMenu");
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => PlayerDataManagement.PlayerData != null);
            
            PlayerGear = new PlayerGear(PlayerDataManagement, PlayerHealth, PlayerMovement, PlayerWeapons);
            this.transform.position = new Vector3(PlayerDataManagement.PlayerData.PositionAxisX, PlayerDataManagement.PlayerData.PositionAxisY, 0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (PauseMenu.IsPaused)
                {
                    PauseMenu.Resume();
                }
                else
                {
                    PauseMenu.Pause();
                }
            }
            if (Input.GetKeyDown(KeyCode.F1))
            {
                CheatCoinCount();
            }
        }
        
        private void CheatCoinCount()
        {
            if (Application.isEditor || Debug.isDebugBuild)
            {
                Debug.Log("CheatCoinCount");
                PlayerDataManagement.PlayerData.CoinCount = 99;
            }
        }
    }
}
