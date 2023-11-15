using Controllers.Skill_Controllers;
using UnityEngine;

namespace Player
{
    public class PlayerGroundedState : PlayerState
    {
        public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
        
            base.Update();
            if (Input.GetKeyDown(KeyCode.R))
            {
                stateMachine.ChangeState(player.blackhole);
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                stateMachine.ChangeState(player.counterAttack);
            }

            if (Input.GetKeyDown(KeyCode.Q) && HasNoSword())
            {
                stateMachine.ChangeState(player.aimSword);
                
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                stateMachine.ChangeState(player.primaryAttcak);
            }
            if (!player.IsGroundDetected())
                stateMachine.ChangeState(player.airState);
                    
            if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected()) 
                stateMachine.ChangeState(player.jumpState);
        }

        private bool HasNoSword()
        {
            if (!player.sword)
            {
                return true;
            }
            player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();

            return false;
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}