using UnityEngine;

namespace DefaultNamespace
{
    public class EnemySkeletonAnimationsTrigger : MonoBehaviour
    {
        private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

        private void AnimationTrigger()
        {
            enemy.AnimationFinishTrigger();
        }
    }
}