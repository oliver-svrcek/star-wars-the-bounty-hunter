using System.Collections;
using System.Collections.Generic;
using Enums;
using Managers;
using PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI.Menus
{
    public class UpgradesMenu : MonoBehaviour
    {
        private Player Player { get; set; }
        private AudioManagement AudioManagement { get; set; }
        private TextMeshProUGUI CoinCount { get; set; }
        private Health Health { get; set; }
        private Blaster Blaster { get; set; }
        private Jetpack Jetpack { get; set; }
        private Flamethrower Flamethrower { get; set; }
        private GameObject PrimaryMenuGameObject { get; set; }
        private GameObject PauseMenuGameObject { get; set; }
        private Dictionary<GearType, Slider> Sliders { get; set; }
        private Dictionary<GearType, Image> SlidersImages { get; set; }

        private void Awake()
        {
            Player = Utils.GetComponentOrThrow<Player>("Player");
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>("Interface/MainCamera/Audio/Sounds");
            CoinCount = Utils.GetComponentOrThrow<TextMeshProUGUI>("Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/CoinsCount/Text (TMP)");
            Health = Utils.GetComponentOrThrow<Health>("Player");
            Blaster = Utils.GetComponentOrThrow<Blaster>("Player/Blaster");
            Jetpack = Utils.GetComponentOrThrow<Jetpack>("Player/Jetpack");
            Flamethrower = Utils.GetComponentOrThrow<Flamethrower>("Player/Flamethrower");
            PrimaryMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu");
            PauseMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/PauseMenu/PrimaryMenu");
            Sliders = new Dictionary<GearType, Slider>
            {
                {GearType.Health, Utils.GetComponentOrThrow<Slider>("Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/UpgradeOptions/Health/Button/Slider")},
                {GearType.Blaster, Utils.GetComponentOrThrow<Slider>("Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/UpgradeOptions/Blaster/Button/Slider")},
                {GearType.Jetpack, Utils.GetComponentOrThrow<Slider>("Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/UpgradeOptions/Jetpack/Button/Slider")},
                {GearType.Flamethrower, Utils.GetComponentOrThrow<Slider>("Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/UpgradeOptions/Flamethrower/Button/Slider")}
            };
            SlidersImages = new Dictionary<GearType, Image>
            {
                {GearType.Health, Utils.GetComponentOrThrow<Image>("Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/UpgradeOptions/Health/Button/Slider/Fill Area/Fill")},
                {GearType.Blaster, Utils.GetComponentOrThrow<Image>("Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/UpgradeOptions/Blaster/Button/Slider/Fill Area/Fill")},
                {GearType.Jetpack, Utils.GetComponentOrThrow<Image>("Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/UpgradeOptions/Jetpack/Button/Slider/Fill Area/Fill")},
                {GearType.Flamethrower, Utils.GetComponentOrThrow<Image>("Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/UpgradeOptions/Flamethrower/Button/Slider/Fill Area/Fill")},
            };
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => Player.PlayerDataManagement.PlayerData != null);
            yield return new WaitUntil(() => Player.PlayerGear != null);
        
            RefreshGearLevels();
        }

        public void RefreshGearLevels()
        {
            CoinCount.text = Player.PlayerDataManagement.PlayerData.CoinCount.ToString();
        
            foreach (var gearType in Sliders.Keys)
            {
                switch (gearType)
                {
                    case GearType.Health:
                        Sliders[gearType].value = Player.PlayerDataManagement.PlayerData.HealthLevel;
                        SlidersImages[gearType].color = UpgradeLevels.UpgradeLevelsCollection[Player.PlayerDataManagement.PlayerData.HealthLevel];
                        break;
                    case GearType.Blaster:
                        Sliders[gearType].value = Player.PlayerDataManagement.PlayerData.BlasterLevel;
                        SlidersImages[gearType].color = UpgradeLevels.UpgradeLevelsCollection[Player.PlayerDataManagement.PlayerData.BlasterLevel];
                        break;
                    case GearType.Jetpack:
                        Sliders[gearType].value = Player.PlayerDataManagement.PlayerData.JetpackLevel;
                        SlidersImages[gearType].color = UpgradeLevels.UpgradeLevelsCollection[Player.PlayerDataManagement.PlayerData.JetpackLevel];
                        break;
                    case GearType.Flamethrower:
                        Sliders[gearType].value = Player.PlayerDataManagement.PlayerData.FlamethrowerLevel;
                        SlidersImages[gearType].color = UpgradeLevels.UpgradeLevelsCollection[Player.PlayerDataManagement.PlayerData.FlamethrowerLevel];
                        break;
                    default:
                        Debug.LogError($"Unknown gear type: {gearType}!");
                        break;
                }
            }
        }
    
        public void UpgradeGear(string gearName)
        {
            var gearType = Convert.GetGearType(gearName);
            if (gearType is null)
            {
                return;
            }
        
            if (Player.PlayerDataManagement.PlayerData.CoinCount == 0)
            {
                AudioManagement.PlayOneShot("ErrorSound");
                return;
            }
        
            switch (gearType)
            {
                case GearType.Health:
                    if (Health.HealCoroutine is not null || Player.PlayerDataManagement.PlayerData.HealthLevel == 4)
                    {
                
                        AudioManagement.PlayOneShot("ErrorSound");
                        return;
                    }
                
                    Player.PlayerDataManagement.PlayerData.HealthLevel += 1;
                    Player.PlayerGear.ReloadHealth();
                
                    break;
                case GearType.Blaster:
                    if (Blaster.ShootCoroutine is not null || Blaster.CoolingCoroutine is not null || 
                        Blaster.OverheatCoroutine is not null || Player.PlayerDataManagement.PlayerData.BlasterLevel == 4)
                    {
                        AudioManagement.PlayOneShot("ErrorSound");
                        return;
                    }
                
                    Player.PlayerDataManagement.PlayerData.BlasterLevel += 1;
                    Player.PlayerGear.ReloadBlaster();
                
                    break;
                case GearType.Jetpack:
                    if (Jetpack.BurnFuelCoroutine is not null || Jetpack.RechargeFuelCoroutine is not null ||
                        Player.PlayerDataManagement.PlayerData.JetpackLevel == 4)
                    {
                        AudioManagement.PlayOneShot("ErrorSound");
                        return;
                    }
                
                    Player.PlayerDataManagement.PlayerData.JetpackLevel += 1;
                    Player.PlayerGear.ReloadJetpack();
                
                    break;
                case GearType.Flamethrower:
                    if (Flamethrower.FlameCoroutine is not null || Flamethrower.CoolingCoroutine is not null ||
                        Player.PlayerDataManagement.PlayerData.FlamethrowerLevel == 4)
                    {
                        AudioManagement.PlayOneShot("ErrorSound");
                        return;
                    }
                
                    Player.PlayerDataManagement.PlayerData.FlamethrowerLevel += 1;
                    Player.PlayerGear.ReloadFlamethrower();
                
                    break;
                default:
                    Debug.LogError($"Unknown gear type: {gearType}!");
                    return;
            }
        
            AudioManagement.PlayOneShot("UpgradeButtonSound");
        
            Player.PlayerDataManagement.PlayerData.CoinCount -= 1;
            CoinCount.text = Player.PlayerDataManagement.PlayerData.CoinCount.ToString();
        
            RefreshGearLevels();
        }
    
        public void SwitchBackToPrimaryMenu()
        {
            AudioManagement.PlayOneShot("ButtonSound");
        
            PrimaryMenuGameObject.SetActive(false);
        
            PauseMenuGameObject.SetActive(true);
        }
    }
}
