using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPatrolComponent : MonoBehaviour, IPatrolComponent
{
    private BehaviourStateHandler _entity;
    private int _curWayPoint;
    private bool _isOnPoint;
    private List<Vector2> _wayPoints = new List<Vector2>();
    private float _maxJumpHight; 
    private bool _doJump = false;
    private EnemyCollisionHandler _collsionHandler;
    private EnemyMovement _movement; 

    private void Start()
    {
        _collsionHandler = this.GetComponent<EnemyCollisionHandler>();
    }

    public int GetNextWayPoint()
    {
        if (_curWayPoint < _wayPoints.Count - 1)
            _curWayPoint++;
        else
            _curWayPoint = 0;

        return _curWayPoint; 
    }

    public float GetXDirection()
    {
        Vector2 dir = _wayPoints[_curWayPoint] - (Vector2)_entity.transform.position;
        return Mathf.Sign(dir.x);
    }

    private void FixedUpdate()
    {
        bool isJumping = false;
        if (_doJump)
           isJumping = _movement.Jump();
        if (!isJumping && _doJump)
        {
            _doJump = false;
            _entity.Movement.RB.velocity = Vector2.zero;
            _entity.Movement.RB.gravityScale = 1;
        }
    }

    public void LookAtTarget()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = -1 * GetXDirection();
        _entity.transform.localScale = newScale;
    }

    //To-Do kann wahrscheinlich vereinfacht werden
    public void Patrol()
    {
        if (_entity.Movement.RB.simulated == false)
        {
            _entity.Movement.RB.simulated = true;
        }
        if (_isOnPoint)
            SetUpNewWayPoint();

        Vector2 temp = _wayPoints[_curWayPoint];
        temp.y = transform.position.y; 
        float distToWayPoint = (temp - (Vector2)_entity.transform.position).sqrMagnitude;

        if (distToWayPoint > (_entity.StoppingDistance * _entity.StoppingDistance))
        {
            if (_collsionHandler.CheckForObstacle(GetXDirection()) && _collsionHandler.IsGrounded() && !_doJump)
            {
                _entity.Movement.RB.gravityScale = 0;
                _doJump = true; 
            }
            Movement(_wayPoints[_curWayPoint]);
        }
        else
            _isOnPoint = true;
    }


    public void SetUpNewWayPoint()
    {
        _curWayPoint = GetNextWayPoint();
        _isOnPoint = false;
    }

    public void Movement(Vector2 target) 
    {
        LookAtTarget();
        Vector2 moveDir = (target - (Vector2)_entity.transform.position).normalized;
        _entity.Movement.Move(moveDir); 
    }

    public void Init(BehaviourStateHandler entity)
    {
        _entity = entity;
    }

    public bool ReachedDestination()
    {
        return _isOnPoint; 
    }

    public void SetWayPoints(List<Transform> wps)
    {
        _wayPoints.Clear();
        foreach (Transform wp in wps)
        {
            _wayPoints.Add(wp.position);
        }
    }
}
