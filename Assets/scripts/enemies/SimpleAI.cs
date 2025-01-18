using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleAI : MonoBehaviour
{

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
    private List<WeaponsAttached> _weapons;
    [SerializeField]
    private EnemyStateHandler.State _curState = EnemyStateHandler.State.Patrol;
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
    float _stoppingDistance = 1;
    //Placeholder VFX stuff
    [SerializeField]
    private GameObject _attackEffect;
    [SerializeField]
    private EnemyStatusEffect _statusEffect;
    [SerializeField]
    private List<Transform> _wayPoints = new();
    [SerializeField]
    private float _jumpHight = 1.5f; 
    
    private Rigidbody2D _rb;
    private sbyte _selectedWeapon;
    private sbyte _selectedPatrolComponent;
    private bool _isInitialized = false;
    private Scene _scene; 

    //Components 
    private List<IHauntingComponent> _hauntingComponents = new List<IHauntingComponent>();
    private List<IPatrolComponent> _patrolComponents= new List<IPatrolComponent>();
    private List<IAttackComponent> _attackComponents = new List<IAttackComponent>();

    //Getter
    public Rigidbody2D RB { get { return _rb; } }
    public float Speed { get { return _speed; } }
    public float JumpHight { get { return _jumpHight; } }
    public float StoppingDistance { get { return _stoppingDistance; } }
    public LayerMask IgnoreLayer { get { return _ignoreLayer; } }
    public GameObject AttackVFX { get { return _attackEffect;  } }
    public Vector2 PlayerPos { get; private set; }
    public Color EnemyColor { get { return _enemyColor;  } }
    public float MaxRange { get { return _attackRange; } }
    public EnemyStatusEffect StatusEffect { get { return _statusEffect; } }
    public Scene Scene { get { return _scene;  } }
    public List<IHauntingComponent> HauntingComponents { get { return _hauntingComponents;  }  }
    public List<IPatrolComponent> PatrolComponents { get { return _patrolComponents; } }
    public List<IAttackComponent> AttackComponents { get { return _attackComponents; } }
    public float ReconizedPlayerRange { get { return _reconizedPlayerRange; } }
    public float AttackRange { get { return _attackRange; } }
    public float JumpForce { get { return _jumpForce; } }
     
    private void Start()
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();
        if (_isInitialized == false)
            Initialize();
    }

    private void Initialize()
    {
        if(_types[0] == EnemyType.GroundEnemy)
            RB.gravityScale = PhysicUttillitys.TimeScale;

        _isInitialized = true; 
        EnemyInitializer.AddPatrolAndHauntComponent(this, _wayPoints, _types);
        EnemyInitializer.AddAttackComponent(this, _weapons); 
    }

    void FixedUpdate()
    {
        PlayerPos = _playerPos.transform.position; 
        ExecuteState();
    }

    private void ExecuteState()
    {
        if (this.isActiveAndEnabled == false)
            return;
        _selectedWeapon = EnemyBehaviourComponentSelecter.SelectAttackComponent(this, _selectedWeapon);
        _selectedPatrolComponent = EnemyBehaviourComponentSelecter.SelectMovementComponent(this, _selectedPatrolComponent);
        _curState = EnemyStateHandler.GetState(this, _curState);
        if (_statusEffect.Status != EnemyStatusEffect.EnemyStatus.Frozen)
        {
            switch (_curState)
            {
                case EnemyStateHandler.State.Patrol:
                    _patrolComponents[_selectedPatrolComponent].Patrol();
                    break;
                case EnemyStateHandler.State.Hount:
                    _hauntingComponents[_selectedPatrolComponent].Haunt(_playerPos.position);
                    break;
                case EnemyStateHandler.State.Attack:
                    _attackComponents[_selectedWeapon].Attack();
                    break;
            }
            if (_types[0] == EnemyType.GroundEnemy)
                RB.gravityScale = PhysicUttillitys.TimeScale;
            RB.velocity = PhysicUttillitys.ClampVelocityEnemy(this);
        }
        else
        {
            _rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void SetScene(Scene scene)
    {
        _scene = scene; 
    }

    public IAttackComponent GetActiveAttackComponent()
    {
        return _attackComponents[_selectedWeapon]; 
    }

    public void InitEnemyWaypoints(List<Transform> wps)
    {
        foreach (IPatrolComponent p in _patrolComponents)
        {
            p.SetWayPoints(wps); 
        }
    }
}