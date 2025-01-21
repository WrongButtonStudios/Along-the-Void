using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _maxJumpHight = 2.5f;
    private Rigidbody2D _rb;

    public Rigidbody2D RB { get { return _rb; } }
    public float Speed { get { return _speed;  } }

    public void ZeroVelocity()
    {
        _rb.velocity = Vector2.zero;
    }
    private void Awake()
    {
        _rb = this.GetComponent<Rigidbody2D>(); 
    }

    private void FixedUpdate()
    {
        _rb.velocity = PhysicUttillitys.ClampVelocity(_rb.velocity, _speed);
    }

    public bool Jump()
    {
        if (transform.position.y < _maxJumpHight)
        {
            Move(Vector2.up, _jumpForce); 
            return true; 
        }
        return false; 
    }

    public void Move(Vector2 dir)
    {
        _rb.velocity += dir.normalized * (_speed * (Time.fixedDeltaTime * PhysicUttillitys.TimeScale)); 
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
    /// <summary>
    /// This functions only calculates the direction on the X Axis. Y will be set to 0. 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public Vector2 CalculateDirectionX(Vector2 a, Vector2 b)
    {
        a.y = 0;
        b.y = 0; 
        return b - a;
    }
}