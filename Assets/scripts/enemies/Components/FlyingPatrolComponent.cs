using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPatrolComponent : MonoBehaviour, IPatrolComponent
{
    private SimpleAI _entity;
    private int _curWayPoint;
    private bool _isOnPoint;

    public int GetNextWayPoint()
    {
        if (_curWayPoint < _entity.WayPoints.Count - 1)
            _curWayPoint++;
        else
            _curWayPoint = 0;

        return _curWayPoint;
    }

    public float GetXDirection()
    {
        Vector2 dir = _entity.WayPoints[_curWayPoint].position - _entity.transform.position;
        return Mathf.Sign(dir.x);
    }

    public void Init(SimpleAI entity)
    {
        _entity = entity; 
    }

    public void LookAtTarget()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = -1 * GetXDirection();
        _entity.transform.localScale = newScale;
    }

    public void Movement(Vector2 target)
    {
        LookAtTarget();
        Vector2 moveDir = (target - (Vector2)_entity.transform.position).normalized;
        Vector2 moveForce = moveDir * _entity.Speed;
        _entity.RB.AddForce(moveForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    public void Patrol()
    {
        if (_isOnPoint)
            SetUpNewWayPoint();

        float distToWayPoint = (_entity.WayPoints[_curWayPoint].position - _entity.transform.position).sqrMagnitude;

        if (distToWayPoint > (_entity.StoppingDistance * _entity.StoppingDistance))
        {
            Movement(_entity.WayPoints[_curWayPoint].position);
        }
        else
            _isOnPoint = true;
    }

    public bool ReachedDestination()
    {
        return _isOnPoint; 
    }

    public void SetUpNewWayPoint()
    {
        _curWayPoint = GetNextWayPoint();
        _isOnPoint = false;
    }
}
