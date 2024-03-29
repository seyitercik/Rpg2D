using UnityEngine;

namespace Enemy
{
    public class EnemyState
    {
        protected EnemyStateMachine stateMachine;
        protected Enemy enemyBase;
        private string animBoolName;
        protected float stateTimer;
        protected Rigidbody2D rb;
        protected bool triggerCalled;

        public EnemyState(Enemy enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
        {
            this.enemyBase = enemyBase;
            this.stateMachine = _stateMachine;
            this.animBoolName = _animBoolName;
        }

        public virtual void Update()
        {
            stateTimer -= Time.deltaTime;
        }
    
        public virtual void Enter()
        {
            triggerCalled = false;
            rb = enemyBase.rb;
            enemyBase.anim.SetBool(animBoolName,true);
        }

        public virtual void Exit()
        {
            enemyBase.anim.SetBool(animBoolName,false);
            enemyBase.AssingLastAnimName(animBoolName);
        
        }

        public virtual void AnimationFinishTrigger() => triggerCalled = true;
    }
}