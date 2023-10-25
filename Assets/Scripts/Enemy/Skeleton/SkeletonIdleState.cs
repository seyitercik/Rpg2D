namespace Enemy.Skeleton
{
    public class SkeletonIdleState : SkeletonGroundedState
    {
        public SkeletonIdleState(Enemy enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(enemyBase, _stateMachine, _animBoolName, _enemy)
        {
        }
        
       
        public override void Update()
        {
            base.Update();
            if (stateTimer<0)
                stateMachine.ChangeState(enemy.moveState);
            
            
            
        }

        public override void Enter()
        {
            base.Enter();
            stateTimer = enemy.idleTime;
        }

        public override void Exit()
        {
            base.Exit();
        }


        
    }
}