using Enums;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerGear
    {
        private PlayerDataManagement PlayerDataManagement { get; set; }
        private Health PlayerHealth { get; set; }
        private PlayerMovement PlayerMovement { get; set; }
        private PlayerWeapons PlayerWeapons { get; set; }
        
        public PlayerGear(PlayerDataManagement playerDataManagement , Health playerHealth, PlayerMovement playerMovement, PlayerWeapons playerWeapons)
        {
            PlayerDataManagement = playerDataManagement;
            PlayerHealth = playerHealth;
            PlayerMovement = playerMovement;
            PlayerWeapons = playerWeapons;
            
            ReloadAbilities();
        }

        public void ReloadAbilities()
        {
            ReloadHealth();
            ReloadBlaster();
            ReloadJetpack();
            ReloadFlamethrower();
        }
        
        public void ReloadHealth()
        {
            switch (PlayerDataManagement.PlayerData.HealthLevel)
            {
                case 1:
                    PlayerHealth.MaximumHealth = 10000;
                    PlayerHealth.HealStartTime = 4.5f;
                    PlayerHealth.HealPoints = 10;
                    break;
                case 2:
                    PlayerHealth.MaximumHealth = 13300;
                    PlayerHealth.HealStartTime = 4f;
                    PlayerHealth.HealPoints = 15;
                    break;
                case 3:
                    PlayerHealth.MaximumHealth = 16600;
                    PlayerHealth.HealStartTime = 3.5f;
                    PlayerHealth.HealPoints = 20;
                    break;
                case 4:
                    PlayerHealth.MaximumHealth = 20000;
                    PlayerHealth.HealStartTime = 3f;
                    PlayerHealth.HealPoints = 25;
                    break;
            }

            PlayerHealth.Reload();
        }
        
        public void ReloadBlaster()
        {
            switch (PlayerDataManagement.PlayerData.BlasterLevel)
            {
                case 1:
                    PlayerWeapons.Blaster.BulletDamage = 2000;
                    PlayerWeapons.Blaster.MaximumBlasterHeat = 10000;
                    PlayerWeapons.Blaster.BlasterHeatPerShot = 2000;
                    PlayerWeapons.Blaster.BlasterCoolingStartTime = 0.2f;
                    PlayerWeapons.Blaster.BlasterCoolingPower = 100;
                    PlayerWeapons.Blaster.BlasterOverheatCoolingStartTime = 0.6f;
                    PlayerWeapons.Blaster.BlasterOverheatCoolingPower = 60;
                    break;
                case 2:
                    PlayerWeapons.Blaster.BulletDamage = 3000;
                    PlayerWeapons.Blaster.MaximumBlasterHeat = 10300;
                    PlayerWeapons.Blaster.BlasterHeatPerShot = 1900;
                    PlayerWeapons.Blaster.BlasterCoolingStartTime = 0.19f;
                    PlayerWeapons.Blaster.BlasterCoolingPower = 110;
                    PlayerWeapons.Blaster.BlasterOverheatCoolingStartTime = 0.53f;
                    PlayerWeapons.Blaster.BlasterOverheatCoolingPower = 63;
                    break;
                case 3:
                    PlayerWeapons.Blaster.BulletDamage = 4000;
                    PlayerWeapons.Blaster.MaximumBlasterHeat = 10600;
                    PlayerWeapons.Blaster.BlasterHeatPerShot = 1800;
                    PlayerWeapons.Blaster.BlasterCoolingStartTime = 0.18f;
                    PlayerWeapons.Blaster.BlasterCoolingPower = 120;
                    PlayerWeapons.Blaster.BlasterOverheatCoolingStartTime = 0.46f;
                    PlayerWeapons.Blaster.BlasterOverheatCoolingPower = 66;
                    break;
                case 4:
                    PlayerWeapons.Blaster.BulletDamage = 5000;
                    PlayerWeapons.Blaster.MaximumBlasterHeat = 11000;
                    PlayerWeapons.Blaster.BlasterHeatPerShot = 1700;
                    PlayerWeapons.Blaster.BlasterCoolingStartTime = 0.17f;
                    PlayerWeapons.Blaster.BlasterCoolingPower = 130;
                    PlayerWeapons.Blaster.BlasterOverheatCoolingStartTime = 0.4f;
                    PlayerWeapons.Blaster.BlasterOverheatCoolingPower = 70;
                    break;
            }
            
            PlayerWeapons.Blaster.Reload();
        }
        
        public void ReloadJetpack()
        {
            switch (PlayerDataManagement.PlayerData.JetpackLevel)
            {
                case 1:
                    PlayerMovement.Jetpack.JetpackFuelConsumptionInitialPoints = 800;
                    PlayerMovement.Jetpack.JetpackFuelConsumptionPoints = 70;
                    PlayerMovement.Jetpack.JetpackFuelRechargePoints = 30;
                    PlayerMovement.Jetpack.JetpackFuelRechargeStartTime = 1.2f;
                    break;
                case 2:
                    PlayerMovement.Jetpack.JetpackFuelConsumptionInitialPoints = 800;
                    PlayerMovement.Jetpack.JetpackFuelConsumptionPoints = 65;
                    PlayerMovement.Jetpack.JetpackFuelRechargePoints = 38;
                    PlayerMovement.Jetpack.JetpackFuelRechargeStartTime = 1f;
                    break;
                case 3:
                    PlayerMovement.Jetpack.JetpackFuelConsumptionInitialPoints = 800;
                    PlayerMovement.Jetpack.JetpackFuelConsumptionPoints = 60;
                    PlayerMovement.Jetpack.JetpackFuelRechargePoints = 46;
                    PlayerMovement.Jetpack.JetpackFuelRechargeStartTime = 0.8f;
                    break;
                case 4:
                    PlayerMovement.Jetpack.JetpackFuelConsumptionInitialPoints = 800;
                    PlayerMovement.Jetpack.JetpackFuelConsumptionPoints = 55;
                    PlayerMovement.Jetpack.JetpackFuelRechargePoints = 52;
                    PlayerMovement.Jetpack.JetpackFuelRechargeStartTime = 0.5f;
                    break;
            }
            
            PlayerMovement.Jetpack.Reload();
        }
        
        public void ReloadFlamethrower()
        {
            switch (PlayerDataManagement.PlayerData.FlamethrowerLevel)
            {
                case 1:
                    PlayerWeapons.Flamethrower.ParticleStartLifetime = 0.25f;
                    PlayerWeapons.Flamethrower.FlameRange = 2.75f;
                    PlayerWeapons.Flamethrower.FlamethrowerDamage = 10000;
                    PlayerWeapons.Flamethrower.FlamethrowerOverheatCoolingPoints = 70;
                    break;
                case 2:
                    PlayerWeapons.Flamethrower.ParticleStartLifetime = 0.5f;
                    PlayerWeapons.Flamethrower.FlameRange = 5.5f;
                    PlayerWeapons.Flamethrower.FlamethrowerDamage = 15000;
                    PlayerWeapons.Flamethrower.FlamethrowerOverheatCoolingPoints = 85;
                    break;
                case 3:
                    PlayerWeapons.Flamethrower.ParticleStartLifetime = 0.75f;
                    PlayerWeapons.Flamethrower.FlameRange = 8.25f;
                    PlayerWeapons.Flamethrower.FlamethrowerDamage = 20000;
                    PlayerWeapons.Flamethrower.FlamethrowerOverheatCoolingPoints = 100;
                    break;
                case 4:
                    PlayerWeapons.Flamethrower.ParticleStartLifetime = 1f;
                    PlayerWeapons.Flamethrower.FlameRange = 11f;
                    PlayerWeapons.Flamethrower.FlamethrowerDamage = 25000;
                    PlayerWeapons.Flamethrower.FlamethrowerOverheatCoolingPoints = 115;
                    break;
            }
            
            PlayerWeapons.Flamethrower.Reload();
        }
    }
}