using Skills;
using Stats;
using UnityEngine;

namespace Player
{
        public class PlayerAnimationTriggers : MonoBehaviour
        {
                private Player player => GetComponentInParent<global::Player.Player>();
        
                private void AnimationTrigger()
                {
                        player.AnimationTrigger();
                }

                private void AttackTrigger()
                {
                        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, 
                                player.attackCheckRadius);
                        foreach (var hit in colliders)
                        {
                                if (hit.GetComponent<Enemy.Enemy>() != null)
                                {
                                        EnemyStats _target = hit.GetComponent<EnemyStats>();
                                        if (_target!=null)
                                                player.stats.DoDamage(_target);
                                        
                                        // hit.GetComponent<Enemy.Enemy>().Damage();
                                       ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);
                                       if(weaponData!=null)
                                               weaponData.Effect(_target.transform);

                                }
                        }
                       
                }

                private void ThrowSword()
                {
                        SkillManager.instance.sword.CreateSword();
                }

                /*private void WeaponEffect()
                {
                        ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);
                        if(weaponData!=null)
                                weaponData.Effect();
                }*/
        }
}
