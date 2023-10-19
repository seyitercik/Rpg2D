using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move info")] 
    public float moveSpeed = 12f;
    public float jumpForce;
    [Header("Dash info")] 
    [SerializeField] private float dashCoolDown;

    private float dashUsegTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir{ get; private set; }

    [Header("Collision info")] 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;
   
    #region Components

    public Animator anim { get; private set; }
    public Rigidbody2D PlayerRigidbody2D { get; private set; }

    #endregion

    #region State

    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    

    #endregion
    
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this,stateMachine,"Idle");
        moveState = new PlayerMoveState(this,stateMachine,"Move");
        jumpState = new PlayerJumpState(this,stateMachine,"Jump");
        airState = new PlayerAirState(this,stateMachine,"Air");
        dashState = new PlayerDashState(this,stateMachine,"Dash");
        wallSlide = new PlayerWallSlideState(this,stateMachine,"WallSlide");
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        PlayerRigidbody2D = GetComponent<Rigidbody2D>();
        stateMachine.Initialize(idleState);
        
    }

    private void Update()
    {
        stateMachine.currentState.Update();
        CheckForDashInput();
        
    }

    private void CheckForDashInput()
    {
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

    public void SetVelocity(float xVelocity,float yVelocity)
    {
        PlayerRigidbody2D.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance,
        whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right*facingDir, wallCheckDistance,
        whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,new Vector3(groundCheck.position.x,groundCheck.position.y-groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x + wallCheckDistance,wallCheck.position.y));
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }

    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
        
        
    }
}