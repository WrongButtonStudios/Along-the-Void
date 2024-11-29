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
    public class playerStatusData
    {
        public playerStates currentState;
        public bool isAllowedToMove;
        public bool isMoving;
        public bool isGrounded;
        public bool isDash;
        public bool isFrozen;
        public bool isOnFire;
    }

    public Rigidbody2D rb;

    [SerializeField] private playerStatusData statusData = new playerStatusData();


    public playerStatusData StatusData { get { return statusData; } }

    private List<IplayerFeature> playerFeatures = new List<IplayerFeature>();

    private Vector2 moveInput;
    private bool dashInput;
    private bool lastDashInput;
    private bool triggerPlayerFeatureInput;

    //Dependencys 
    [SerializeField]
    private CharachterMovement _movement;
    [SerializeField]
    private CollisionHandler _collision;
    [SerializeField]
    private CharachterBuffs _buffs;
    [SerializeField]
    private InputController _input; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        statusData.isAllowedToMove = true;

        IplayerFeature playerStompAttack = this.AddComponent<playerStompAttack>();
        playerFeatures.Add(playerStompAttack);

        IplayerFeature playerFlipGravity = this.AddComponent<playerFlipGravity>();
        playerFeatures.Add(playerFlipGravity);

        IplayerFeature playerClimbWall = this.AddComponent<playerClimbWall>();
        playerFeatures.Add(playerClimbWall);

        IplayerFeature playerKamiboost = this.AddComponent<playerKamiboost>();
        playerFeatures.Add(playerKamiboost);

        foreach (IplayerFeature iplayerFeature in playerFeatures)
        {
            iplayerFeature.initFeauture(this);
        }
    }

    private void FixedUpdate()
    {
        statusData.isMoving = _input.MoveInput.x != 0;


        _movement.ClampVelocity();
        handleStates();
        handleStateTransitions();


        rb.velocity = Vector2.ClampMagnitude(rb.velocity, _movement.GetMaxSpeed());

        _input.LastDashInput = _input.DashInput;
    }

    public void handleStates()
    {
        switch (statusData.currentState)
        {
            case playerStates.dead:
                break;

            //state falltrough to green so robin can test. this isnt final
            case playerStates.burntGreen:
            case playerStates.burntRed:
            case playerStates.burntBlue:
            case playerStates.burntYellow:

            case playerStates.green:
                playerFeatures.OfType<playerStompAttack>().FirstOrDefault().triggerFeauture(true, triggerPlayerFeatureInput);
                break;

            case playerStates.red:

                if (triggerPlayerFeatureInput)
                {
                    playerFeatures.OfType<playerFlipGravity>().FirstOrDefault().triggerFeauture();

                    triggerPlayerFeatureInput = false;
                }
                break;

            case playerStates.blue:
                if (triggerPlayerFeatureInput)
                {
                    playerFeatures.OfType<playerClimbWall>().FirstOrDefault().triggerFeauture();

                    triggerPlayerFeatureInput = false;
                }
                break;

            case playerStates.yellow:
                if (triggerPlayerFeatureInput)
                {
                    playerFeatures.OfType<playerKamiboost>().FirstOrDefault().triggerFeauture();

                    triggerPlayerFeatureInput = false;
                }
                break;

            default:
                Debug.LogError("state not implemented");
                break;
        }
        RaycastHit2D groundHit = _collision.doGroundedCheck();
        _movement.dash();

        _movement.baseMovement();

        _movement.hoverAboveGround(groundHit);
    }

    public void handleStateTransitions()
    {

    }

    private void TransitionEndPlayerFeature() 
    {
        switch (statusData.currentState) 
        {
            case playerStates.dead:
                break;
            case playerStates.green:
                playerFeatures.OfType<playerStompAttack>().FirstOrDefault().endFeauture();
                break;
            case playerStates.red:
                playerFeatures.OfType<playerFlipGravity>().FirstOrDefault().endFeauture();
                break; 
            case playerStates.yellow:
                playerFeatures.OfType<playerKamiboost>().FirstOrDefault().endFeauture();
                break;
            case playerStates.blue:
                playerFeatures.OfType<playerClimbWall>().FirstOrDefault().endFeauture();
                break;
            default:
                Debug.LogError("unhandled state: " + statusData.currentState);
                break; 
        }
    }

    private void HandlePlayerBuffsForNewState(playerStates targetState) 
    {
        switch (targetState) 
        {
            case playerStates.blue: 
                statusData.isOnFire = false;
                break;
            case playerStates.red: 
                statusData.isFrozen = false;
                break;
            default:
                Debug.Log("nothing changed on current player buffs");
                break; 
        }
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

        if (statusData.currentState == targetState)
        {
            Debug.LogWarning("is allready in target state");
            return;
        }
        TransitionEndPlayerFeature();
        statusData.currentState = targetState;
        transitionSuccesful = statusData.currentState == targetState; 
        if (transitionSuccesful)
        {
            HandlePlayerBuffsForNewState(targetState);
            Debug.Log("transition from " + originState.ToString() + " to " + targetState.ToString());
        } else 
        {
            Debug.LogError("Transition failed"); 
        }
    }

    //call this to check player status data outside of this script
    public playerStatusData getPlayerStatus()
    {
        return statusData;
    }
}