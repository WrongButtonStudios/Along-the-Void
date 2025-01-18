using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarCombatAttackComponent : MonoBehaviour, IAttackComponent
{
    private SimpleAI _entity;
    private float _fireRangeSQR;
    private GameObject _attackEffect;
    private float _speed = 100f;
    private Rigidbody2D _rb;
    private float _startDistanceTargetBullet; 
    private bool _isCoolingDown = false;
    private bool _finnishedAttacking;
    private bool _isAttacking = false;

    private AttackPhases _curPhase = AttackPhases.Charge;
    private bool _clearedForce;
    private bool _switchedFromeOtherState = true;
    private MoveSlimeball _slimeball; 
    private enum AttackPhases
    {
        Charge,
        Attack
    }

    public void Init(SimpleAI entity)
    {
        _entity = entity;
        _fireRangeSQR = (_entity.MaxRange / 2) * (_entity.MaxRange / 2);
    }
    public void Attack()
    {
        switch (_curPhase)
        {
            case AttackPhases.Charge:
                Charge();
                break;
            case AttackPhases.Attack:
                Shoot();
                break;
            default:
                Debug.LogError("This isnt a defined Phase..." + _curPhase);
                break;
        }
    }

    private void Charge()
    {
        if (_switchedFromeOtherState)
        {
            _switchedFromeOtherState = false; 
            ClearForce();
        }
        if (_clearedForce)
            Unfreeze(); 
        Vector2 direction = new Vector2(_entity.PlayerPos.x, 0) - new Vector2(transform.position.x, 0);

        if (direction.sqrMagnitude >= _fireRangeSQR)
        {
            Vector2 movementVelocity = direction.normalized * _entity.Speed * (Time.fixedDeltaTime * PhysicUttillitys.TimeScale);
            _entity.RB.velocity += movementVelocity; 
        }
        else if (direction.sqrMagnitude <= _fireRangeSQR)
        {
            _curPhase = AttackPhases.Attack;
            ClearForce();
            return;

        }
    }

    private void Shoot()
    {
        Vector2 direction = new Vector2(_entity.PlayerPos.x, 0) - new Vector2(transform.position.x, 0);
        if (direction.sqrMagnitude <= _fireRangeSQR)
        {
            if (!_isCoolingDown)
            {
                _isAttacking = true;
                _isCoolingDown = true;
                _attackEffect = SlimeBallPool.Instance.GetPooledSlimeBall();
                _attackEffect.SetActive(true);
                _attackEffect.transform.position = this.transform.position; 
                Debug.Log("Activated Slimeball: " + _attackEffect.activeInHierarchy);
                _rb = _attackEffect.GetComponent<Rigidbody2D>();
                if (_rb == null)
                {
                    _rb = _attackEffect.AddComponent<Rigidbody2D>();
                }
                _startDistanceTargetBullet = (_rb.position - _entity.PlayerPos).magnitude;
                _rb.gravityScale = PhysicUttillitys.TimeScale; 
                FireSlimeBall();
                _isAttacking = false;
                StartCoroutine(CoolDown());
            }
        }
        else
        {
            _curPhase = AttackPhases.Charge; 
        }
    }
    private void FireSlimeBall()
    {
        Vector2 targetPos = _entity.PlayerPos;
        Vector2 startVelocity = (targetPos - (Vector2)_entity.transform.position + CalculateAimOffset(targetPos - (Vector2)_entity.transform.position)).normalized * _speed ;
        _slimeball = _rb.GetComponent<MoveSlimeball>(); //Slimeball pool will get Adjusted, so that the Class is return insteat of an GameObject. This is just to test, if its work like it is intendet after the rework.
        _slimeball.Instantiate(startVelocity, _startDistanceTargetBullet, _entity, _rb); 
    }

    private Vector2 CalculateAimOffset(Vector2 linearDir)
    {
        Vector2 gravity = Physics2D.gravity * _rb.gravityScale;
        float estimateFlyDuration = (linearDir.magnitude / _speed) / PhysicUttillitys.TimeScale; 
        return gravity*-1 * estimateFlyDuration * (Time.fixedDeltaTime * PhysicUttillitys.TimeScale); 
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

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(1.5f);
        _slimeball.Deactivate();
        _attackEffect.gameObject.SetActive(false);
        _attackEffect = null;
        _rb = null; 
        _isCoolingDown = false;
        Debug.Log("Finnished Cooldown " + !_isCoolingDown); 
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

    public void Exit()
    {
        Unfreeze(); 
        _isCoolingDown = false;
        _isAttacking = false;
        _switchedFromeOtherState = true;

    }
}

