using UnityEngine;

namespace Player
{
    public class PlayerAimSwordState : PlayerState
    {
        public PlayerAimSwordState(global::Player.Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.skill.sword.DotsActive(true);
        }

        public override void Update()
        {
            base.Update();
            if (Input.GetKeyUp(KeyCode.Q))
            {
                stateMachine.ChangeState(player.idleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

