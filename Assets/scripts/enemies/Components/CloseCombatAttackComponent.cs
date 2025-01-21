using UnityEngine;

public class CloseCombatAttackComponent : MonoBehaviour, IAttackComponent
{
    private BehaviourStateHandler _entity;
    private bool _isCoolingDown = false;
    private float _bodyCheckSpeed;
    private bool _finnishedAttacking;
    private bool _isAttacking;
    private Vector2 _chargeDir;
    private AttackPhases _curPhase = AttackPhases.Charge;
    private EnemyMovement _movement;
    private EnemyCollisionHandler _collisionHandler; 

    private bool _doJump; 

    private enum AttackPhases
    {
        Charge,
        Attack,
        BackUp
    }

    private void Start()
    {
        _movement = this.GetComponent<EnemyMovement>();
        _collisionHandler = GetComponent<EnemyCollisionHandler>();
    }

    public void Attack()
    {
        switch (_curPhase)
        {
            case AttackPhases.Charge:
                Charge();
                break;
            case AttackPhases.Attack:
                BodyCheck();
                break;
            case AttackPhases.BackUp:
                BackUp(); 
                break;
        }
    }

    private void FixedUpdate()
    {
        bool isJumping = false;
        if (_doJump)
            isJumping = _movement.Jump();
        if (isJumping == false && _doJump)
        {
            _doJump = false;
            _movement.SetGravitiyScale(1);
        }
    }

    public bool FinnishedAttack()
    {
        return _finnishedAttacking; 
    }

    public void Init(BehaviourStateHandler entity)
    {
        _entity = entity;
        _bodyCheckSpeed = _movement.Speed * 13f; 
    }

    private void Charge()
    {
        _isAttacking = true;
        _finnishedAttacking = false;

        _chargeDir = _movement.CalculateDirection(transform.position, _entity.Player.position); 
        _movement.Move(_chargeDir); 
        bool isGrounded = _collisionHandler.IsGrounded(); 
        if (_chargeDir.sqrMagnitude <= (4 * 4) && isGrounded && !_doJump)
        {
            _doJump = true;
            _isCoolingDown = false; 
        }
    } 

    private void BodyCheck()
    {
        if (!_collisionHandler.IsGrounded() && !_isCoolingDown)
        {
            _movement.Move(_chargeDir, _bodyCheckSpeed);
            return; 
        }
        _movement.SetGravitiyScale(1);
        _curPhase = AttackPhases.BackUp;
    }

    private void BackUp()
    {
        Vector2 backUpDirection = _movement.CalculateDirection(_entity.Player.position, transform.position); 
        _movement.Move(backUpDirection); 
        float distanceSqr = backUpDirection.sqrMagnitude;
        if (distanceSqr > (4 * 4))
        {
            _curPhase = AttackPhases.Charge;
            _isCoolingDown = false;
            _finnishedAttacking = true;
            _entity.Movement.ZeroVelocity();
        }
    }

    public void ResetAttackStatus()
    {
        _finnishedAttacking = false; 
    }

    public bool IsAttacking()
    {
        return _isAttacking; 
    }

    public void Exit()
    {
        _doJump = false; 
    }
}
