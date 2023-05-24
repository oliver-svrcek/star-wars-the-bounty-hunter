    
namespace Enemies
{
    public class EnemyRebelTrooper : EnemyShooter
    {
        private new void Start()
        {
            if (UseOnlyEditorValues)
            {
                return;
            }
        
            CanHeal = false;
            MaximumHealth = 20000;
            CurrentHealth = MaximumHealth;
            ShootingRate = 0.8f;
            BulletsPerShot = 3;
            BulletDamage = 2000;
            BulletSpeed = 22f;
            DeathSound = "RebelTrooperDeathSound";
            
            base.Start();
        }
    }
}
