using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarCombatAttackComponent : MonoBehaviour, IAttackComponent
{
    private SimpleAI _entity;
    private GameObject _attackEffect;
    private float _speed = 100f;
    private Rigidbody2D _rb;
    private bool _isCoolingDown = false;
    private bool _finnishedAttacking;
    public bool _isAttacking = false; 

    public void Init(SimpleAI entity)
    {
        _entity = entity;
    }
    public void Attack()
    {
        if (!_isCoolingDown)
        {
            _isAttacking = true; 
            _isCoolingDown = true;
            _attackEffect = Instantiate(_entity.AttackVFX, _entity.transform.position, Quaternion.identity);
             _rb = _attackEffect.GetComponent<Rigidbody2D>(); 
             if (_rb == null)
             {
                 _rb = _attackEffect.AddComponent<Rigidbody2D>();
             }
             FireSlimeBall();
            _isAttacking = false; 
            StartCoroutine(CoolDown()); 
        }
    }

    private void FireSlimeBall()
    {
        Vector2 targetPos = _entity.PlayerPos;
        Vector2 force = (targetPos - (Vector2)_entity.transform.position + CalculateAimOffset(targetPos - (Vector2)_entity.transform.position)).normalized * _speed;
        Debug.Log(force); 
        _rb.AddForce(force, ForceMode2D.Impulse); 

    }

    private Vector2 CalculateAimOffset(Vector2 linearDir)
    {
        Vector2 gravity = Physics2D.gravity * _rb.gravityScale;
        float estimateFlyDuration = linearDir.magnitude / _speed; 
        return ((gravity*-1) * estimateFlyDuration) * Time.fixedDeltaTime; 
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(1.5f);
        _isCoolingDown = false; 
    }

    public bool FinnishedAttack()
    {
        return _finnishedAttacking; 
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
