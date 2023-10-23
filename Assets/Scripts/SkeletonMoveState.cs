using UnityEngine;

namespace DefaultNamespace
{
    public class SkeletonMoveState : SkeletonGroundedState
    {
        public SkeletonMoveState(Enemy enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(enemyBase, _stateMachine, _animBoolName, _enemy)
        {
        }
        

        public override void Update()
        {
            base.Update();
            enemy.SetVelocity(enemy.moveSpeed*enemy.facingDir,rb.velocity.y);
            if (enemy.IsWallDetected()||!enemy.IsGroundDetected())
            {
                enemy.Flip();
            }
            
            
                
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

       
    }
}