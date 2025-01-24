using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHight = 2.5f;
    [SerializeField] private Rigidbody2D _rb;

    private float _maxJumpHight; 

    public Rigidbody2D RB { get { return _rb; } }
    public float Speed { get { return _speed;  } }

    public void ZeroVelocity()
    {
        _rb.velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        _rb.velocity = PhysicUttillitys.ClampVelocity(_rb.velocity, _speed);
    }

    public bool Jump()
    {
        if (transform.position.y <= _maxJumpHight)
        {
            Move(Vector2.up, _jumpForce); 
            return true; 
        }
        return false; 
    }

    public void Move(Vector2 dir, bool useDebugLogs = false)
    {
        _rb.velocity += dir.normalized * (_speed * (Time.fixedDeltaTime * PhysicUttillitys.TimeScale));
        if (useDebugLogs)
        {
            Debug.Log("velocity change value: " + dir.normalized * (_speed * (Time.fixedDeltaTime * PhysicUttillitys.TimeScale)));
            Debug.Log("Direction: " + dir.normalized + " Delta Speed: " + (_speed * (Time.fixedDeltaTime * PhysicUttillitys.TimeScale)));
        }
    }

    public void Move(Vector2 dir, float speed)
    {
        _rb.velocity += dir.normalized * (speed * (Time.fixedDeltaTime * PhysicUttillitys.TimeScale));
    }

    public void SetGravitiyScale(float gravityScale)
    {
        ZeroVelocity();
        RB.gravityScale = gravityScale;
    }

    public Vector2 CalculateDirection(Vector2 a, Vector2 b)
    {
        return b - a; 
    }

    public Vector2 CalculateDirectionX(Vector2 a, Vector2 b)
    {
        a.y = 0;
        b.y = 0; 
        return b - a;
    }

    public void CalculateMaxJumpHight() 
    {
        _maxJumpHight = transform.position.y + _jumpHight;
    }

    public void CalculateMaxJumpHight(float jumpHight)
    {
        _maxJumpHight = transform.position.y + jumpHight;
    }

    public void ZeroVelocityX()
    {
        Vector2 newVel = _rb.velocity;
        newVel.x = 0; 
        _rb.velocity = newVel;
    }
}