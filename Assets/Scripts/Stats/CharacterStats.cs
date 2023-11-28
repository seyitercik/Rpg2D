using Controllers;
using Stats;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Stats
{
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
                [SerializeField] private GameObject shockStrikePrefab;
                private int igniteDamage;
                private int shockDamage;
        
        
                public int currentHealth;

                public System.Action onHealthChanged;
                public bool isDead { get; private set; }

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
                        if(isIgnited) 
                                ApplyIgniteDamage();
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

                        _targetStats.TakeDamage(totalDamage);
                        //DoMagicalDamage(_targetStats);
                }
                public virtual void TakeDamage(int _damage)
                {
                        DecreaseHealthBy(_damage);
                        GetComponent<Entity>().DamageImpact();
                        fx.StartCoroutine("FlashFx");

                        if (currentHealth < 0 && !isDead)
                        {
                                Die();
                        }

                        onHealthChanged();
                }

                public virtual void IncreaseHealthBy(int _amount)
                {
                        currentHealth += _amount;
                        if (currentHealth> GetMaxHealthValue())
                        {
                                currentHealth = GetMaxHealthValue();
                        }

                        if (onHealthChanged != null)
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
                        isDead = true;

                }

                #region Magical damage and ailments
                
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

                        AttemtyToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightingDamage);
                }

                private  void AttemtyToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage,
                        int _lightingDamage)
                {
                        bool canApplyIgnıte = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
                        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
                        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

                        while (!canApplyIgnıte && !canApplyChill && !canApplyShock)
                        {
                                if (Random.value < .3f && _fireDamage > 0)
                                {
                                        canApplyIgnıte = true;
                                        _targetStats.ApplyAilments(canApplyIgnıte, canApplyChill, canApplyShock);
                                        return;
                                }

                                if (Random.value < .5f && _iceDamage > 0)
                                {
                                        canApplyChill = true;
                                        _targetStats.ApplyAilments(canApplyIgnıte, canApplyChill, canApplyShock);
                                        return;
                                }

                                if (Random.value < .5f && _lightingDamage > 0)
                                {
                                        canApplyShock = true;
                                        _targetStats.ApplyAilments(canApplyIgnıte, canApplyChill, canApplyShock);
                                        return;
                                }
                        }

                        if (canApplyIgnıte)
                                _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
                        if (canApplyShock)
                                _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .1f));

                        _targetStats.ApplyAilments(canApplyIgnıte, canApplyChill, canApplyShock);
                }


                public void ApplyAilments(bool _ignite,bool _chill, bool _shock)
                {
                        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
                        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
                        bool canApplyShock = !isIgnited && !isChilled;
                
                        if (_ignite && canApplyIgnite)
                        {
                                isIgnited = _ignite;
                                ignitedTimer = ailmentsDuration;
                                fx.IgniteFxFor(ailmentsDuration);
                        }
                        if (_chill && canApplyChill)
                        {
                                isChilled = _chill;
                                chilledTimer = ailmentsDuration;

                                float slowPercentage = .2f;
                        
                                GetComponent<Entity>().SlowEntityBy(slowPercentage,ailmentsDuration);
                                fx.ChillFxFor(ailmentsDuration);
                        }
                        if (_shock && canApplyShock)
                        {
                                if (!isShocked)
                                {
                                        ApplyShock(_shock);
                                }
                                else
                                {
                                        if(GetComponent<Player.Player>() !=null)
                                                return;
                                        HitNearestTargetWithShockStrike();
                                }
                       
                        }
                
                }

                public void ApplyShock(bool _shock)
                {
                        if(isShocked)
                                return;
                        isShocked = _shock;
                        shockedTimer = ailmentsDuration;
                        fx.ShockFxFor(ailmentsDuration);
                }

                private void ApplyIgniteDamage()
                {
                        if (ignitteDamageTimer < 0 && isIgnited)
                        {
                                DecreaseHealthBy(igniteDamage);
                                if (currentHealth < 0 )
                                {
                                        Die();
                                     isIgnited = false;
                                }

                                ignitteDamageTimer = igniteDamageCooldowm;
                        }
                }
        


                private void HitNearestTargetWithShockStrike()
                {
                        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

                        float closestDistance = Mathf.Infinity;
                        Transform closestEnemy = null;

                        foreach (var hit in colliders)
                        {
                                if (hit.GetComponent<Enemy.Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
                                {
                                        float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                                        if (distanceToEnemy < closestDistance)
                                        {
                                                closestDistance = distanceToEnemy;
                                                closestEnemy = hit.transform;
                                        }
                                }

                                if (closestEnemy == null)
                                {
                                        closestEnemy = transform;
                                }
                        }

                        if (closestEnemy != null)
                        {
                                GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position,
                                        Quaternion.identity);
                                newShockStrike.GetComponent<ShockStrike_Controller>()
                                        .Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
                        }
                }

                public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
                public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;
                #endregion
              
                #region Stat calculations
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
                #endregion
                

        }
}