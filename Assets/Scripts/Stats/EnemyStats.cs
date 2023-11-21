using UnityEngine;

namespace Stats
{
    public class EnemyStats : CharacterStats
    {
        [Header("level Details")] 
        [SerializeField] private int level;

        [Range(0f, 1f)]
        [SerializeField] private float percantageModifier = .4f;
        
        private Enemy.Enemy enemy;
        private ItemDrop myDropSystem;
        protected override void Start()
        {
            ApplyLevelModifiers();
            base.Start();
            enemy = GetComponent<Enemy.Enemy>();
            myDropSystem = GetComponent<ItemDrop>();
        }

        private void ApplyLevelModifiers()
        {
            Modify(strength);
            Modify(agility);
            Modify(intelligence);
            Modify(vitality);
            
            Modify(damage);
            Modify(critPower);
            Modify(critChance);
            
            Modify(armor);
            Modify(evasion);
            Modify(magicResistance);
            Modify(maxHealth);
            
            Modify(fireDamage);
            Modify(iceDamage);
            Modify(lightingDamage);
        }

        private void Modify(Stat _stat)
        {
            for (int i = 1; i < level; i++)
            {
                float modifier = _stat.GetValue() * percantageModifier;
                _stat.AddModifier(Mathf.RoundToInt(modifier));

            }
        }

        public override void TakeDamage(int _damage)
        {
            base.TakeDamage(_damage);
            
        }

        protected override void Die()
        {
           
            base.Die();
            enemy.Die();
            myDropSystem.GenerateDrop();
        }
    }
}