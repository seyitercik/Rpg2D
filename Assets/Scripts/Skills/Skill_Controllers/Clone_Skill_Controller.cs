using UnityEngine;
using Random = UnityEngine.Random;

namespace Skills.Skill_Controllers
{
   
   public class Clone_Skill_Controller : MonoBehaviour
   {
      private SpriteRenderer sr;
      [SerializeField] private float colorLosingSpeed;
      private Animator anim;
      private float cloneTimer;
      [SerializeField] private Transform attackCheck;
      [SerializeField] private float attackCheckRadius = .8f;
      private bool canDublicateClone;
      private Transform closestEnemy;
      private int facingDir = 1;
      private float chanceToDublicate;
      

      private void Awake()
      {
         sr = GetComponent<SpriteRenderer>();
         anim = GetComponent<Animator>();
      }

      private void Update()
      {
         cloneTimer -= Time.deltaTime;
         if (cloneTimer < 0)
         {
            sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime * colorLosingSpeed);
         }

         if (sr.color.a <= 0)
         {
            Destroy(gameObject);
         }
      }

      public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, 
         Vector3 _offset,Transform _closestEnemy,bool _canDublicateClone,float _chanceToDublicate)
      {
         if (_canAttack)
         {
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
         }
         transform.position = _newTransform.position + _offset;
         cloneTimer = _cloneDuration;
         closestEnemy = _closestEnemy;
         canDublicateClone = _canDublicateClone;
         chanceToDublicate = _chanceToDublicate;
         FaceClosestTarget();
         
      }


      private void AnimationTrigger()
      {
         cloneTimer = -.1f;

      }

      private void AttackTrigger()
      {
         Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
         foreach (var hit in colliders)
         {
            if (hit.GetComponent<Enemy.Enemy>() != null)
            {
               hit.GetComponent<Enemy.Enemy>().Damage();
               
               
               if (canDublicateClone)
               {
                  if (Random.Range(0, 100) < chanceToDublicate)
                  {
                     SkillManager.instance.clone.CreateClone(hit.transform,new Vector3(.5f*facingDir,0));
                  }
               }
               
            }
         }

      }

      private void FaceClosestTarget()
      {
         if (closestEnemy != null)
         {
            if ( transform.position.x > closestEnemy.position.x)
            {
               facingDir = -1;
               transform.Rotate(0, 180, 0);
               
            }
            
         }

      }
   }
}
