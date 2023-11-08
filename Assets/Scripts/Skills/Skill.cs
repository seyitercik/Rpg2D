using UnityEngine;

namespace Skills
{
   public class Skill : MonoBehaviour
   {
      [SerializeField] protected float coolDown;
      
      protected float coolDownTimer;

      protected Player.Player player;

      protected virtual void Start()
      {
         player = PlayerManager.instance.player;
      }

      protected virtual void Update()
      {
         coolDownTimer -= Time.deltaTime;
      }

      public virtual bool CanUseSkill()
      {
         if (coolDownTimer < 0 )
         {
            UseSkill();
            coolDownTimer = coolDown;
            return true;
         }
      

         return false;
      }

      public virtual void UseSkill()
      {
         //do some skill spesific things
      }

      protected virtual Transform FindClosestEnemy(Transform _checkTransform)
      {
         Collider2D[] coliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
         float closestDistance = Mathf.Infinity;
         Transform closestEnemy = null;
         foreach (var hit in coliders)
         {
            if (hit.GetComponent<Enemy.Enemy>() != null)
            {
               float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);
               if (distanceToEnemy < closestDistance)
               {
                  closestDistance = distanceToEnemy;
                  closestEnemy = hit.transform;

               }
            }
         }

         return closestEnemy;
      }
   }
}
