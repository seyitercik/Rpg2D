using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Controllers.Skill_Controllers
{
    public class Sword_Skill_Controller : MonoBehaviour
    {
        private Animator anim;
        private Rigidbody2D rb;
        private CircleCollider2D cd;
        private Player.Player  player;
        
        private bool canRotate=true;
        private bool isReturning;
        
        private float freezeTimeDuration;
        private float returnSpeed = 12;
        
        [Header("Pierce info")]
        [SerializeField] private float pierceAmount;

        [Header("spin info")] 
        private float maxTravelDistance;
        private float spinDuration;
        private float spinTimer;
        private bool wasStopped;
        private bool isSpining;
        private float hitTimer;
        private float hitCooldown;
        private float spinDirection;
        
        
        
        [Header("Bounce info")] 
         private float bounceSpeed;
        private bool isBouncing;
        private int bounceAmount;
        private List<Transform> enemyTarget;
        private int targetIndex;

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();
            cd = GetComponent<CircleCollider2D>();
        }
        private void Update()
        {
            if(canRotate)
                transform.right = rb.velocity;
            if (isReturning)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position,
                    returnSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position,player.transform.position)< 1)
                    player.CatchTheSword();
            }
            BounceLogic();
            SpinLogic();
        }

        private void DestroyMe()
        {
            Destroy(gameObject);
        }

        public void SetupSword(Vector2 _direction, float _gravityScale, Player.Player _player,
            float _freezeTimeDuration, float _returnSpeed)
        {
            
            
            player = _player;
            returnSpeed = _returnSpeed;
            freezeTimeDuration = _freezeTimeDuration;
            rb.velocity = _direction;
            rb.gravityScale = _gravityScale;
            if(pierceAmount <= 0)
                anim.SetBool("Rotation",true);
            spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
            Invoke("DestroyMe",7);
            
        }

        public void SetupBounce(bool _isBouncing, int _amountOfBounces,float _bounceSpeed)
        {
            isBouncing = _isBouncing;
            bounceAmount = _amountOfBounces;
            bounceSpeed = _bounceSpeed;
            enemyTarget = new List<Transform>();

        }

        public void SetupPierce(int _pierceAmount)
        {
            pierceAmount = _pierceAmount;

        }

        public void SetupSpip(bool _isSpinning, float _maxTravelDistance, float _spinDuration,float _hitCooldown)
        {
            isSpining = _isSpinning;
            maxTravelDistance = _maxTravelDistance;
            spinDuration = _spinDuration;
            hitCooldown = _hitCooldown;

        }

        public void ReturnSword()
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //rb.isKinematic = false;
            transform.parent = null;
            isReturning = true;


        }
        private void SpinLogic()
        {
            if (isSpining)
            {
                if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
                {
                    StopWhenSpinning();
                }

                if (wasStopped)
                {
                    spinTimer -= Time.deltaTime;
                    /*transform.position = Vector2.MoveTowards(transform.position, new Vector2(
                        transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);*/
                    if (spinTimer < 0)
                    {
                        isReturning = true;
                        isSpining = false;
                    }

                    hitTimer -= Time.deltaTime;
                    ;
                    if (hitTimer < 0)
                    {
                        hitTimer = hitCooldown;
                        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                        foreach (var hit in colliders)
                        {
                            if (hit.GetComponent<Enemy.Enemy>() != null)
                            {
                                SwordSkillDamage(hit.GetComponent<Enemy.Enemy>());
                                
                            }
                            
                        }
                    }
                }
            }
        }

        private void StopWhenSpinning()
        {
            wasStopped = true;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            spinTimer = spinDuration;
        }

        private void BounceLogic()
        {
            if (isBouncing && enemyTarget.Count > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
                {
                    SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy.Enemy>());
                    
                    targetIndex++;
                    bounceAmount--;
                    if (bounceAmount <= 0)
                    {
                        isBouncing = false;
                        isReturning = true;
                    }

                    if (targetIndex >= enemyTarget.Count)
                        targetIndex = 0;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy.Enemy>() != null)
                            hit.GetComponent<Enemy.Enemy>().DamageImpact();
                        
                    }
                }
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if(isReturning)
                return;
            if (collision.GetComponent<Enemy.Enemy>() != null)
            {
                Enemy.Enemy enemy = collision.GetComponent<Enemy.Enemy>();
                SwordSkillDamage(enemy);
            }

            SetupTargetsBounce(collision);
            StuckInto(collision);
        }

        private void SwordSkillDamage(Enemy.Enemy enemy)
        {
            player.stats.DoDamage(enemy.GetComponent<CharacterStats>());
            enemy.StartCoroutine("FreezeTimerCorountine", freezeTimeDuration);
            
            ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
            if (equipedAmulet != null)
            {
                equipedAmulet.Effect(enemy.transform);
            }
        }

        private void SetupTargetsBounce(Collider2D collision)
        {
            if (collision.GetComponent<Enemy.Enemy>() != null)
            {
                if (isBouncing && enemyTarget.Count <= 0)
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy.Enemy>() != null)
                        {
                            enemyTarget.Add(hit.transform);
                        }
                    }
                }
            }
        }

        private void StuckInto(Collider2D collision)
        {
            if (pierceAmount > 0 && collision.GetComponent<Enemy.Enemy>() != null)
            {
                pierceAmount--;
                return;
            }

            if (isSpining)
            {
                StopWhenSpinning();
                return;
            }
                
            
            
            canRotate = false;
            cd.enabled = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            if(isBouncing&& enemyTarget.Count>0)
                return;
            anim.SetBool("Rotation", false);
            transform.parent = collision.transform;
        }
    }
}
