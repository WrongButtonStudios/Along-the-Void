using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerKamiboost : MonoBehaviour, IplayerFeature
{

    private characterController characterController;
    private CharacterMovement characterMovement;
    private ContactFilter2D contactFilter = new ContactFilter2D();
    private CollisionHandler _collisionHandler;
    private float damagePerSecond = 0.1f;
    private fairyController _fairyController;  
    private string defaultLayerName = "yellowDustArea";
    public int kamiBoostSpeed = 200;
    private bool doKamiboost = false;
    private Vector2 _dir; 
    public LayerMask layerMask;



    public void Awake()
    {
        layerMask = LayerMask.GetMask(defaultLayerName);
        _collisionHandler = this.GetComponent<CollisionHandler>();
        _fairyController = this.GetComponent<Warmodes>().FairyController; 
        contactFilter.useTriggers = true;
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask;
    }

    public void FixedUpdate()
    {
        

        if (doKamiboost)
        {
            if (_collisionHandler.InYellowFog == false)
            {
               float yellowColorAmount =  PlayerDamageHandler.GetDamage(damagePerSecond * Time.fixedDeltaTime, PlayerUttillitys.GetPlayerColor(characterController), _fairyController);
                Debug.Log("deale damage..."); 
                if (yellowColorAmount <= 0f)
                {
                    endFeauture();
                    return; 
                }
            }
            if (!characterController.Input.TriggerPlayerFeatureInput)
            {
                endFeauture();
                return; 
            }
            characterMovement.disableMovement();
            characterController.rb.gravityScale = 0;

            //Ich muss einfach an die Velocity Diggaaaaaa!!!
            characterController.rb.velocity = new Vector2(characterController.rb.velocity.x, 0);
            //Ich muss einfach an die Velocity Diggaaaaaa!!!
            characterMovement.setMaxSpeed(kamiBoostSpeed);
            //Muss ?berarbeitet werden!
            characterController.rb.AddForce(_dir.normalized * kamiBoostSpeed);
            //Muss ?berarbeitet werden!
        }
    }


    public void initFeauture(characterController characterController)
    {
        this.characterController = characterController;
        this.characterMovement = characterController.GetComponent<CharacterMovement>();
    }



    public void triggerFeauture(bool useInput = false, bool input = false)
    {
        float yellowColorAmount = PlayerDamageHandler.GetHealth(PlayerUttillitys.GetPlayerColor(characterController), _fairyController); 
        if (!characterController.getPlayerStatus().isGrounded && !doKamiboost && yellowColorAmount > 0)
        {
            List<Collider2D> colliders = new List<Collider2D>();

            characterController.rb.OverlapCollider(contactFilter, colliders);

            //if (colliders.Count > 0)
            //{
            //Game Design Änderung laut Robin: Kamiboost soll ausserhalb des Gelben nebels Damage dealen, aber aktiviebar sein
                Debug.Log("Activated Kami boost..."); 
                doKamiboost = input;
                Debug.Log("doKamiboost = " + doKamiboost);
            //}

            if (characterController.Input.MoveInput.magnitude > 0.1f)
            {
                _dir = characterController.Input.MoveInput;
            }
            else
            {
                _dir = Vector2.right; 
            }

            if (input == false)
            {
                endFeauture();
            }
        }
    }

    public void endFeauture()
    {
        characterController.rb.gravityScale = 1;
        characterMovement.enableMovement();
        doKamiboost = false;
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (LayerMask.LayerToName(collider.gameObject.layer) == defaultLayerName)
        {
            endFeauture();
        }
    }
}
