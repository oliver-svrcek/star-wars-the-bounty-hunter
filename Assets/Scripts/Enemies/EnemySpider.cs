namespace Enemies
{
    public class EnemySpider : EnemyWalker
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
            Speed = 35f;
            Damage = 4900;
            DeathSound = "SpiderDeathSound";
            
            base.Start();
        }
    }
}
