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
    private Color _enemyColor = Color.Red;
    [SerializeField]
    private List<WeaponsAttached> _weapons;
    [SerializeField]
    private State _curState = State.Patrol;
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
    
    private Rigidbody2D _rb;
    private int _selectedWeapon;
    private int _selectedPatrolComponent;
    private Scene _scene; 

    //Components 
    private List<IHauntingComponent> _hauntingComponents = new List<IHauntingComponent>();
    private List<IPatrolComponent> _patrolComponents= new List<IPatrolComponent>();
    private List<IAttackComponent> _attackComponents = new List<IAttackComponent>();

    //Getter
    public Rigidbody2D RB { get { return _rb; } }
    public float Speed { get { return _speed; } }
    public float StoppingDistance { get { return _stoppingDistance; } }
    public LayerMask IgnoreLayer { get { return _ignoreLayer; } }
    public GameObject AttackVFX { get { return _attackEffect;  } }
    public Vector2 PlayerPos { get { return (Vector2)_playerPos.position; } }
    public Color EnemyColor { get { return _enemyColor;  } }
    public float MaxRange { get { return _attackRange; } }
    public EnemyStatusEffect StatusEffect { get { return _statusEffect; } }
    public Scene Scene { get { return _scene;  } }
    public float TimeScale { get; private set; }



    public float JumpForce { get { return _jumpForce; } }

    private bool _isInitialized = false; 

    private void OnEnable()
    {
        if(_rb==null)
            _rb = this.GetComponent<Rigidbody2D>();
        if(_playerPos == null)
            _playerPos = GameObject.FindObjectOfType<characterController>().transform;
        if(_isInitialized == false)
            Initialize();
    }

    private void Initialize()
    {
        TimeScale = 1; 
        _isInitialized = true; 
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
        if (this.isActiveAndEnabled == false)
            return; 
        SelectNewWeapon();
        SelectMovementComponent();
        ChangedState();
        if (_statusEffect.Status != EnemyStatusEffect.EnemyStatus.Frozen)
        {
            switch (_curState)
            {
                case State.Patrol:
                    _patrolComponents[_selectedPatrolComponent].Patrol();
                    break;
                case State.Hount:
                    _hauntingComponents[_selectedPatrolComponent].Haunt(_playerPos.position);
                    break;
                case State.Attack:
                    _attackComponents[_selectedWeapon].Attack();
                    break;
            }
        }
        else
        {
            _rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

    }

    public void SetTimeScale(float val)
    {
        if (val > 1)
        {
            Debug.LogWarning("Time scale is not allowed to be bigger than 1! It is automaticly set to 1.");
            TimeScale = 1;
            return;
        }
        TimeScale = val;
    }
    private bool ChangedState()
    {
        float distanceSqr = _hauntingComponents[0].GetDistanceToTargetSqr(_playerPos.position, transform.position);
        if (distanceSqr < (_reconizedPlayerRange*_reconizedPlayerRange) && distanceSqr > (_attackRange*_attackRange) && _curState != State.Hount)
        {
            if(_curState == State.Attack)
                _attackComponents[_selectedWeapon].Exit();
            _curState = State.Hount;
            return true;
        }

        if (_hauntingComponents[0].GetDistanceToTargetSqr(_playerPos.position, transform.position) < (_attackRange * _attackRange) && _curState != State.Attack)
        {
            _curState = State.Attack;
            return true;
        }

        if (_hauntingComponents[0].GetDistanceToTargetSqr(_playerPos.position, transform.position) > (_reconizedPlayerRange*_reconizedPlayerRange) && _curState != State.Patrol)
        {
            if (_curState == State.Attack)
                _attackComponents[_selectedWeapon].Exit();
            _curState = State.Patrol;
            return true;
        }
        return false;
    }

    private void SelectNewWeapon()
    {
        if (_attackComponents[_selectedWeapon].FinnishedAttack())
        {
            int oldWeapon = _selectedWeapon; 
            _selectedWeapon = Random.Range(0, _attackComponents.Count - 1);
            _attackComponents[oldWeapon].ResetAttackStatus(); 
        }
    }

    private void SelectMovementComponent()
    {
        if (_patrolComponents[_selectedPatrolComponent].ReachedDestination())
        {
            _selectedPatrolComponent = Random.Range(0, _patrolComponents.Count - 1);
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