using System.Collections;
using UnityEngine;

namespace Player
{
    public class Player : Entity
    {
        [Header("Attack info")] 
        public Vector2[] attackMovement;
    
        public bool isBusy { get; private set; }
        [Header("Move info")] 
        public float moveSpeed = 12f;
        public float jumpForce;
        [Header("Dash info")] 
        [SerializeField] private float dashCoolDown;

        private float dashUsegTimer;
        public float dashSpeed;
        public float dashDuration;
        public float dashDir{ get; private set; }
    
        #region State

        public PlayerStateMachine stateMachine { get; private set; }
        public PlayerIdleState idleState { get; private set; }
        public PlayerMoveState moveState { get; private set; }
        public PlayerJumpState jumpState { get; private set; }
        public PlayerAirState airState { get; private set; }
        public PlayerDashState dashState { get; private set; }
        public PlayerWallSlideState wallSlide { get; private set; }
        public PlayerWallJumpState wallJump { get; private set; }
        public PlayerPrimaryAttackState primaryAttcak { get; private set; }
    

        #endregion
    
        protected override void Awake()
        {
            base.Awake();
        
            stateMachine = new PlayerStateMachine();
            idleState = new PlayerIdleState(this,stateMachine,"Idle");
            moveState = new PlayerMoveState(this,stateMachine,"Move");
            jumpState = new PlayerJumpState(this,stateMachine,"Jump");
            airState = new PlayerAirState(this,stateMachine,"Jump");
            dashState = new PlayerDashState(this,stateMachine,"Dash");
            wallSlide = new PlayerWallSlideState(this,stateMachine,"WallSlide");
            wallJump = new PlayerWallJumpState(this,stateMachine,"Jump");
            primaryAttcak = new PlayerPrimaryAttackState(this,stateMachine,"Attack");
        }

        protected override void Start()
        {
            base.Start();
            stateMachine.Initialize(idleState);
        }

        protected override void Update()
        {
             base.Update();
            stateMachine.currentState.Update();
            CheckForDashInput();
        }

        public IEnumerator BusyFor(float _second)
        {
            isBusy = true;
            yield return new WaitForSeconds(_second);
            isBusy = false;
        }

        public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

        private void CheckForDashInput()
        {
            if (IsWallDetected())
                return;
            dashUsegTimer -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsegTimer < 0)
            {
                dashUsegTimer = dashCoolDown;
                dashDir = Input.GetAxisRaw("Horizontal");
                if (dashDir == 0)
                    dashDir = facingDir;
                stateMachine.ChangeState(dashState);
            }
        }
    }
}
