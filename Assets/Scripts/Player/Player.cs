using System.Collections;
using Skills;
using UnityEngine;

namespace Player
{
    
    public class Player : Entity
    {
        [Header("Attack info")] 
        public Vector2[] attackMovement;

        public float counterAttackDuration;
    
        public bool isBusy { get; private set; }
        [Header("Move info")] 
        public float moveSpeed = 12f;
        public float jumpForce;
        public float swordReturnImpact;
        private float defaultMoveSpeed;
        private float defaultJumpForce; 
        [Header("Dash info")] 
        public float dashSpeed;
        public float dashDuration;
        private float defaultDashSpeed;
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
        public PlayerCounterAttackState counterAttack { get; private set; }
        public PlayerAimSwordState aimSword { get; private set; }
        public PlayerCatchSwordState catchSword { get; private set; }
        public PlayerBlackholeState blackhole { get; private set; }
        public PlayerDeadState deadState { get; private set; }
    

        #endregion
        public SkillManager skill { get; private set; }
        public GameObject sword { get; private set; }
    
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
            counterAttack = new PlayerCounterAttackState(this,stateMachine,"CounterAttack");
            aimSword = new PlayerAimSwordState(this,stateMachine,"AimSword");
            catchSword = new PlayerCatchSwordState(this,stateMachine,"CatchSword");
            blackhole = new PlayerBlackholeState(this,stateMachine,"Jump");
            deadState = new PlayerDeadState(this,stateMachine,"Die");
        }

        protected override void Start()
        {
            base.Start();
            skill = SkillManager.instance;
            stateMachine.Initialize(idleState);
            defaultMoveSpeed = moveSpeed;
            defaultJumpForce = jumpForce;
            defaultDashSpeed = dashSpeed;
        }

        protected override void Update()
        {
             base.Update();
            stateMachine.currentState.Update();
            CheckForDashInput();
            if (Input.GetKeyDown(KeyCode.F))
            {
                skill.crystal.CanUseSkill();
            }
        }

        public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
        {
            moveSpeed = moveSpeed * (1 - _slowPercentage);
            jumpForce = jumpForce * (1 - _slowPercentage);
            dashSpeed = dashSpeed * (1 - _slowPercentage);
            anim.speed = anim.speed * (1 - _slowPercentage);
            Invoke("ReturnDefaultSpeed", _slowDuration);

        }

        protected override void ReturnDefaultSpeed()
        {
            base.ReturnDefaultSpeed();
            moveSpeed = defaultMoveSpeed;
            jumpForce = defaultJumpForce;
            dashSpeed = defaultDashSpeed;

        }

        public void AssingNewSword(GameObject _newSowrd)
        {
            sword = _newSowrd;
            
        }

        public void CatchTheSword()
        {
            stateMachine.ChangeState(catchSword);
            Destroy(sword);
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
            
            if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
            {
                
                dashDir = Input.GetAxisRaw("Horizontal");
                if (dashDir == 0)
                    dashDir = facingDir;
                stateMachine.ChangeState(dashState);
            }
        }

        public override void Die()
        {
            base.Die();
            stateMachine.ChangeState(deadState);
        }
    }
}
