using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class Enemy : Entity
    {
        [SerializeField] protected LayerMask whatIsPlayer;

        [Header("Move Info")] 
        public float moveSpeed;
        public float idleTime;
        public float battleTime;
        private float defaultMoveSpeed;
        [Header("Attack Info")] 
        public float attackDistance;
        public float attackCoolDowm;
        [Header("Stunned Info")] 
        public float stunDuration;
        public Vector2 stunDirection;
        protected bool canBeStunned;
        [SerializeField] protected GameObject counterImage;

        [HideInInspector] public float lastTimeAtatck;
    
    
    
        public EnemyStateMachine stateMachine { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            stateMachine = new EnemyStateMachine();
            defaultMoveSpeed = moveSpeed;
        }

        protected override void Update()
        {
            base.Update();
            stateMachine.currentState.Update();
        
        }

        protected virtual IEnumerator FreezeTimerFor(float _seconds)
        {
            FreezeTime(true);
            yield return new WaitForSeconds(_seconds);
            FreezeTime(false);
        }

        public virtual void FreezeTime(bool _timeFrozen)
        {
            if (_timeFrozen)
            {
                moveSpeed = 0;
                anim.speed = 0;
            }
            else
            {
                moveSpeed = defaultMoveSpeed;
                anim.speed = 1;
            }
        }
            

        public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

        public virtual void OpenCounterAttackWindow()
        {
            canBeStunned = true;
            counterImage.SetActive(true);
        }

        public virtual void CloseCounterAttackWindow()
        {
            canBeStunned = false;
            counterImage.SetActive(false);
        }

        public virtual bool CanBeStunned()
        {
            if (canBeStunned)
            {
                CloseCounterAttackWindow();
                return true;
            }
            else
                return false;
            
        }

        public virtual RaycastHit2D IsPlayerDetected() =>
            Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, 
                whatIsPlayer);

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color =Color.yellow;
            Gizmos.DrawLine(transform.position,new Vector3(transform.position.x + attackDistance * facingDir,transform.position.y));
        }
    }
}