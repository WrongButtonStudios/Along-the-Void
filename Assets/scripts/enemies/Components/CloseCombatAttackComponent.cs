using UnityEngine;

public class CloseCombatAttackComponent : MonoBehaviour, IAttackComponent
{
    private SimpleAI _entity;
    private bool _isCoolingDown = false;
    private float _bodyCheckSpeed;
    private bool _finnishedAttacking;
    private bool _isAttacking;
    private Vector2 _chargeDir;
    private bool _clearedForce = false; 
    private AttackPhases _curPhase = AttackPhases.Charge;
    private EnemyMovement _movement;
    private EnemyCollisionHandler _collisionHandler; 
    private float _maxJumpHight; 

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
                FinnishState();
                break;
            default:
                Debug.LogError("This isnt a defined Phase...");
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
            _entity.RB.gravityScale = 1;
            ClearForce(); 
        }
    }

    public bool FinnishedAttack()
    {
        return _finnishedAttacking; 
    }

    public void Init(SimpleAI entity)
    {
        _entity = entity;
        _bodyCheckSpeed = _entity.Speed * 13f; 
    }

    private void Charge()
    {
        if (_clearedForce)
            Unfreeze();
        _isAttacking = true;
        _finnishedAttacking = false; 
        Vector2 pos = _entity.transform.position;
        _chargeDir = (_entity.PlayerPos - pos).normalized;
        _movement.Move(_chargeDir); 
        bool isGrounded = _collisionHandler.IsGrounded(); 
        if ((_entity.PlayerPos - pos).sqrMagnitude <= (4 * 4) && isGrounded && !_doJump)
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
        }
        else
        {
            ClearForce();
            _entity.RB.gravityScale = 1; 
            _curPhase = AttackPhases.BackUp;
        }
    }

    void FinnishState()//name is wip lol
    {
        if (_clearedForce)
            Unfreeze();
        else
            BackUp();
    }

    private void BackUp()
    {
        Vector2 backUpDirection = ((Vector2)_entity.transform.position - _entity.PlayerPos);
        _movement.Move(backUpDirection); 
        float distanceSqr = backUpDirection.sqrMagnitude;
        if (distanceSqr > (4 * 4))
        {
            _curPhase = AttackPhases.Charge;
            _isCoolingDown = false;
            _finnishedAttacking = true;
            ClearForce();
        }
    }

    private void ClearForce()
    {
        _clearedForce = true;
        _doJump = false; 
        _entity.RB.constraints = RigidbodyConstraints2D.FreezePosition;
        _entity.RB.velocity = Vector2.zero;
    }

    private void Unfreeze()
    {
        _clearedForce = false; 
        _entity.RB.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation; 
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
        Unfreeze(); 
    }
}
