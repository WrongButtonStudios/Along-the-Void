using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlyingPatrolComponent : PatrolComponent
{
    [SerializeField] private BehaviourStateHandler _entity;
    [SerializeField] private EnemyMovement _movement;
    [SerializeField] private List<Transform> _wayPoints = new List<Transform>();

    private int _curWayPoint;
    private bool _isOnPoint;

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
        Vector2 dir = _wayPoints[_curWayPoint].position - _entity.transform.position;
        return Mathf.Sign(dir.x);
    }

    public override void LookAtTarget()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = -1 * GetXDirection();
        _entity.transform.localScale = newScale;
    }

    public override void Movement(Vector2 target)
    {
        LookAtTarget();
        _entity.Movement.Move(_movement.CalculateDirection((Vector2)_entity.transform.position, target, target.y));
    }

    public override void Patrol()
    {
        if (_isOnPoint)
            SetUpNewWayPoint();

        float distToWayPoint = (_wayPoints[_curWayPoint].position - transform.position).sqrMagnitude;

        if (distToWayPoint > (_entity.StoppingDistance * _entity.StoppingDistance))
            Movement(_wayPoints[_curWayPoint].position);
        else
            _isOnPoint = true;
    }

    public override bool ReachedDestination()
    {
        return _isOnPoint; 
    }

    public override void SetUpNewWayPoint()
    {
        _curWayPoint = GetNextWayPoint();
        _isOnPoint = false;
    }
}