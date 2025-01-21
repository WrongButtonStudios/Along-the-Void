using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleAI : MonoBehaviour
{
    //I HATE THIS AMOUNT OF VARIABLES, FEELS LIKE MY FIRST CODE I EVER WROTE IN MY ENTIRE LIFE TF 
    public enum Color
    {
        Red,
        Blue,
        Green,
        Yellow
    }

    [SerializeField]
    private List<EnemyType> _types;
    [SerializeField]
    private Color _enemyColor = Color.Red;
    [SerializeField]
    private List<WeaponsAttached> _weapons = new();
    [SerializeField]
    private EnemyStateHandler.State _curState = EnemyStateHandler.State.Patrol;
    [SerializeField]
    private Transform _playerPos;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _reconizedPlayerRange = 7.5f;
    private float _stoppingDistance = 1;

    [SerializeField]
    private EnemyStatusEffect _statusEffect;
    [SerializeField]
    private List<Transform> _wayPoints = new();

    private Rigidbody2D _rb;
    private sbyte _selectedWeapon;
    private sbyte _selectedPatrolComponent;
    private Scene _scene;

    //Components 
    public List<IHauntingComponent> HauntingComponents { get; private set; } = new();
    public List<IPatrolComponent> PatrolComponents { get; private set; } = new();
    public List<IAttackComponent> AttackComponents { get; private set; } = new();

    public float StoppingDistance { get { return _stoppingDistance; } }

    public EnemyMovement Movement { get; private set; }
    public Vector2 PlayerPos { get; private set; } //obsolete => new direction calc. function cast v3 to v2
    public Color EnemyColor { get { return _enemyColor;  } }
    public float MaxRange { get { return _attackRange; } }
    public EnemyStatusEffect StatusEffect { get { return _statusEffect; } }
    public Scene Scene { get { return _scene;  } }
    public float ReconizedPlayerRange { get { return _reconizedPlayerRange; } }
    public float AttackRange { get { return _attackRange; } }

    //this gets removed later
    private void Start()
    {
        Initialize();
    }

    //I'm not sure if this belongs here. 
    private void Initialize()
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();
        Movement = this.GetComponent<EnemyMovement>(); 
        if(_types[0] == EnemyType.GroundEnemy)
            _rb.gravityScale = PhysicUttillitys.TimeScale;
        EnemyInitializer.AddPatrolAndHauntComponent(this, _wayPoints, _types);
        EnemyInitializer.AddAttackComponent(this, _weapons); 
    }

    //I'm not happy with this. 
    void FixedUpdate()
    {
        if (this.isActiveAndEnabled == false || _statusEffect.Status == EnemyStatusEffect.EnemyStatus.Frozen)
            return;
        PlayerPos = _playerPos.transform.position; 

        if (_types[0] == EnemyType.GroundEnemy)
            _rb.gravityScale = PhysicUttillitys.TimeScale;

        _curState = EnemyStateHandler.GetState(this, _curState);
        ExecuteState();
    }

    private void ExecuteState()
    {
        switch (_curState)
        {
            case EnemyStateHandler.State.Patrol:
                 PatrolComponents[_selectedPatrolComponent].Patrol();
                 break;
            case EnemyStateHandler.State.Hount:
                 HauntingComponents[_selectedPatrolComponent].Haunt(_playerPos.position);
                 break;
            case EnemyStateHandler.State.Attack:
                 AttackComponents[_selectedWeapon].Attack();
                 break;
        }
    }

    public void SetScene(Scene scene)
    {
        _scene = scene; 
    }

    //To-Do: Function is obsolete, remove this.  
    public IAttackComponent GetActiveAttackComponent()
    {
        return AttackComponents[_selectedWeapon]; 
    }

    public void InitEnemyWaypoints(List<Transform> wps)
    {
        foreach (IPatrolComponent p in PatrolComponents)
        {
            p.SetWayPoints(wps); 
        }
    }
}