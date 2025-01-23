using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BehaviourStateHandler : MonoBehaviour
{
    enum EnemyType
    {
        GroundEnemy,
        FlyingEnemy
    }

    [SerializeField] private BehaviourState _curState = BehaviourState.Patrol;
    [SerializeField] private List<EnemyType> _types;
    [SerializeField] private float _attackRange = 2.0f;
    [SerializeField] private float _aggroRange = 7.5f;
    [SerializeField] private Enemy _entity;
    [SerializeField] private float _stoppingDistance = 1.0f;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private List<HuntingComponent> _huntPatterns = new();
    [SerializeField] private List<PatrolComponent> _patrolPatterns = new();
    [SerializeField] private List<AttackComponent> _attackPatterns = new();
    [SerializeField] private EnemyMovement _movement;

    private sbyte _currentAttackPattern = 0;
    private sbyte _currentHuntPattern = 0;
    private sbyte _currentPatrolPattern = 0;
    private Scene _scene;

    public List<HuntingComponent> HuntPatterns { get => _huntPatterns; }
    public List<PatrolComponent> PatrolPatterns { get => _patrolPatterns;}
    public List<AttackComponent> AttackPatterns { get => _attackPatterns;}

    public Transform Player{
        get {
            return _playerTransform;
        }
    }
    public float StoppingDistance { get { return _stoppingDistance; } }
    public EnemyMovement Movement { get { return _movement;  } }
    public float AttackRange { get { return _attackRange; } }
    public Enemy Enemy { get { return _entity; } }
    public Scene Scene { get { return _scene;  } }
    public float ReconizedPlayerRange { get { return _aggroRange; } }

    //this gets removed later
    private void Start()
    {
        if (_types[0] == EnemyType.GroundEnemy)
        {
            _rb.gravityScale = PhysicUttillitys.TimeScale;
        }
    }

    void FixedUpdate()
    {
        if(_entity.Debuffs.Debuffs == Debuffs.Frozen)
        {
            return;
        }
        _curState = BehaviourStateMachine.UpdateState(this, _curState);
        ExecuteState();
    }

    private void ExecuteState()
    {
        switch (_curState)
        {
            case BehaviourState.Patrol:
                 PatrolPatterns[_currentPatrolPattern].Patrol();
                 break;
            case BehaviourState.Hunt:
                 HuntPatterns[_currentHuntPattern].Hunt();
                 break;
            case BehaviourState.Attack:
                 AttackPatterns[_currentAttackPattern].Attack();
                 break;
        }
    }

    public void SetScene(Scene scene)
    {
        _scene = scene; 
    }
}