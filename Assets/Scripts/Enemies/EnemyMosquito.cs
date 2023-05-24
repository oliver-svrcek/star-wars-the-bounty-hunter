namespace Enemies
{
    public class EnemyMosquito : EnemyFlyer
    {
        private new void Start()
        {
            if (UseOnlyEditorValues)
            {
                return;
            }
        
            CanHeal = false;
            MaximumHealth = 1;
            CurrentHealth = MaximumHealth;
            Damage = int.MaxValue;
            DeathSound = "MosquitoDeathSound";
            
            base.Start();
        }
    }
}
