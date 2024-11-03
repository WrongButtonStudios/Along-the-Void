using UnityEngine;

public class CloseCombatAttackComponent : MonoBehaviour, IAttackComponent
{
    private SimpleAI _entity;
    private bool _isCoolingDown = false;
    private float _bodyCheckSpeed;
    private bool _bodyCheck = false; 
    public void Attack()
    {
        BodyCheck(); 
    }

    public void Init(SimpleAI entity)
    {
        _entity = entity;
        _bodyCheckSpeed = _entity.Speed * 13f; 
    }

    private void BodyCheck()
    {
        Vector2 pos = _entity.transform.position;
        Vector2 ChargeDir = (_entity.PlayerPos - pos).normalized;

        _entity.RB.AddForce(ChargeDir * _entity.Speed);

        if ((_entity.PlayerPos - pos).sqrMagnitude < (4 * 4) && IsGrounded())
        {
            _entity.RB.AddForce(Vector2.up * _entity.JumpForce, ForceMode2D.Impulse);
            _isCoolingDown = false;
            _bodyCheck = true; 

        }

        if (!IsGrounded() && !_isCoolingDown && _bodyCheck)
        {
            _bodyCheck = false; 
            _isCoolingDown = true;
            _entity.RB.AddForce(ChargeDir * _bodyCheckSpeed, ForceMode2D.Impulse); 
        }
    }

    private bool IsGrounded()
    {
        if (Physics2D.Raycast(_entity.transform.position, -Vector2.up, 1.45f, ~_entity.IgnoreLayer))
        {
            return true;
        }
        return false;
    }


}
