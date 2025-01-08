using UnityEngine;

public class CloseCombatAttackComponent : MonoBehaviour, IAttackComponent
{
    private SimpleAI _entity;
    private bool _isCoolingDown = false;
    private float _bodyCheckSpeed;
    private bool _bodyCheck = false;
    private bool _finnishedAttacking;
    private bool _isAttacking;
    private Vector2 _chargeDir;
    private bool _clearedForce = false; 
    private AttackPhases _curPhase = AttackPhases.Charge;
    private float _jumpHight = 2;
    private float _maxJumpHight; 

    private bool _doJump; 

    private enum AttackPhases
    {
        Charge,
        Attack,
        BackUp
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
        Jump();
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

    private void Jump()
    {
        if (!_doJump) //early out if not jumping 
            return;

        if (_entity.transform.position.y < _maxJumpHight)
        {
            _entity.RB.MovePosition((Vector2)transform.position + (Vector2.up * _entity.JumpForce * (Time.fixedDeltaTime * _entity.TimeScale)) );
            Debug.Log(Vector2.up * _entity.JumpForce * (Time.fixedDeltaTime * _entity.TimeScale));
        }
        else
            _doJump = false;
        
    }

    private void Charge()
    {
        if (_clearedForce)
            Unfreeze();
        _isAttacking = true;
        _finnishedAttacking = false; 
        Vector2 pos = _entity.transform.position;
        _chargeDir = (_entity.PlayerPos - pos).normalized;

        _entity.RB.MovePosition((Vector2)transform.position + (_chargeDir * _entity.Speed * (Time.fixedDeltaTime * _entity.TimeScale)));
        Debug.Log(_chargeDir * _entity.Speed * (Time.fixedDeltaTime * _entity.TimeScale)); 

        if ((_entity.PlayerPos - pos).sqrMagnitude < (4 * 4) && IsGrounded() && !_doJump)
        {
            _doJump = true;
            _maxJumpHight = _entity.transform.position.y + _jumpHight; 
            _isCoolingDown = false;
            _bodyCheck = true;
            Debug.Log("Springe"); 
        } 
        if(!IsGrounded(2))
            _curPhase = AttackPhases.Attack;
    } 

    private void BodyCheck()
    {
        if (!IsGrounded() && !_isCoolingDown && _bodyCheck)
        {
            _bodyCheck = false;
            _isCoolingDown = true;
            //To-DO change Addforce to MovePosition 
            _entity.RB.MovePosition((Vector2) transform.position + (_chargeDir * _bodyCheckSpeed * (Time.fixedDeltaTime * _entity.TimeScale)));
        }
        else
        {
            ClearForce();
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
        Vector2 distance = ((Vector2)_entity.transform.position - _entity.PlayerPos); //variable has bad naming but i dont know a better one
        //To-DO change Addforce to MovePosition 
        _entity.RB.MovePosition((Vector2)transform.position + (distance.normalized * _entity.Speed * (Time.fixedDeltaTime * _entity.TimeScale)));
        float distanceSqr = distance.sqrMagnitude;
        if (distanceSqr < (5 * 5) == false)
        {
            _curPhase = AttackPhases.Charge;
            _isCoolingDown = false;
            _finnishedAttacking = true;
            ClearForce();
        }
    }
    private bool IsGrounded(float groundDist = 1.45f)
    {
        if (Physics2D.Raycast(_entity.transform.position, -Vector2.up, groundDist, ~_entity.IgnoreLayer))
        {
            return true;
        }
        return false;
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
