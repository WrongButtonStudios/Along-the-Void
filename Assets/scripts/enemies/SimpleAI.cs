using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleAI : MonoBehaviour
{
    private enum State
    {
        Patrol,
        Hount,
        Attack
    }

    private enum EnemyType
    {
        GroundEnemy,
        FlyingEnemy
    }

    private enum WeaponsAttached
    {
        CloseCombat,
        FarCombat
    }

    [SerializeField]
    private List<EnemyType> _types;

    [SerializeField]
    private List<WeaponsAttached> _weapons;
    [SerializeField]
    private State _curState = State.Patrol;
    [SerializeField]
    private List<Transform> _wayPoints = new List<Transform>();
    [SerializeField]
    private Transform _playerPos;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private LayerMask _ignoreLayer;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _reconizedPlayerRange = 7.5f;
    [SerializeField]
    private float _stoppingDistance = 1f;
    [SerializeField, Range(1, 2)]
    private int _attackComponentCount = 1;
    [SerializeField, Range(1, 2)]
    private int _patrolComponentCount = 1;
    [SerializeField, Range(1, 2)]
    private int _hauntComponentCount = 1;
    
    private int _curWayPoint = 0;
    private Rigidbody2D _rb;

    //Components 
    private List<IHauntingComponent> _hauntingComponents;
    private List<IPatrolComponent> _patrolComponents;
    private List<IAttackComponent> _attackComponents;


    //Getter
    public Rigidbody2D RB { get { return _rb; } }
    public float Speed { get { return _speed; } }
    public List<Transform> WayPoints { get { return _wayPoints; } }
    public float StoppingDistance { get { return _stoppingDistance; } }
    public LayerMask IgnoreLayer { get { return _ignoreLayer; } }

    public float JumpForce { get { return _jumpForce; } }

    void Awake()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        Initialize();
    }

    private void Initialize()
    {
        AddPatrolAndHauntComponent();
        AddAttackComponent(); 
    }
     
    private void AddPatrolAndHauntComponent()
    {
        foreach (EnemyType type in _types)
        {
            switch (type)
            {
                case EnemyType.FlyingEnemy:
                    _hauntingComponents.Add(new FlyingHauntComponent(this));
                    _patrolComponents.Add(new FlyingPatrolComponent(this));
                    break;
                case EnemyType.GroundEnemy:
                    _hauntingComponents.Add(new GroundHauntingComponent(this));
                    _patrolComponents.Add(new GroundPatrolComponent(this));
                    break;
            }
        }
    }
    private void Start()
    {
        offset = new Vector3(0, (this.transform.localScale.y / 4), 0);
    }


    void FixedUpdate()
    {
        ExecuteState();
    }

    private void ExecuteState()
    {
        if (ChangedState())
            return;
        switch (_curState)
        {
            case State.Patrol:
                _patrolComponents[0].Patrol();
                break;
            case State.Hount:
                _hauntingComponents[0].Haunt(_playerPos.position);
                break;
            case State.Attack:
                _attackComponents[0].Attack(); 
                break;

        }
    }


    public float GetNewXDirection()
    {
        Vector2 dir = _wayPoints[_curWayPoint].position - transform.position;
        return Mathf.Sign(dir.x);
    }

    public void LookAtTarget()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = -1 * GetNewXDirection();
        transform.localScale = newScale;
    }


    private bool ChangedState()
    {
        float distance = Vector2.Distance(_playerPos.position, transform.position);
        if (distance < _reconizedPlayerRange && distance > _attackRange && _curState != State.Hount)
        {
            _curState = State.Hount;
            return true;
        }

        if (_hauntingComponents[0].GetDistanceToTargetSqr(_playerPos.position, transform.position) < (_attackRange * _attackRange) && _curState != State.Attack)
        {
            _curState = State.Attack;
            return true;
        }

        if (Vector2.Distance(_playerPos.position, transform.position) > _reconizedPlayerRange && _curState != State.Patrol)
        {
            _curState = State.Patrol;
            return true;
        }
        return false;
    }

    void AddAttackComponent()
    {
        foreach(WeaponsAttached weapon in _weapons) 
        {
            switch(weapon)
            {
                case WeaponsAttached.CloseCombat:
                    _attackComponents.Add(new CloseCombatAttackComponent(this)); 
                    break;
                case WeaponsAttached.FarCombat:
                    _attackComponents.Add(new FarCombatAttackComponent(this));
                    break; 
            }
        }
    }
}