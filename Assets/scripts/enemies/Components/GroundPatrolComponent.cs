using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPatrolComponent : MonoBehaviour, IPatrolComponent
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

    public void LookAtTarget()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = -1 * GetXDirection();
        _entity.transform.localScale = newScale;
    }

    public void Patrol()
    {
        if (_entity.RB.simulated == false)
        {
            _entity.RB.simulated = true;
        }
        if (_isOnPoint)
            SetUpNewWayPoint();

        float distToWayPoint = (_wayPoints[_curWayPoint] - (Vector2)_entity.transform.position).sqrMagnitude;

        if (distToWayPoint > (_entity.StoppingDistance * _entity.StoppingDistance))
        {
            if (CheckForObstacle() && IsGrounded())
                Jump();
            Movement(_wayPoints[_curWayPoint]); 

        }
        else
            _isOnPoint = true;
    }

    private bool CheckForObstacle()
    {
        Vector3 rayOrigin = _entity.transform.position - new Vector3(0, 0.5f, 0);
        float direction = GetXDirection();
        float rayDistance = 1f;

        RaycastHit2D hitLow = Physics2D.Raycast(rayOrigin, Vector2.right * direction, rayDistance, ~_entity.IgnoreLayer);
        RaycastHit2D hitMid = Physics2D.Raycast(_entity.transform.position, Vector2.right * direction, rayDistance, ~_entity.IgnoreLayer);

        if (hitLow || hitMid)
            return true;

        return false;
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
        Vector2 moveForce = moveDir * _entity.Speed; 
        _entity.RB.AddForce(moveForce * Time.fixedDeltaTime, ForceMode2D.Impulse); 
    }

    private void Jump()
    {
        _entity.RB.AddForce(Vector2.up * _entity.JumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
    private bool IsGrounded()
    {
        if (Physics2D.Raycast(_entity.transform.position, -Vector2.up, 0.75f, ~_entity.IgnoreLayer))
        {
            return true;
        }
        return false;
    }

    public void Init(SimpleAI entity)
    {
        _entity = entity;
        foreach (Transform wp in _entity.WayPoints.transform)
        {
            _wayPoints.Add(wp.position);
        }
    }

    public bool ReachedDestination()
    {
        return _isOnPoint; 
    }
}
