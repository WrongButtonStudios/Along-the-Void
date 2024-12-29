using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerKamiboost : MonoBehaviour, IplayerFeature
{

    private characterController characterController;
    private CharacterMovement characterMovement;
    private ContactFilter2D contactFilter = new ContactFilter2D();
    private string defaultLayerName = "yellowDustArea";
    public int kamiBoostSpeed = 200;
    private bool doKamiboost = false;
    private Vector2 _dir; 
    public LayerMask layerMask;



    public void Awake()
    {
        layerMask = LayerMask.GetMask(defaultLayerName);

        contactFilter.useTriggers = true;
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask;
    }

    public void FixedUpdate()
    {
        

        if (doKamiboost)
        {
            characterMovement.disableMovement();
            characterController.rb.gravityScale = 0;

            //Ich muss einfach an die Velocity Diggaaaaaa!!!
            characterController.rb.velocity = new Vector2(characterController.rb.velocity.x, 0);
            //Ich muss einfach an die Velocity Diggaaaaaa!!!
            characterMovement.setMaxSpeed(kamiBoostSpeed);
            //Muss ?berarbeitet werden!
            characterController.rb.AddForce(_dir * kamiBoostSpeed);
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
        if (!characterController.getPlayerStatus().isGrounded && !doKamiboost)
        {
            List<Collider2D> colliders = new List<Collider2D>();

            characterController.rb.OverlapCollider(contactFilter, colliders);

            if (colliders.Count > 0)
            {
                Debug.Log("Activated Kami boost..."); 
                doKamiboost = input;
                Debug.Log("doKamiboost = " + doKamiboost);
            }

            if (characterController._input.MoveInput.magnitude > 0.1f)
            {
                _dir = characterController._input.MoveInput;
            }
            else
            {
                input = false;
                Debug.Log("No direction input recieved. Stoped using Kami Boost"); 
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
