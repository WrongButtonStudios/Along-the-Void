using UnityEngine;

[System.Serializable]
public class CloseCombatAttackComponent : AttackComponent
{
    [SerializeField] private BehaviourStateHandler _entity;
    [SerializeField] private EnemyMovement _movement;
    [SerializeField] private EnemyCollisionHandler _collisionHandler;
    [SerializeField] private float _bodyCheckSpeed = 150f;
    [SerializeField] private Transform _player; 

    private bool _isCoolingDown = false;
    private bool _finnishedAttacking;
    private bool _isAttacking;
    private Vector2 _chargeDir;
    private AttackPhases _curPhase = AttackPhases.Charge;

    private bool _doJump; 

    private enum AttackPhases
    {
        Charge,
        Attack,
        BackUp
    }

    public override void Attack()
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
        {
            _movement.ZeroVelocityX(); 
            isJumping = _movement.Jump();

            if (isJumping == false && _doJump)
            {
                _doJump = false;
                _movement.SetGravitiyScale(1);
                _curPhase = AttackPhases.Attack; 
            }
        }
    }

    public override bool FinnishedAttack()
    {
        return _finnishedAttacking; 
    }

    private void Charge()
    {
        _isAttacking = true;
        _finnishedAttacking = false;

        _chargeDir = _movement.CalculateDirection(transform.position, _player.position); 
        _movement.Move(_chargeDir); 
        bool isGrounded = _collisionHandler.IsGrounded(1.5f); 
        if (_chargeDir.sqrMagnitude <= (2.5f * 2.5f) && isGrounded && !_doJump)
        {
            _movement.CalculateMaxJumpHight(); 
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
        Vector2 backUpDirection = _movement.CalculateDirection(_player.position, transform.position); 
        _movement.Move(backUpDirection); 
        float distanceSqr = backUpDirection.sqrMagnitude;
        if (distanceSqr >= (5 * 5))
        {
            _curPhase = AttackPhases.Charge;
            _isCoolingDown = false;
            _finnishedAttacking = true;
            _entity.Movement.ZeroVelocity();
        }
    }

    public override void ResetAttackStatus()
    {
        _finnishedAttacking = false; 
    }

    public override bool IsAttacking()
    {
        return _isAttacking; 
    }

    public override void Exit()
    {
        _doJump = false; 
    }
}
