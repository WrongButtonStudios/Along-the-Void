using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPatrolComponent : MonoBehaviour, IPatrolComponent
{
    private SimpleAI _entity;
    private int _curWayPoint;
    private bool _isOnPoint;
    private List<Vector2> _wayPoints = new List<Vector2>(); 

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
        Vector2 moveDirection = (target - (Vector2)_entity.transform.position).normalized;
        Vector2 movementVelocity = moveDirection * _entity.Speed * (Time.fixedDeltaTime * _entity.TimeScale);
        _entity.RB.velocity += movementVelocity; 
    }

    public void Patrol()
    {
        if (_isOnPoint)
            SetUpNewWayPoint();

        float distToWayPoint = (_wayPoints[_curWayPoint] - (Vector2)_entity.transform.position).sqrMagnitude;

        if (distToWayPoint > (_entity.StoppingDistance * _entity.StoppingDistance))
        {
            Movement(_wayPoints[_curWayPoint]);
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

    public void SetWayPoints(List<Transform> wps)
    {
        _wayPoints.Clear();
        foreach (Transform wp in wps)
        {
            _wayPoints.Add(wp.position);
        }
    }
}
