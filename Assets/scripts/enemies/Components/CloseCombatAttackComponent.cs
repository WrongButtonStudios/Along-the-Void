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

    void FixedUpdate() // remove this after debugging
    {
        Debug.Log(_curPhase + " " + _entity.RB.velocity.magnitude); 
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

        _entity.RB.AddForce(_chargeDir * _entity.Speed);

        if ((_entity.PlayerPos - pos).sqrMagnitude < (4 * 4) && IsGrounded())
        {
            _entity.RB.AddForce(Vector2.up * _entity.JumpForce, ForceMode2D.Impulse);
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
            _entity.RB.AddForce(_chargeDir * _bodyCheckSpeed, ForceMode2D.Impulse);
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
        _entity.RB.AddForce(distance.normalized * _entity.Speed);
        float distanceSqr = distance.sqrMagnitude;
        if (distanceSqr > (2 * 2) && distanceSqr < (4 * 4))
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
}
