namespace DefaultNamespace
{
    public class PlayerStats : CharacterStats
    {
        private Player.Player player;
            
        protected override void Start()
        {
            base.Start();
            player = GetComponent<Player.Player>();
        }

        public override void TakeDamage(int _damage)
        {
            base.TakeDamage(_damage);
            player.DamageEffect();
        }

        protected override void Die()
        {
            base.Die();
        }
    }
}