using UnityEngine;

namespace Player
{
    public class PlayerBlackholeState : PlayerState
    {
        private float flyTime = .4f;
        private bool skillUsed;
        
        public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            skillUsed = false;
            stateTimer = flyTime;
            rb.gravityScale = 0;
        }

        public override void Update()
        {
            base.Update();
            if (stateTimer > 0)
            {
                rb.velocity = new Vector2(0, 15);
                
            }

            if (stateTimer < 0)
            {
                rb.velocity = new Vector2(0, -.1f);
                if (!skillUsed)
                {
                    if(player.skill.blackhole.CanUseSkill());
                    skillUsed = true;
                }
                
            }

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}