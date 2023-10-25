using UnityEngine;

namespace Enemy.Skeleton
{
    public class SkeletonGroundedState : EnemyState
    {
        protected Enemy_Skeleton enemy;
        protected Transform player;
        public SkeletonGroundedState(Enemy enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Skeleton _enemy) : base(enemyBase, _stateMachine, _animBoolName)
        {
            this.enemy = _enemy;
        }
        public override void Enter()
        {
            base.Enter();
            player = GameObject.Find("Player").transform;

        }

        public override void Update()
        {
            base.Update();
            
            if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position)<2)
                stateMachine.ChangeState(enemy.battleState);
            
        }

        

        public override void Exit()
        {
            base.Exit();
        }
    }
}