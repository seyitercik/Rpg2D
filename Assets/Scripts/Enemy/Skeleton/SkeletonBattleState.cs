using UnityEngine;

namespace Enemy.Skeleton
{
    public class SkeletonBattleState : EnemyState

    {
        private Transform player;
        private Enemy_Skeleton enemy;
        private int moveDir;
        public SkeletonBattleState(Enemy enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Skeleton _enemy) : base(enemyBase, _stateMachine, _animBoolName)
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
            if (enemy.IsPlayerDetected())
            {
                stateTimer = enemy.battleTime;
                if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
                {
                    if (CanAttack())
                        stateMachine.ChangeState(enemy.attackState);
                }
            }
            else
            {
                if (stateTimer<0|| Vector2.Distance(player.transform.position,enemy.transform.position)>7)
                    stateMachine.ChangeState(enemy.idleState);
            
            }
            if (player.position.x > enemy.transform.position.x)
                moveDir = 1;
            else if (player.position.x < enemy.transform.position.x)
                moveDir = -1;
            
            enemy.SetVelocity(enemy.moveSpeed*moveDir,rb.velocity.y);
            
        }


        public override void Exit()
        {
            base.Exit();
        }

        private bool CanAttack()
        {
            if (Time.time>=enemy.lastTimeAtatck+enemy.attackCoolDowm)
            {
                enemy.lastTimeAtatck = Time.time;
                return true;
            }
        

            return false;
        }
    }
}