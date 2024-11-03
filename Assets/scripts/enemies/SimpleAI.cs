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

    //Placeholder VFX stuff
    [SerializeField]
    private GameObject _attackEffect; 
    
    private Rigidbody2D _rb;

    

    //Components 
    private List<IHauntingComponent> _hauntingComponents = new List<IHauntingComponent>();
    private List<IPatrolComponent> _patrolComponents= new List<IPatrolComponent>();
    private List<IAttackComponent> _attackComponents = new List<IAttackComponent>();


    //Getter
    public Rigidbody2D RB { get { return _rb; } }
    public float Speed { get { return _speed; } }
    public List<Transform> WayPoints { get { return _wayPoints; } }
    public float StoppingDistance { get { return _stoppingDistance; } }
    public LayerMask IgnoreLayer { get { return _ignoreLayer; } }
    public GameObject AttackVFX { get { return _attackEffect;  } }
    public Vector2 PlayerPos { get { return (Vector2)_playerPos.position; } }

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
                    var FlyingHaunting = this.gameObject.AddComponent<FlyingHauntComponent>();
                    FlyingHaunting.Init(this);
                    _hauntingComponents.Add(FlyingHaunting);
                    var FlyingPatrol = this.gameObject.AddComponent<FlyingPatrolComponent>();
                    FlyingPatrol.Init(this);
                    _patrolComponents.Add(FlyingPatrol);
                    break;
                case EnemyType.GroundEnemy:
                    var groundHaunting = this.gameObject.AddComponent<GroundHauntingComponent>();
                    groundHaunting.Init(this);
                    _hauntingComponents.Add(groundHaunting);
                    var groundPatrol = this.gameObject.AddComponent<GroundPatrolComponent>();
                    groundPatrol.Init(this);
                    _patrolComponents.Add(groundPatrol);
                    break;
            }
        }
    }
    void AddAttackComponent()
    {
        foreach(WeaponsAttached weapon in _weapons) 
        {
            switch(weapon)
            {
                case WeaponsAttached.CloseCombat:
                    var closeCombat = this.gameObject.AddComponent<CloseCombatAttackComponent>();
                    closeCombat.Init(this);
                    _attackComponents.Add(closeCombat);
                    break;
                case WeaponsAttached.FarCombat:
                    var farCombat = this.gameObject.AddComponent<FarCombatAttackComponent>();
                    farCombat.Init(this); 
                    _attackComponents.Add(farCombat);
                    break; 
            }
        }
    }

    void FixedUpdate()
    {
        ExecuteState();
    }

    private void ExecuteState()
    {
        //if (ChangedState())
            //return;
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
}