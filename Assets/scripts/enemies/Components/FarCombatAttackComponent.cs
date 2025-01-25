using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FarCombatAttackComponent : AttackComponent
{
    [SerializeField] private BehaviourStateHandler _entity;
    [SerializeField] private EnemyMovement _movement;
    [SerializeField] private Transform _player; 
    [SerializeField] private float _fireRangeSQR;

    private GameObject _attackEffect;
    private float _speed = 100f;
    private float _startDistanceTargetBullet; 
    private bool _isCoolingDown = false;
    private bool _finnishedAttacking;
    private bool _isAttacking = false;
    private MoveSlimeball _slimeball;

    private AttackPhases _curPhase = AttackPhases.Charge;

    private enum AttackPhases
    {
        Charge,
        Attack
    }

    private void Start()
    {
        float attackRangeHalf = _entity.AttackRange / 2;
        _fireRangeSQR = attackRangeHalf * attackRangeHalf; 
    }
    public override void Attack()
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
        Vector2 direction = new Vector2(_player.position.x, 0) - new Vector2(transform.position.x, 0); //y = 0 so that the opponent does not land on the ground

        if (direction.sqrMagnitude >= _fireRangeSQR)
        {
            _movement.Move(direction);
            return;
        }
        _curPhase = AttackPhases.Attack;
        _movement.ZeroVelocity(); 
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
                Rigidbody2D rb = _attackEffect.GetComponent<Rigidbody2D>();
                _startDistanceTargetBullet = (rb.position - (Vector2)_entity.Player.position).magnitude;
                rb.gravityScale = PhysicUttillitys.TimeScale; 
                FireSlimeBall(rb);
                _isAttacking = false;
                StartCoroutine(CoolDown());
            }
        }
        else
        {
            _curPhase = AttackPhases.Charge; 
        }
    }

    private void FireSlimeBall(Rigidbody2D rb)
    {
        Vector2 startVelocity = (_player.position - transform.position).normalized * _speed ;
        _slimeball = rb.GetComponent<MoveSlimeball>(); //Slimeball pool will get Adjusted, so that the Class is return insteat of an GameObject. This is just to test, if its work like it is intendet after the rework.
        _slimeball.Instantiate(startVelocity, _startDistanceTargetBullet, _entity, rb); 
    }

    private Vector2 CalculateAimOffset(Vector2 linearDir, Rigidbody2D rb)
    {
        Vector2 gravity = Physics2D.gravity * rb.gravityScale;
        float estimateFlyDuration = (linearDir.magnitude / _speed) / PhysicUttillitys.TimeScale; 
        return gravity*-1 * estimateFlyDuration * (Time.fixedDeltaTime * PhysicUttillitys.TimeScale); 
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(1.5f);
        SlimeBallPool.Instance.DeactivateSlimeball(_attackEffect);
        _attackEffect = null; 
        _isCoolingDown = false;
        Debug.Log("Finnished Cooldown " + !_isCoolingDown); 
    }

    public override bool FinnishedAttack()
    {
        return _finnishedAttacking; 
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
        _isCoolingDown = false;
        _isAttacking = false;
    }
}