using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{

    private float maxSpeed;

    //variable block used for movement
    [SerializeField] private float maxMovementSpeed = 60f;
    [SerializeField] private float maxSpeedChangeSpeed = 1f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private AnimationCurve accelerationFactorFromDot;
    [SerializeField] private float counterMoveForce = 30f;
    [SerializeField] private float inAirTurnSpeed = 2f; //will turn player to allogn local up to world up when in air
    [Space]

    //variable block used for hovering while staning
    [SerializeField] private float rideHeight = 1f;
    [SerializeField] private float maxRideHeight = 1f;
    [SerializeField] private float rideSpringStrenght = 1f;
    [SerializeField] private float rideSpringDamper = 1f;
    [SerializeField] private float groundedDistance = 1.1f;
    [SerializeField] private LayerMask groundLayer;
    [Space]

    //variable block for dash and falling bs
    [SerializeField] private float deccendGravityMultiplier = 2f;
    [SerializeField] private float dashStrenght = 50f;
    [SerializeField] private float dashMaxSpeed = 100f;
    
    //dependencys 
    [SerializeField]
    private characterController _controller;
    [SerializeField]
    private InputController _input; 
 

    //public Getter
    public float MaxRideHight { get { return maxRideHeight; } }
    public float RideHight { get { return rideHeight;  } }
    public float GroundDistance { get { return groundedDistance; } }
    public float InAirTurnSpeed { get { return inAirTurnSpeed; } }
    public float DeccendGravityMultiplier { get { return deccendGravityMultiplier;} }

    private void Start()
    {
        maxSpeed = maxMovementSpeed; 
    }

    public void lerpCurrentMaxSpeedToMaxSpeed()
    {
        if (maxSpeed > maxMovementSpeed)
        {
            maxSpeed = Mathf.Lerp(maxSpeed, maxMovementSpeed, maxSpeedChangeSpeed * Time.deltaTime);
        }
    }

    public float GetMaxSpeed() 
    {
        return maxSpeed; 
    }

    public void disableMovement()
    {
        _controller.StatusData.isAllowedToMove = false;
    }

    public void enableMovement()
    {
        _controller.StatusData.isAllowedToMove = true;
    }

    public void baseMovement()
    {
        if (_controller.StatusData.isAllowedToMove)
        {
            movePlayer();
        }

        if (!_controller.StatusData.isMoving && _controller.StatusData.isGrounded)
        {
            counterMovePlayer();
        }
    }

    public void movePlayer()
    {
        float accelerationToAdd = acceleration * _input.MoveInput.normalized.x;

        Vector2 forceToAdd = transform.right * accelerationToAdd;

        forceToAdd = forceToAdd * accelerationFactorFromDot.Evaluate(Vector2.Dot(forceToAdd.normalized, _controller.rb.velocity.normalized));

        _controller.rb.AddForce(forceToAdd, ForceMode2D.Force);
    }
    
    public void setMaxSpeed(float maxSpeedToSet)
    {
        maxSpeed = maxSpeedToSet;
    }

    public void dash()
    {
        //when dash input is pressed
            //set max speed to dashSpeed
            //when wasnt dahsing 
                //apply dash initial boost in input direction

        if (_input.DashInput)
        {
            foreach(IplayerFeature feature in _controller.GetPlayerFeatures)
            {
                feature.endFeauture();
            }

            _controller.StatusData.isDash = true;
            setMaxSpeed(dashMaxSpeed);

            if(!_controller.StatusData.wasDash)
            {
                _controller.rb.AddForce(_input.MoveInput * dashStrenght, ForceMode2D.Impulse);
            }
        }
    }

    public void hoverAboveGround(RaycastHit2D groundHit)
    {
        if (_controller.StatusData.isGrounded && !_controller.StatusData.isMoving)
        {
            if (groundHit.distance < maxRideHeight)
            {
                Vector2 yVelocity = _controller.rb.velocity;
                yVelocity.x = 0;

                float distanceToGround = groundHit.distance;
                Vector2 upForce = (Vector2.up * _controller.rb.gravityScale) * (rideHeight - distanceToGround) * rideSpringStrenght;
                Vector2 dampingForce = -yVelocity * rideSpringDamper;

                _controller.rb.AddForce(upForce + dampingForce, ForceMode2D.Force);
            }
        }
    }

    public void counterMovePlayer()
    {
        Vector2 horizontalVelocity = Vector2.right * Vector2.Dot(_controller.rb.velocity, transform.right);

        if (horizontalVelocity.magnitude > 0.01f)
        {
            Vector2 counterForce = -horizontalVelocity * _controller.rb.mass / Time.fixedDeltaTime;

            counterForce = Vector2.ClampMagnitude(counterForce, counterMoveForce);

            _controller.rb.AddForce(counterForce, ForceMode2D.Force);
        }
    }

    public AnimationCurve returnAccelerationCurve()
    {
        return accelerationFactorFromDot;
    }
}
