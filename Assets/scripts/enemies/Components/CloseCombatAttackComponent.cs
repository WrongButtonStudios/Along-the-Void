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
        if (!_doJump)
        {
            return;
        }
        bool isJumping = false;
        isJumping = _movement.Jump();
        Debug.Log("Jumping..."); 
        if (isJumping == false && _doJump)
        {
            _doJump = false;
            _movement.SetGravitiyScale(1);
            _curPhase = AttackPhases.Attack;
            Debug.Log("finnished jumping..."); 
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
        if (_chargeDir.sqrMagnitude <= (5f * 5f) && _collisionHandler.IsGrounded() && !_doJump)
        {
            _movement.CalculateMaxJumpHight();
            _movement.SetGravitiyScale(0); 
            _isCoolingDown = false; 
            _doJump = true;
            Debug.Log("I should jump now..."); 
        }
    } 

    private void BodyCheck()
    {
        if (!_collisionHandler.IsGrounded() && !_isCoolingDown)
        {
            _movement.Move(_chargeDir, _bodyCheckSpeed);
            return; 
        }
        _curPhase = AttackPhases.BackUp;
    }

    private void BackUp()
    {
        Vector2 backUpDirection = _movement.CalculateDirection(_player.position, transform.position); 
        _movement.Move(backUpDirection); 
        float distanceSqr = backUpDirection.sqrMagnitude;
        float chargeDist = _entity.AttackRange - 2;  
        if (distanceSqr >= (chargeDist * chargeDist))
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
