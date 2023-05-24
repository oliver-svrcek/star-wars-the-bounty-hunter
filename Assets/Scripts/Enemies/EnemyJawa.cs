namespace Enemies
{
    public class EnemyJawa: EnemyChaser
    {
        private new void Start()
        {
            if (UseOnlyEditorValues)
            {
                return;
            }
        
            CanHeal = false;
            MaximumHealth = 30000;
            CurrentHealth = MaximumHealth;
            Speed = 25f;
            Damage = 7400;
            DeathSound = "JawaDeathSound";
            
            base.Start();
        }
    }
}