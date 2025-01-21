using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BehaviourStateHandler : MonoBehaviour
{


    [SerializeField] private EnemyStateHandler.State _curState = EnemyStateHandler.State.Patrol;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _aggroRange = 7.5f;
    [SerializeField] private Enemy _statusEffect;
    [SerializeField] private List<Transform> _wayPoints = new();

    private float _stoppingDistance = 1; // warum hardgecoded? hängt vom gegner ab
    private Rigidbody2D _rb; // ja
    private sbyte _selectedWeapon; //nein. falscher datentyp?
    private sbyte _selectedPatrolComponent;
    private Scene _scene; // nicht SOLID, aber ist bekannt (single responsibility)

    //Components kommen als seralized field rein
    public List<IHauntingComponent> HauntingComponents { get; private set; } = new(); // was genau macht das? name? 
    public List<IPatrolComponent> PatrolComponents { get; private set; } = new(); // was genau macht das? name? 
    public List<IAttackComponent> AttackComponents { get; private set; } = new(); // was genau macht das? name? 

    public float StoppingDistance { get { return _stoppingDistance; } }
    public EnemyMovement Movement { get; private set; }
    public Vector2 PlayerPos { get; private set; } //obsolete => new direction calc. function cast v3 to v2
    public Color EnemyColor { get { return _enemyColor;  } }
    public float MaxRange { get { return _attackRange; } }
    public Enemy StatusEffect { get { return _statusEffect; } }
    public Scene Scene { get { return _scene;  } }
    public float ReconizedPlayerRange { get { return _aggroRange; } }
    public float AttackRange { get { return _attackRange; } }

    //this gets removed later
    private void Start()
    {
        Initialize();
    }

    //I'm not sure if this belongs here. 
    private void Initialize()
    {
        if (_rb == null) // keine einzeilen anweisungen
            _rb = this.GetComponent<Rigidbody2D>();
        Movement = this.GetComponent<EnemyMovement>(); // ne, weil prefab
        if(_types[0] == EnemyType.GroundEnemy) // keine einzeilen anweisungen
            _rb.gravityScale = PhysicUttillitys.TimeScale; // mutmaßlich nicht solid. extra component
        EnemyInitializer.AddPatrolAndHauntComponent(this, _wayPoints, _types); // ne, weil prefab
        EnemyInitializer.AddAttackComponent(this, _weapons); // ne, weil prefab
    }

    //I'm not happy with this. 
    void FixedUpdate()
    {
        // wird doch gar nicht gecalled dann? (erste fall)
        // einzeilenanweisung
        // warum ist frozen kein state?
        // manche sachen/states könnten im frozen zustand stattfinden
        if (this.isActiveAndEnabled == false || _statusEffect.Status == Enemy.Status.Frozen)
            return;

        _curState = BehaviourStateMachine.UpdateState(this, _curState); // 
        ExecuteState(); // ist doch nicht physic, warum im fixed update
    }

    //klassen trennen
    private void ExecuteState()
    {
        switch (_curState)
        {
            case EnemyStateHandler.State.Patrol:
                 PatrolComponents[_selectedPatrolComponent].Patrol();
                 break;
            case EnemyStateHandler.State.Hount: // spelling
                 HauntingComponents[_selectedPatrolComponent].Haunt();
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

    //To-Do: Function is obsolete, remove this
    // ja denke auch
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