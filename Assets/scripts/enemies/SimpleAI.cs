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
    private bool _isOnPoint;
    private int _curWayPoint = -1;
    private Rigidbody2D _rb;


    // Start is called before the first frame update
    void Awake()
    {
        _rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ExecuteState();     
    }

    private void ExecuteState() 
    {
        switch(_curState)
        {
            case State.Patrol:
                Patrol(); 
                break;
            case State.Hount:
                Hount(); 
                break;
            case State.Attack:
                Attack(); 
                break;

        }
    }

    private void TurnAround() 
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale; 
    }

    void Jump() 
    {
        _rb.AddForce(Vector2.up * _jumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse); 
    }
    private void Patrol()
    {
        if (_isOnPoint || _curWayPoint == -1) 
        {
            _curWayPoint = GetRandomWaypoint();
            TurnAround(); 
            _isOnPoint = false; 
        }
        float distToWayPoint = (_wayPoints[_curWayPoint].position - transform.position).sqrMagnitude; 
        if (distToWayPoint > (0.2f * 0.2f)) 
        {
            if (CheckForObstacle() && IsGrounded())
                Jump(); 

            _rb.AddForce(CalculateMovementForce() * Time.fixedDeltaTime, ForceMode2D.Impulse);
            ClampVelocity(); 
        } else 
        {
            _isOnPoint = true; 
        }
                       
    }

    private bool CheckForObstacle()
    {
        if (Physics2D.Raycast(transform.position, transform.right * -1, 2f, ~_ignoreLayer))
            return true;
        else
            return false; 
    }
    private void ClampVelocity() 
    {
        if(_rb.velocity.magnitude > _speed*3) 
        {
            _rb.velocity = _rb.velocity.normalized * _speed * Time.fixedDeltaTime; 
        }
    }
    private Vector2 CalculateMovementForce() 
    {
        Vector2 dir = (_wayPoints[_curWayPoint].position - transform.position).normalized;
        Vector2 force = dir * _speed;
        return force; 
    }

    private void Hount()
    {
        throw new System.NotImplementedException();
    }

    private void Attack()
    {
        throw new System.NotImplementedException();
    }
    private int GetRandomWaypoint() 
    {
        int newWP = _curWayPoint;

        newWP = Random.Range(0, _wayPoints.Count - 1);
        if (newWP == _curWayPoint && newWP < _wayPoints.Count - 1)
            newWP++; 
        else 
            newWP = 0;

        return newWP; 
    }

    private bool IsGrounded() 
    {
        if (Physics2D.Raycast(transform.position, Vector2.up * -1, 1f, ~_ignoreLayer))
        {
            Debug.Log("Grounded"); 
            return true;
        }
        
        return false; 
    }
}
