using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

namespace DefaultNamespace
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            if(xInput==player.facingDir && player.IsWallDetected())
                return;
            
            if (xInput != 0)
                stateMachine.ChangeState(player.moveState);
            
        }

        public override void Exit()
        {
            base.Exit();
            
        }
    }
}