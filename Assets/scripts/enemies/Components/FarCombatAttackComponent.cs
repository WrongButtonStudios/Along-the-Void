using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FarCombatAttackComponent : MonoBehaviour, IAttackComponent
{
    private BehaviourStateHandler _entity;
    private float _fireRangeSQR;
    private GameObject _attackEffect;
    private float _speed = 100f;
    private Rigidbody2D _rb;
    private float _startDistanceTargetBullet; 
    private bool _isCoolingDown = false;
    private bool _finnishedAttacking;
    private bool _isAttacking = false;

    private AttackPhases _curPhase = AttackPhases.Charge;
    private MoveSlimeball _slimeball;
    private EnemyMovement _movement; 
    private enum AttackPhases
    {
        Charge,
        Attack
    }

    public void Init(BehaviourStateHandler entity)
    {
        _movement = this.GetComponent<EnemyMovement>(); 
        _entity = entity;
        _fireRangeSQR = (_entity.AttackRange / 2) * (_entity.AttackRange / 2);
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
        }
    }

    private void Charge()
    {
        Vector2 direction = new Vector2(_entity.Player.position.x, 0) - new Vector2(transform.position.x, 0); //y = 0 so that the opponent does not land on the ground

        if (direction.sqrMagnitude >= _fireRangeSQR)
        {
            _movement.Move(direction);
            return;
        }
        _curPhase = AttackPhases.Attack;
        _entity.Movement.ZeroVelocity(); 
    }

    //To-Do: Hier drin passiert eindeutig zu viel stuff
    private void Shoot()
    {
        Vector2 direction = _movement.CalculateDirectionX(transform.position, _entity.Player.position); 
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
                _startDistanceTargetBullet = (_rb.position - (Vector2)_entity.Player.position).magnitude;
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
        Vector2 targetPos = _entity.Player.position;
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
        _isCoolingDown = false;
        _isAttacking = false;
    }
}