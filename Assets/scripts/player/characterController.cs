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
        public bool wasDash;
        public bool isFrozen;
        public bool isOnFire;
    }

    public Rigidbody2D rb;

    [SerializeField] private playerStatusData statusData = new playerStatusData();


    public playerStatusData StatusData { get { return statusData; } }

    private List<IplayerFeature> playerFeatures = new List<IplayerFeature>();
    public List<IplayerFeature> GetPlayerFeatures { get { return playerFeatures; } } 

    //Dependencys 
    private CharacterMovement _movement;
    public CollisionHandler Collision { get; private set;  }
    private CharacterDebuffs _buffs;
    public InputController _input; 

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

        //load in Dependencys
        _movement = this.GetComponent<CharacterMovement>();
        Collision = this.GetComponent<CollisionHandler>();
        _buffs = this.GetComponent<CharacterDebuffs>();
        _input = this.GetComponent<InputController>(); 
    }

    private void FixedUpdate()
    {
        statusData.isMoving = _input.MoveInput.x != 0;


        _movement.lerpCurrentMaxSpeedToMaxSpeed();
        handleStates();

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, _movement.GetMaxSpeed());

        _input.LastDashInput = _input.DashInput;
        statusData.wasDash = statusData.isDash;
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
                RaycastHit2D groundHit = Collision.doGroundedCheck();
        
                playerFeatures.OfType<playerStompAttack>().FirstOrDefault().triggerFeauture(true, _input.TriggerPlayerFeatureInput);
        
                _movement.dash();
                _movement.baseMovement();
                _movement.hoverAboveGround(groundHit);
                break;

            case playerStates.red:
                groundHit = Collision.doGroundedCheck();

                if (_input.TriggerPlayerFeatureInput)
                {
                    playerFeatures.OfType<playerFlipGravity>().FirstOrDefault().triggerFeauture();

                    _input.ResetTriggerPlayerFeature();
                }

                _movement.dash();
                _movement.baseMovement();
                _movement.hoverAboveGround(groundHit);
                break;

            case playerStates.blue:
                groundHit = Collision.doGroundedCheck();

                if (_input.TriggerPlayerFeatureInput)
                {
                    playerFeatures.OfType<playerClimbWall>().FirstOrDefault().triggerFeauture();

                    _input.ResetTriggerPlayerFeature();

                    statusData.isDash = false;
                    statusData.wasDash = false;
                }

                _movement.dash();
                _movement.baseMovement();
                _movement.hoverAboveGround(groundHit);
                break;

            case playerStates.yellow:
                groundHit = Collision.doGroundedCheck();

                
                    playerFeatures.OfType<playerKamiboost>().FirstOrDefault().triggerFeauture(true, _input.TriggerPlayerFeatureInput);
                   // _input.ResetTriggerPlayerFeature();
                

                _movement.dash();
                _movement.baseMovement();
                _movement.hoverAboveGround(groundHit);
                break;

            default:
                Debug.LogError("state not implemented");
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

        //state exit
        switch(statusData.currentState)
        {
            case playerStates.dead:
                Debug.LogWarning("cant transition out of dead state");
                break;

            case playerStates.green:
                playerFeatures.OfType<playerStompAttack>().FirstOrDefault().endFeauture();
                break;

            case playerStates.red:
                playerFeatures.OfType<playerFlipGravity>().FirstOrDefault().endFeauture();
                break;

            case playerStates.blue:
                playerFeatures.OfType<playerClimbWall>().FirstOrDefault().endFeauture();
                break;

            case playerStates.yellow:
                playerFeatures.OfType<playerKamiboost>().FirstOrDefault().endFeauture();
                break;

            case playerStates.burntGreen:
            case playerStates.burntRed:
            case playerStates.burntBlue:
            case playerStates.burntYellow:
                break;

            default:
                Debug.LogError("state transition origin not implemented");
                break;
        }

        //state entry
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

            case playerStates.red:
                statusData.currentState = playerStates.red;

                statusData.isFrozen = false;

                transitionSuccesful = true;
                break;

            case playerStates.blue:
                statusData.currentState = playerStates.blue;

                statusData.isOnFire = false;

                transitionSuccesful = true;
                break;

            case playerStates.yellow:
                statusData.currentState = playerStates.yellow;

                transitionSuccesful = true;
                break;

            case playerStates.burntGreen:
                statusData.currentState = playerStates.burntGreen;

                transitionSuccesful = true;
                break;

            case playerStates.burntRed:
                statusData.currentState = playerStates.burntRed;

                transitionSuccesful = true;
                break;

            case playerStates.burntBlue:
                statusData.currentState = playerStates.burntBlue;

                transitionSuccesful = true;
                break;

            case playerStates.burntYellow:
                statusData.currentState = playerStates.burntYellow;

                transitionSuccesful = true;
                break;

            default:
                Debug.LogError("state transition target not implemented");
                break;
        }

        if (transitionSuccesful)
        {
            Debug.Log("transition from " + originState.ToString() + " to " + targetState.ToString());
        }
        else 
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