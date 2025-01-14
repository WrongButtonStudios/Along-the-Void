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
            if (CheckForObstacle() && IsGrounded() && !_doJump)
            {
                Debug.Log("I should be jumping..."); 
                _maxJumpHight = transform.position.y + _entity.JumpHight;
                _entity.RB.gravityScale = 0;
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

        RaycastHit2D hitLow = Physics2D.Raycast(rayOrigin, transform.right * direction, rayDistance, ~_entity.IgnoreLayer);
        RaycastHit2D hitMid = Physics2D.Raycast(_entity.transform.position, transform.right * direction, rayDistance, ~_entity.IgnoreLayer);
        Debug.DrawLine(_entity.transform.position, _entity.transform.position + (transform.right * direction), Color.black, Time.fixedDeltaTime); 
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
        _entity.RB.velocity += moveForce;
    }

    private void Jump()
    {
        if (!_doJump) //early out if not jumping 
            return;

        if (_entity.transform.position.y < _maxJumpHight)
        {
            Debug.Log("jumping...");
            Vector2 jumpVel = Vector2.up * _entity.JumpForce - Physics2D.gravity * (Time.fixedDeltaTime * _entity.TimeScale);
            _entity.RB.velocity += jumpVel;
        }
        else
        {
            _doJump = false;
            _entity.RB.velocity = Vector2.zero; 
            _entity.RB.gravityScale = 1; 
        }
    }

    private bool IsGrounded()
    {
        Debug.DrawLine(_entity.transform.position, (Vector2)_entity.transform.position + (-Vector2.up * 1.25f) , Color.gray, Time.fixedDeltaTime); 
        if (Physics2D.Raycast(_entity.transform.position, -Vector2.up, 1.25f, ~_entity.IgnoreLayer))
        {
            return true;
        }
        Debug.Log("not grounded..."); 
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
