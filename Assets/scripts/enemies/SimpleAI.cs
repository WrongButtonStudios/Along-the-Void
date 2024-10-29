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
    [SerializeField]
    private State _curState = State.Patrol;
    [SerializeField]
    private List<Transform> _wayPoints = new List<Transform>();
    [SerializeField]
    private Transform _playerPos;
    public Transform PlayerPos { get { return _playerPos;  } }
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
    private bool _isOnPoint;
    private int _curWayPoint = 0;
    private Rigidbody2D _rb;
    private Vector3 offset;

    void Awake()
    {
        _rb = this.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        offset = new Vector3(0, (this.transform.localScale.y / 4), 0);
    }


    void Update()
    {
        ExecuteState();     
    }

    private void ExecuteState() 
    {
        bool stateChanged; 
        switch (_curState)
        {
            case State.Patrol:
                stateChanged = ChangedState();
                if (stateChanged)
                    return;
                break;
            case State.Hount:
                stateChanged = ChangedState();
                if (stateChanged)
                    return;
                break;
            case State.Attack:
                stateChanged = ChangedState();
                if (stateChanged)
                    return;
                break;

        }
    }

    
    private void Patrol()
    {
        if (_isOnPoint)
            SetUpNewWayPoint(); 

        float distToWayPoint = (_wayPoints[_curWayPoint].position - transform.position).sqrMagnitude;

        if (distToWayPoint > (_stoppingDistance * _stoppingDistance)) 
        {
            //hier sollte Gegner sich bewegen
        }
        else
            _isOnPoint = true;
    }

    public float GetNewXDirection() 
    {
        Vector2 dir = _wayPoints[_curWayPoint].position - transform.position;
        return Mathf.Sign(dir.x); 
    }

    void SetUpNewWayPoint() 
    {
        _curWayPoint = GetNextWayPoint(); 
        _isOnPoint = false;
    }

    public void LookAtTarget()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = -1 * GetNewXDirection();
        transform.localScale = newScale;
    }

    private int GetNextWayPoint() 
    {
        if (_curWayPoint < _wayPoints.Count - 1)
            _curWayPoint++; 
        else
            _curWayPoint = 0;

        return _curWayPoint; 
    }

    private bool ChangedState() 
    {
        float distance = Vector2.Distance(_playerPos.position, transform.position); 
        if (distance < _reconizedPlayerRange && distance > _attackRange && _curState != State.Hount) 
        {
            _curState = State.Hount;
            return true; 
        } 
         
        if (Vector2.Distance(_playerPos.position, transform.position) < _attackRange && _curState != State.Attack)
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