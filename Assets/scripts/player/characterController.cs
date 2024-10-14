using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class characterController : MonoBehaviour
{
    public enum playerStates
    {
        dead,
        green,
        red,
        blue,
        yellow,
        burntGreen,
        burntRed,
        burntBlue,
        burntYellow
    }

    [System.Serializable]
    public struct playerStatusData
    {
        public playerStates currentState;
        public bool isMoving;
        public bool isGrounded;
    }

    private Rigidbody2D rb;

    [SerializeField] private playerStatusData statusData = new playerStatusData();
    [SerializeField] private float maxSpeed = 60f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private AnimationCurve accelerationFactorFromDot;
    [SerializeField] private float counterMoveForce = 30f;
    [Space]
    [SerializeField] private float groundedDistance = 1.1f;
    [SerializeField] private LayerMask groundLayer;


    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        statusData.isMoving = moveInput.magnitude != 0;
    }

    private void FixedUpdate()
    {
        handleStates();
        handleStateTransitions();

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        if(rb.velocity.normalized.magnitude < 0.01f)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void handleStates()
    {
        switch (statusData.currentState)
        {
            case playerStates.dead:
                break;

            case playerStates.green:
                statusData.isGrounded = checkGrounded(out RaycastHit2D groundHit);

                if (statusData.isGrounded)
                {
                    transform.up = groundHit.normal;
                }
                else
                {
                    transform.up = Vector2.up;
                }

                baseMovement();
                break;

            default:
                Debug.LogError("state not implemented");
                break;
        }
    }

    public void handleStateTransitions()
    {
        
    }

    public void transitionToState(playerStates targetState)
    {
        bool transitionSuccesful = false;
        playerStates originState = statusData.currentState;

        if(statusData.currentState == targetState)
        {
            Debug.LogWarning("is allready in target state");
            return;
        }

        switch (statusData.currentState)
        {
            case playerStates.dead:
                Debug.LogWarning("cant transition out of dead state");
                break;

            case playerStates.green:
                switch (targetState)
                {
                    case playerStates.dead:
                        statusData.currentState = playerStates.dead;
                        
                        transitionSuccesful = true;
                        break;

                    default:
                        Debug.LogError("state transition target not implemented");
                        break;
                }
                break;

            default:
                Debug.LogError("state transition origin not implemented");
                break;
        }

        if (transitionSuccesful)
        {
            Debug.Log("transition from " + originState.ToString() + " to " + targetState.ToString());
        }
    }

    public void baseMovement()
    {
        movePlayer();

        if (!statusData.isMoving && statusData.isGrounded)
        {
            counterMovePlayer();
        }
    }

    public void movePlayer()
    {
        float accelerationToAdd = acceleration * moveInput.normalized.x;

        Vector2 forceToAdd = transform.right * accelerationToAdd;

        forceToAdd = forceToAdd * accelerationFactorFromDot.Evaluate(Vector2.Dot(forceToAdd.normalized, rb.velocity.normalized));

        rb.AddForce(forceToAdd, ForceMode2D.Force);
    }
    public void counterMovePlayer()
    {
        Vector2 horizontalVelocity = Vector2.right * Vector2.Dot(rb.velocity, transform.right);

        if (horizontalVelocity.magnitude > 0.01f)
        {
            Vector2 counterForce = -horizontalVelocity * rb.mass / Time.fixedDeltaTime;

            counterForce = Vector2.ClampMagnitude(counterForce, counterMoveForce);

            rb.AddForce(counterForce, ForceMode2D.Force);
        }
    }

    public bool checkGrounded(out RaycastHit2D hit)
    {
        return hit = Physics2D.Raycast(transform.position, -transform.up, groundedDistance, groundLayer);
    }

    //call this to check player status data outside of this script
    public playerStatusData getPlayerStatus()
    {
        return statusData;
    }
    public void getMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void getDashInput(InputAction.CallbackContext context)
    {
        
    }
}
