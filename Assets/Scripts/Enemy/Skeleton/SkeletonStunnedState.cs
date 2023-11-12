using UnityEngine;

namespace Enemy.Skeleton
{
    public class SkeletonStunnedState : EnemyState
    {
        private Enemy_Skeleton enemy;
        public SkeletonStunnedState(Enemy enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Skeleton _enemy) : base(enemyBase, _stateMachine, _animBoolName)
        {
            this.enemy = _enemy;
        }

        public override void Update()
        {
            base.Update();
            enemy.fx.InvokeRepeating("RedColorBlink",0,.1f);
            if (stateTimer<0)
                stateMachine.ChangeState(enemy.idleState);
                
            
        }

        public override void Enter()
        {
            base.Enter();
            stateTimer = enemy.stunDuration;
            rb.velocity=new Vector2(-enemy.facingDir * enemy.stunDirection.x,enemy.stunDirection.y);
        }

        public override void Exit()
        {
            base.Exit();
            enemy.fx.Invoke("CancelColorChange",0);
        }
    }
}