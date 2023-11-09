using DefaultNamespace;
using UnityEngine;

namespace Enemy.Skeleton
{
    public class EnemySkeletonAnimationsTrigger : MonoBehaviour
    {
        private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

        private void AnimationTrigger()
        {
            enemy.AnimationFinishTrigger();
        }
        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Player.Player>() != null)
                {
                    PlayerStats target = hit.GetComponent<PlayerStats>(); 
                    enemy.stats.DoDamage(target);
                }
                  //  hit.GetComponent<Player.Player>().Damage();
            }
                       
        }

        private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
        private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
    }
}