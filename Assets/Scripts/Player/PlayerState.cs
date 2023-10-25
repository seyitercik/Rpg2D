
using UnityEngine;

namespace Player
{
    public class PlayerState
    {
    
        protected PlayerStateMachine stateMachine;
        protected Player player;
        protected float xInput;
        protected float yInput;
        protected Rigidbody2D rb;
        protected float stateTimer;
        protected bool triggerCalled;
        private string animBoolName;

        public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
        {
            this.player = _player;
            this.stateMachine = _stateMachine;
            this.animBoolName = _animBoolName;
        
        }

        public virtual void Enter()
        {
            player.anim.SetBool(animBoolName,true);
            rb = player.rb;
            triggerCalled = false;

        }
        public virtual void Update()
        {
            stateTimer -= Time.deltaTime;
            xInput = Input.GetAxisRaw("Horizontal");
            yInput = Input.GetAxisRaw("Vertical");
            player.anim.SetFloat("yVelocity",rb.velocity.y);

        }
        public virtual void Exit()
        {
            player.anim.SetBool(animBoolName,false);
        
        }

        public virtual void AnimationFinishTrigger()
        {
            triggerCalled = true;
        }
        
    }
}
