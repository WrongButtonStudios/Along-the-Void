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

    private State _curState;
    [SerializeField]
    private List<Transform> _wayPoints = new List<Transform>();
    [SerializeField]
    private Transform _playerPos;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _AttackRange;
    private bool _isOnPoint;
    private int _curWayPoint = -1; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void Patrol()
    {
        if (_isOnPoint || _curWayPoint == -1) 
        {
            _curWayPoint = GetRandomWaypoint();
            _isOnPoint = false; 
        }
        float distToWayPoint = (_wayPoints[_curWayPoint].position - transform.position).sqrMagnitude; 
        if (distToWayPoint > (0.1f * 0.1f)) 
        {
            Vector3 dir = (_wayPoints[_curWayPoint].position - transform.position).normalized;
            transform.position += dir * _speed * Time.deltaTime; 
        } else 
        {
            _isOnPoint = true; 
        }
                       
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
        return Random.Range(0, _wayPoints.Count - 1); 
    }
}
