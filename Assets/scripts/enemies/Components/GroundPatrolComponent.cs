using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundPatrolComponent : PatrolComponent
{
    [SerializeField] private BehaviourStateHandler _entity;
    [SerializeField] private EnemyCollisionHandler _collsionHandler;
    [SerializeField] private EnemyMovement _movement;
    [SerializeField] private List<Transform> _wayPoints = new List<Transform>();

    private int _curWayPoint;
    private bool _isOnPoint;
    private bool _doJump = false;

    public override int GetNextWayPoint()
    {
        if (_curWayPoint < _wayPoints.Count - 1)
            _curWayPoint++;
        else
            _curWayPoint = 0;

        return _curWayPoint; 
    }

    public override float GetXDirection()
    {
        Vector2 dir = _wayPoints[_curWayPoint].position - transform.position;
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
            _movement.ZeroVelocity(); 
            _movement.SetGravitiyScale(1); 
        }
    }

    public override void LookAtTarget()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = -1 * GetXDirection();
        _entity.transform.localScale = newScale;
    }

    //To-Do kann wahrscheinlich vereinfacht werden
    public override void Patrol()
    {
        if (_isOnPoint)
        {
            SetUpNewWayPoint(); 
        }
        Vector2 temp = _wayPoints[_curWayPoint].position;
        temp.y = transform.position.y; 
        float distToWayPoint = (temp - (Vector2)_entity.transform.position).sqrMagnitude;

        if (distToWayPoint > (_entity.StoppingDistance * _entity.StoppingDistance))
        {
            Movement(_wayPoints[_curWayPoint].position);
        }
        else
        {
            _isOnPoint = true; 
        }
    }

    public override void SetUpNewWayPoint()
    {
        _curWayPoint = GetNextWayPoint();
        _isOnPoint = false;
    }

    public override void Movement(Vector2 target) 
    {
        LookAtTarget();
        _movement.Move(target - (Vector2)_entity.transform.position);
    }

    public override bool ReachedDestination()
    {
        return _isOnPoint; 
    }
}