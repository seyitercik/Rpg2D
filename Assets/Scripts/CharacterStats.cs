

using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
        private EntityFX fx;
        
        [Header("Major Stats")]
        public Stat strength; // 1 point increase damage by 1 and crit.power by % 1
        public Stat agility; // 1 point increase evasion by 1 and crit.chance by % 1
        public Stat intelligence; // 1 point increase magic damage by 1 and magic resistance by 3
        public Stat vitality; // 1 point increase healt by 3 or 5 points
        [Header("Defensive Stats")]
        
        public Stat maxHealth;
        public Stat armor;
        public Stat evasion;
        public Stat magicResistance;
        
        [Header("Offensive Stats")]
        public Stat damage;
        public Stat critChance;
        public Stat critPower;          // Defoult value %150

        [Header("Magic Stats")] 
        public Stat fireDamage;
        public Stat iceDamage;
        public Stat lightingDamage;


        public bool isIgnited; // does damage over time
        public bool isChilled; // reduce armor by 20% 
        public bool isShocked; // reduce accuracy by 20%



        [SerializeField] private float ailmentsDuration = 4;
        private float ignitedTimer;
        private float chilledTimer;
        private float shockedTimer;
        
        
        
        private float igniteDamageCooldowm;
        private float ignitteDamageTimer;
        private int igniteDamage;
        
        
        public int currentHealth;

        public System.Action onHealthChanged;

        protected virtual void Start()
        { 
                critPower.SetDefaultValue(150);
                currentHealth = GetMaxHealthValue();
                fx = GetComponent<EntityFX>();
        }

        protected virtual void Update()
        {
                ignitedTimer -= Time.deltaTime;
                chilledTimer -= Time.deltaTime;
                shockedTimer -= Time.deltaTime;
                
                ignitteDamageTimer -= Time.deltaTime;
                if (ignitedTimer < 0)
                        isIgnited = false;
                
                if (chilledTimer < 0)
                        isChilled = false;
                
                if (shockedTimer < 0)
                        isShocked = false;

                if (ignitteDamageTimer<0 && isIgnited)
                {
                        Debug.Log("Take burn Damage" + igniteDamage);
                        DecreaseHealthBy(igniteDamage);
                        if(currentHealth<  0)
                                Die();
                        ignitteDamageTimer = igniteDamageCooldowm;
                }
        }

        public virtual void DoDamage(CharacterStats _targetStats)
        {
                if (TargetCanAvoidAttack(_targetStats))
                        return;
                
                int totalDamage = damage.GetValue() + strength.GetValue();

                if (CanCrit())
                {
                        totalDamage = CalculateCriticalDamage(totalDamage);
                }
                
                totalDamage = CheckTargetArmor(_targetStats, totalDamage);

                //_targetStats.TakeDamage(totalDamage);
                DoMagicalDamage(_targetStats);
        }

        public virtual void DoMagicalDamage(CharacterStats _targetStats)
        {
                int _fireDamage = fireDamage.GetValue();
                int _iceDamage = iceDamage.GetValue();
                int _lightingDamage = lightingDamage.GetValue();

                int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage+intelligence.GetValue();
                totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

                _targetStats.TakeDamage(totalMagicalDamage);
                if(Mathf.Max(_fireDamage, _iceDamage, _lightingDamage )<= 0)
                        return;

                bool canApplyIgnıte = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
                bool canApplyChill =  _iceDamage  > _fireDamage && _iceDamage   > _lightingDamage;
                bool canApplyShock =  _lightingDamage  > _fireDamage && _lightingDamage > _iceDamage;
                
                while(!canApplyIgnıte && !canApplyChill && !canApplyShock)
                {
                        if (Random.value < .3f && _fireDamage > 0)
                        {
                                canApplyIgnıte = true;
                                _targetStats.ApplyElements(canApplyIgnıte,canApplyChill,canApplyShock);
                                return;

                        }
                        if (Random.value < .5f && _iceDamage > 0)
                        {
                                canApplyChill = true;
                                _targetStats.ApplyElements(canApplyIgnıte,canApplyChill,canApplyShock);
                                return;

                        }
                        if (Random.value < .5f && _lightingDamage > 0)
                        {
                                canApplyShock = true;
                                _targetStats.ApplyElements(canApplyIgnıte,canApplyChill,canApplyShock);
                                return;

                        }
                        
                }
                
                if(canApplyIgnıte)
                        _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
                
                _targetStats.ApplyElements(canApplyIgnıte,canApplyChill,canApplyShock);


        }

        

        public void ApplyElements(bool _ignite,bool _chill, bool _shock)
        {
                if(isIgnited|| isChilled || isShocked)
                        return;
                if (_ignite)
                {
                        isIgnited = _ignite;
                        ignitedTimer = ailmentsDuration;
                        fx.IgniteFxFor(ailmentsDuration);
                }
                if (_chill)
                {
                        isChilled = _chill;
                        chilledTimer = ailmentsDuration;

                        float slowPercentage = .2f;
                        
                        GetComponent<Entity>().SlowEntityBy(slowPercentage,ailmentsDuration);
                        fx.ChillFxFor(ailmentsDuration);
                }
                if (_shock)
                {
                        isShocked = _shock;
                        shockedTimer = ailmentsDuration;
                        fx.ShockFxFor(ailmentsDuration);
                }
                
        }

        public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
        

       

        public virtual void TakeDamage(int _damage)
        {
                DecreaseHealthBy(_damage);
                if (currentHealth < 0)
                {
                        Die();
                }

                onHealthChanged();
        }

        protected virtual void DecreaseHealthBy(int _damage)
        {
                currentHealth -= _damage;
                if (onHealthChanged != null)
                {
                        onHealthChanged();
                }
        }

        protected virtual void Die()
        {
                
        }
        private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
        {
                if (_targetStats.isChilled)
                        totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
                else
                        totalDamage -= _targetStats.armor.GetValue();    
                
                
                totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
                return totalDamage;
        }

        private bool  TargetCanAvoidAttack(CharacterStats _targetStats)
        {
                int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

                if (isShocked)
                        totalEvasion += 20;
                
                if (Random.Range(0, 100) < totalEvasion)
                {
                        return true;
                }

                return false;
        }

        private bool CanCrit()
        {
                int totalCriticalChance = critChance.GetValue() + agility.GetValue();
                if (Random.Range(0,100)<=totalCriticalChance)
                {
                        return true;
                }

                return false;
        }
        private int CalculateCriticalDamage(int _damage)
        {
                float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
                float critDamage = _damage * totalCritPower;
                return Mathf.RoundToInt(critDamage);
        }
        private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
        {
                totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
                totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
                return totalMagicalDamage;
        }

        public int GetMaxHealthValue()
        {;
                return maxHealth.GetValue() + vitality.GetValue() * 5;
        }

}