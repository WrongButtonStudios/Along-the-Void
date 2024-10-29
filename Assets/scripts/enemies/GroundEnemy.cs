using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private LayerMask _ignoreLayer; 

    private SimpleAI _enemy; 
    private Vector3 offset;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemy = GetComponent<SimpleAI>();
    }

    private void Start()
    {
        offset = new Vector3(0, (this.transform.localScale.y / 4), 0);
    }
    public void Attack()
    {
        Debug.Log("Hier sollte der Spieler schaden nehmen"); 
    }

    public void Haunt()
    {
        Movement(_enemy.PlayerPos.position);
    }

    public void Movement(Vector3 targetPos)
    {
        _enemy.LookAtTarget(); 
        if (CheckForObstacle() && IsGrounded())
            Jump();
        _rb.AddForce(CalculateMovementForce(targetPos) * _speed * Time.fixedDeltaTime, ForceMode2D.Impulse);

    }

    void Jump()
    {
        if (IsGrounded())
            _rb.AddForce(Vector2.up * _jumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    private bool CheckForObstacle()
    {
        Vector3 rayOrigin = transform.position - offset;
        float direction = _enemy.GetNewXDirection();
        float rayDistance = 1f;

        RaycastHit2D hitLow = Physics2D.Raycast(rayOrigin, Vector2.right * direction, rayDistance, ~_ignoreLayer);
        RaycastHit2D hitMid = Physics2D.Raycast(rayOrigin + new Vector3(0, 0.5f, 0), Vector2.right * direction, rayDistance, ~_ignoreLayer);

        if (hitLow || hitMid)
            return true;

        return false;
    }


    private Vector2 CalculateMovementForce(Vector3 targetPos)
    {
        Vector2 dir = (targetPos - transform.position).normalized;
        dir.y = 0; 
        Vector2 force = dir * _speed;
        return force;
    }

    private bool IsGrounded()
    {
        if (Physics2D.Raycast(transform.position, -Vector2.up, 0.75f, ~_ignoreLayer))
        {
            return true;
        }
        return false;
    }
}
