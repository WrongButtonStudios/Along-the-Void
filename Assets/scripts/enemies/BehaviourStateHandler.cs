using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BehaviourStateHandler : MonoBehaviour
{
    [SerializeField] private BehaviourState _curState = BehaviourState.Patrol;
    [SerializeField] private float _attackRange = 2.0f;
    [SerializeField] private float _aggroRange = 7.5f;
    [SerializeField] private Enemy _entity;
    [SerializeField] private List<Transform> _wayPoints = new();
    [SerializeField] private float _stoppingDistance = 1.0f;
    [SerializeField] private List<IHuntingComponent> _huntPatterns = new();
    [SerializeField] private List<IPatrolComponent> _patrolPatterns = new();
    [SerializeField] private List<IAttackComponent> _attackPatterns = new();
    [SerializeField] private Rigidbody2D _rb;

    private sbyte _currentAttackPattern = 0;
    private sbyte _currentPatrolPattern = 0;
    private Scene _scene;

    public List<IHuntingComponent> HuntPatterns { get => _huntPatterns; }
    public List<IPatrolComponent> PatrolPatterns { get => _patrolPatterns;}
    public List<IAttackComponent> AttackPatterns { get => _attackPatterns;}

    public float StoppingDistance { get { return _stoppingDistance; } }
    public EnemyMovement Movement { get; private set; }
    public float AttackRange { get { return _attackRange; } }
    public Enemy Enemy { get { return _entity; } }
    public Scene Scene { get { return _scene;  } }
    public float ReconizedPlayerRange { get { return _aggroRange; } }

    //this gets removed later
    private void Start()
    {
        Initialize();
    }

    //I'm not sure if this belongs here. 
    private void Initialize()
    {
        Movement = this.GetComponent<EnemyMovement>();
        if(_types[0] == EnemyType.GroundEnemy) {
            _rb.gravityScale = PhysicUttillitys.TimeScale;
        }
    }

    void FixedUpdate()
    {
        if(_entity.Status == Enemy.Status.Frozen) {
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
                 PatrolPatterns[_selectedPatrolComponent].Patrol();
                 break;
            case BehaviourState.Hunt:
                 HuntPatterns[_selectedPatrolComponent].Hunt();
                 break;
            case BehaviourState.Attack:
                 AttackPatterns[_selectedWeapon].Attack();
                 break;
        }
    }

    public void SetScene(Scene scene)
    {
        _scene = scene; 
    }

    public void InitEnemyWaypoints(List<Transform> wps)
    {
        foreach (IPatrolComponent p in PatrolPatterns)
        {
            p.SetWayPoints(wps); 
        }
    }
}