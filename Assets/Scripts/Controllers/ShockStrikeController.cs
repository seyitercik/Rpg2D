using Stats;
using UnityEngine;

namespace Controllers
{
        public class ShockStrikeController : MonoBehaviour
        {
                [SerializeField] private CharacterStats targetStats;
                [SerializeField] private float speed;
                private int damage;
        

                private Animator anim;
                private bool triggered;

                private void Start()
                {
                        anim = GetComponentInChildren<Animator>();
                }

                public void Setup (int _damage, CharacterStats _targetStats)
                {
                        damage = _damage;
                        targetStats = _targetStats;
                } 

                private void Update()
                {
                        if(!targetStats)
                                return;

                        if (triggered) 
                                return;
                        
                
                        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position,
                                speed * Time.deltaTime);
                        transform.right = transform.position - targetStats.transform.position;
                        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
                        {
                                anim.transform.localRotation=Quaternion.identity;
                                transform.localRotation= Quaternion.identity;
                                transform.localScale = new Vector3(3, 3);
                                anim.transform.localPosition = new Vector3(0, .5f);
                        
                        
                                Invoke("DamageAndSelfDestroy",.2f);
                                triggered = true;
                                anim.SetTrigger("Hit");
                        }
                
                }

                private void DamageAndSelfDestroy()
                {
                        targetStats.ApplyShock(true);
                
                        targetStats.TakeDamage(damage);
                        Destroy(gameObject,.4f);
                }
        }
}