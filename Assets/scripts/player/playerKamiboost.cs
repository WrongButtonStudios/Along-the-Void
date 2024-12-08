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
            //Muss überarbeitet werden!
            characterController.rb.AddForce(characterController._input.MoveInput * kamiBoostSpeed);
            //Muss überarbeitet werden!
        }
    }


    public void initFeauture(characterController characterController)
    {
        this.characterController = characterController;
        this.characterMovement = characterController.GetComponent<CharacterMovement>();
    }



    public void triggerFeauture(bool useInput = false, bool input = false)
    {
        if (!characterController.getPlayerStatus().isGrounded)
        {
            List<Collider2D> colliders = new List<Collider2D>();

            characterController.rb.OverlapCollider(contactFilter, colliders);

            if (colliders.Count > 0)
            {
                doKamiboost = input;
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
