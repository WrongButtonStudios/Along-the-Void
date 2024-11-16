using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
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
        public bool isDash;
        public bool isFrozen;
    }

    public Rigidbody2D rb;

    [SerializeField] private playerStatusData statusData = new playerStatusData();
    [SerializeField] private float maxMovementSpeed = 60f;
    [SerializeField] private float maxSpeedChangeSpeed = 1f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private AnimationCurve accelerationFactorFromDot;
    [SerializeField] private float counterMoveForce = 30f;
    [SerializeField] private float inAirTurnSpeed = 2f; //will turn player to allogn local up to world up when in air
    [Space]
    [SerializeField] private float rideHeight = 1f;
    [SerializeField] private float maxRideHeight = 1f;
    [SerializeField] private float rideSpringStrenght = 1f;
    [SerializeField] private float rideSpringDamper = 1f;
    [SerializeField] private float groundedDistance = 1.1f;
    [SerializeField] private LayerMask groundLayer;
    [Space]
    [SerializeField] private float deccendGravityMultiplier = 2f;
    [SerializeField] private float dashStrenght = 50f;
    [SerializeField] private float dashMaxSpeed = 100f;

    private float maxSpeed;

    private List<IplayerFeature> playerFeatures = new List<IplayerFeature>();

    private Vector2 moveInput;
    private bool dashInput;
    private bool lastDashInput;
    private bool triggerPlayerFeatureInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        maxSpeed = maxMovementSpeed;

        IplayerFeature playerStompAttack = this.AddComponent<playerStompAttack>();
        playerFeatures.Add(playerStompAttack);

        IplayerFeature playerFlipGravity = this.AddComponent<playerFlipGravity>();
        playerFeatures.Add(playerFlipGravity);

        foreach(IplayerFeature iplayerFeature in playerFeatures)
        {
            iplayerFeature.initFeauture(this);
        }
    }

    private void FixedUpdate()
    {
        statusData.isMoving = moveInput.x != 0;

        if(maxSpeed > maxMovementSpeed)
        {
            maxSpeed = Mathf.Lerp(maxSpeed, maxMovementSpeed, maxSpeedChangeSpeed * Time.deltaTime);
        }

        handleStates();
        handleStateTransitions();

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        lastDashInput = dashInput;
    }

    public void handleStates()
    {
        switch (statusData.currentState)
        {
            case playerStates.dead:
                break;

            //state falltrough to green so robin can test. this isnt final
            case playerStates.blue:
            case playerStates.yellow:
            case playerStates.burntGreen:
            case playerStates.burntRed:
            case playerStates.burntBlue:
            case playerStates.burntYellow:
            case playerStates.green:
                RaycastHit2D groundHit = doGroundedCheck();

                if(triggerPlayerFeatureInput)
                {
                    playerFeatures.OfType<playerStompAttack>().FirstOrDefault().triggerFeauture();

                    triggerPlayerFeatureInput = false;
                }

                dash();

                baseMovement();

                hoverAboveGround(groundHit);
                break;

            case playerStates.red:
                groundHit = doGroundedCheck();

                if(triggerPlayerFeatureInput)
                {
                    playerFeatures.OfType<playerFlipGravity>().FirstOrDefault().triggerFeauture();

                    triggerPlayerFeatureInput = false;
                }

                dash();

                baseMovement();

                hoverAboveGround(groundHit);
                break;

            default:
                Debug.LogError("state not implemented");
                break;
        }
    }

    public void handleStateTransitions()
    {
        
    }

    public void transitionToState(playerStates targetState, bool force = false)
    {
        //super ugly. again so robin can test
        if (force)
        {
            Debug.LogWarning("force transition to " + targetState.ToString());

            statusData.currentState = targetState;
            return;
        }

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

                    case playerStates.red:
                        statusData.currentState = playerStates.red;

                        transitionSuccesful = true;
                        break;

                    default:
                        Debug.LogError("state transition target not implemented");
                        break;
                }
                break;

            case playerStates.red:
                rb.gravityScale = Mathf.Abs(rb.gravityScale);

                switch (targetState)
                {
                    case playerStates.dead:
                        statusData.currentState = playerStates.dead;
                        
                        transitionSuccesful = true;
                        break;

                    case playerStates.green:
                        statusData.currentState = playerStates.green;

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

    public RaycastHit2D doGroundedCheck()
    {
        statusData.isGrounded = checkGrounded(out RaycastHit2D groundHit);

        if (statusData.isGrounded)
        {
            transform.up = groundHit.normal * rb.gravityScale;
        }
        else
        {
            transform.up = Vector2.Lerp(transform.up, Vector2.up, Time.deltaTime * inAirTurnSpeed);

            //this ads downwards force to make the gravity more gamey. does alot for gamefeel
            if (rb.velocity.y * rb.gravityScale < 0)
            {
                rb.AddForce((Physics2D.gravity * rb.gravityScale) * deccendGravityMultiplier, ForceMode2D.Force);
            }
        }

        return groundHit;
    }

    public void dash()
    {
        if(dashInput && !lastDashInput && !statusData.isDash)
        {
            statusData.isDash = true;
            maxSpeed = dashMaxSpeed;

            if(moveInput.magnitude != 0)
            {
                rb.AddForce(moveInput * dashStrenght, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(Vector2.up * dashStrenght, ForceMode2D.Impulse);
            }
        }
    }

    public void hoverAboveGround(RaycastHit2D groundHit)
    {
        if(statusData.isGrounded && !statusData.isMoving && !statusData.isDash)
        {
            if(groundHit.distance < maxRideHeight)
            {
                Vector2 yVelocity = rb.velocity;
                yVelocity.x = 0;

                float distanceToGround = groundHit.distance;
                Vector2 upForce = (Vector2.up * rb.gravityScale) * (rideHeight - distanceToGround) * rideSpringStrenght;
                Vector2 dampingForce = -yVelocity * rideSpringDamper;

                rb.AddForce(upForce + dampingForce, ForceMode2D.Force);
            }
        }
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
        hit = Physics2D.Raycast(transform.position, -transform.up * rb.gravityScale, Mathf.Infinity, groundLayer);
        
        if(hit.collider != null)
        {
            if(statusData.isGrounded)
            {
                return hit.distance <= maxRideHeight;
            }
            else
            {
                return hit.distance <= groundedDistance;
            }
        }

        return false;
    }

    private void OnCollisionStay2D()
    {
        statusData.isDash = false;
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
        if(context.performed)
        {
            dashInput = true;
        }
        else if(context.canceled)
        {
            dashInput = false;
        }
    }

    public void getTriggerPlayerFeatureInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            triggerPlayerFeatureInput = true;
        }
        else if(context.canceled)
        {
            triggerPlayerFeatureInput = false;            
        }
    }
}
