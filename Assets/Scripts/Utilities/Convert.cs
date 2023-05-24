using Enums;
using UnityEngine;

namespace Utilities
{
    public static class Convert
    {
        public static GearType? GetGearType(string gearName)
        {
            gearName = gearName.ToLower();
        
            switch (gearName)
            {
                case "health":
                    return GearType.Health;
                case "blaster":
                    return GearType.Blaster;
                case "jetpack":
                    return GearType.Jetpack;
                case "flamethrower":
                    return GearType.Flamethrower;
                default:
                    Debug.LogError($"Unknown gear type: {gearName}!");
                    return null;
            }
        } 
    }
}