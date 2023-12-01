
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemies effect  effect",menuName = "Data/Item Effect/Freeze enemies")]
public class FreezeEnemy_Effect : ItemEffect
{
   [SerializeField] private float duration;
   public override void ExecuteEffect(Transform _transform)
   {
      Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

      foreach (var hit in colliders)
      {
       //  hit.GetComponent<Enemy.Enemy>().FreezeTime();

      }
   }
}
