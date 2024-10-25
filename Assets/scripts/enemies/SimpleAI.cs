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

    private State _curState = State.Patrol;
    [SerializeField]
    private List<Transform> _wayPoints = new List<Transform>();
    [SerializeField]
    private Transform _playerPos;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _AttackRange;
    [SerializeField]
    private LayerMask _ignoreLayer;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _stoppingDistance = 1f; 
    private bool _isOnPoint;
    private int _curWayPoint = 0;
    private Rigidbody2D _rb;
    private Vector3 offset;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = this.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        offset = new Vector3(0, (this.transform.localScale.y / 4), 0);
    }
    // Update is called once per frame
    void Update()
    {
        ExecuteState();     
    }

    private void ExecuteState() 
    {
        bool stateChanged = false; 
        switch (_curState)
        {
            case State.Patrol:
                stateChanged = ChangedState();
                if (stateChanged)
                    return;
                Patrol(); 
                break;
            case State.Hount:
                stateChanged = ChangedState();
                if (stateChanged)
                    return;
                Hount(); 
                Debug.Log("sui lan, willst du stress amk?!"); 
                break;
            case State.Attack:
                stateChanged = ChangedState();
                if (stateChanged)
                    return;
                Debug.Log("sui lan, isch mach disch Messer amk!");
                Attack(); 
                break;

        }
    }

    private void LookAtTarget() 
    {
        Vector3 newScale = transform.localScale;
        newScale.x = -1 * GetNewXDirection(); 
        transform.localScale = newScale; 
    }

    void Jump() 
    {
        if(IsGrounded())
            _rb.AddForce(Vector2.up * _jumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse); 
    }
    private void Patrol()
    {
        if (_isOnPoint)
            SetUpNewWayPoint(); 

        float distToWayPoint = (_wayPoints[_curWayPoint].position - transform.position).sqrMagnitude; 
        
        if (distToWayPoint > (_stoppingDistance*_stoppingDistance)) 
            HandleMovement(); 
        else 
            _isOnPoint = true;
    }

    private float GetNewXDirection() 
    {
        Vector2 dir = _wayPoints[_curWayPoint].position - transform.position;
        return Mathf.Sign(dir.x); 
    }

    void SetUpNewWayPoint() 
    {
        _curWayPoint = GetRandomWaypoint(); 
        _isOnPoint = false;
    }

    void HandleMovement() 
    {
        LookAtTarget();
        if (CheckForObstacle())
            Jump();

        _rb.AddForce(CalculateMovementForce(_wayPoints[_curWayPoint].position) * Time.fixedDeltaTime, ForceMode2D.Impulse);

        //ClampVelocity();
    }
    private bool CheckForObstacle()
    {
        Vector3 rayOrigin = transform.position - offset;
        float direction = GetNewXDirection();
        float rayDistance = 1f;

        RaycastHit2D hitLow = Physics2D.Raycast(rayOrigin, Vector2.right * direction, rayDistance, ~_ignoreLayer);
        RaycastHit2D hitMid = Physics2D.Raycast(rayOrigin + new Vector3(0, 0.5f, 0), Vector2.right * direction, rayDistance, ~_ignoreLayer);

        if (hitLow || hitMid)
            return true;

        return false;
    }

    private void ClampVelocity() 
    {
        if(_rb.velocity.x > _speed) 
        {
            Vector3 speed = _rb.velocity.normalized;
            speed.x *= _speed * Time.fixedDeltaTime;
            speed.y = _rb.velocity.y; //ignoring y achxis for jumping 
            _rb.velocity = speed; 
        }
    }
    private Vector2 CalculateMovementForce(Vector3 targetPos) 
    {
        Vector2 dir = (targetPos - transform.position).normalized;
        dir.y = 0; 
        Vector2 force = dir * _speed;
        return force; 
    }

    private void Hount()
    {
        _rb.AddForce(CalculateMovementForce(_playerPos.position) * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    private void Attack()
    {
        Debug.Log("Erfolgreich schaden hinzugefpügt zum Spieler. Also theoretisch metisch"); 
    }
    private int GetRandomWaypoint() 
    {
        int newWP = Random.Range(0, _wayPoints.Count - 1);
        if (newWP == _curWayPoint && newWP < _wayPoints.Count - 1)
            newWP++; 
        else 
            newWP = 0;

        return newWP; 
    }

    private bool ChangedState() 
    {
        if(Vector2.Distance(_playerPos.position, transform.position) < 5f && _curState != State.Hount) 
        {
            _curState = State.Hount;
            return true; 
        } 
        
        if (Vector2.Distance(_playerPos.position, transform.position) < 1f && _curState != State.Attack)
        {
            _curState = State.Attack;
            return true;
        }

        if (Vector2.Distance(_playerPos.position, transform.position) > 5f && _curState != State.Patrol)
        {
            _curState = State.Patrol;
            return true;
        }
        return false;
    }
    private bool IsGrounded() 
    {
        if (Physics2D.Raycast(transform.position, Vector2.up * -1, 0.75f, ~_ignoreLayer))
        {
            Debug.Log("Grounded"); 
            return true;
        }
        Debug.Log("Ich bin Fly wie ein Flugzeug"); 
        return false; 
    }
}