using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHight = 2.5f;
    [SerializeField] private LayerMask _groundLayer; 
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

    public Vector2 CalculateDirection(Vector2 a, Vector2 b, float desiredHeight)
    {
        Vector2 dir;
        dir.x = b.x - a.x;
        dir.y = CalculateYMovement(desiredHeight, a.y);
        return dir; 
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

    public float CalculateYMovement(float desiredHeight, float currentY)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, _groundLayer);

        if (hit.collider != null)
        {
            float groundYPos = hit.point.y;
            float currentHeight = currentY - groundYPos;
            float moveSpeed = 2f;
            float deadZone = 0.1f; // Toleranz für Rundungsfehler

            Debug.Log($"Current Height: {currentHeight}, Desired Height: {desiredHeight}");

            // Überprüfung mit Dead Zone
            float heightDifference = currentHeight - desiredHeight;

            if (Mathf.Abs(heightDifference) > deadZone)
            {
                float direction = Mathf.Sign(heightDifference); // +1 = zu hoch, -1 = zu niedrig
                float velocity = -direction * moveSpeed; // Negativ weil wir in Richtung der gewünschten Höhe gehen
                Debug.Log($"Adjusting height: {velocity}");
                return velocity;
            }

            return 0; // In der Dead Zone, also keine Bewegung notwendig
        }

        Debug.LogWarning("No ground detected! Keeping current Y position.");
        return 0; // Falls kein Boden erkannt wird, nicht bewegen
    }


}