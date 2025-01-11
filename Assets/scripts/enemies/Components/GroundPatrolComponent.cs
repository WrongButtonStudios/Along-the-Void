using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPatrolComponent : MonoBehaviour, IPatrolComponent
{
    private SimpleAI _entity;
    private int _curWayPoint;
    private bool _isOnPoint;
    private List<Vector2> _wayPoints = new List<Vector2>();
    private float _maxJumpHight;
    private float _jumpHight = 2; 
    private bool _doJump = false;

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
        Jump(); 
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

        Vector2 temp = _wayPoints[_curWayPoint];
        temp.y = transform.position.y; 
        float distToWayPoint = (temp - (Vector2)_entity.transform.position).sqrMagnitude;

        if (distToWayPoint > (_entity.StoppingDistance * _entity.StoppingDistance))
        {
            if (CheckForObstacle() && IsGrounded())
            {
                _maxJumpHight = transform.position.y + _jumpHight; 
                _doJump = true; 
            }
                
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
        moveDir.y = 0f; 
        Vector2 moveForce = moveDir.normalized * _entity.Speed * (Time.fixedDeltaTime * _entity.TimeScale);
        //((Vector2 newDir = (Vector2)transform.position + (moveForce * (Time.fixedDeltaTime * _entity.TimeScale)); 
        //To-DO change Addforce to MovePosition 
        _entity.RB.velocity += moveDir;
    }

    private void Jump()
    {
        if (!_doJump) //early out if not jumping 
            return;

        if (_entity.transform.position.y < _maxJumpHight)
        {
            _entity.RB.MovePosition( (Vector2)transform.position + (Vector2.up * _entity.JumpForce * (Time.fixedDeltaTime * _entity.TimeScale)));
            Debug.Log(Vector2.up * _entity.JumpForce * (Time.fixedDeltaTime * _entity.TimeScale));
        }
        else
            _doJump = false;

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
