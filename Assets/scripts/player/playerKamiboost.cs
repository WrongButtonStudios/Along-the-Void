using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerKamiboost : MonoBehaviour, IplayerFeature
{

    private characterController characterController;
    private CharachterMovement characterMovement;
    private ContactFilter2D contactFilter = new ContactFilter2D();
    private string defaultLayerName = "yellowDustArea";
    public int kamiBoostSpeed = 300;

    public LayerMask layerMask;



    public void Awake()
    {
        layerMask = LayerMask.GetMask(defaultLayerName);

        contactFilter.useTriggers = true;
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask;
    }


    public void initFeauture(characterController characterController)
    {
        this.characterController = characterController;
        this.characterMovement = characterController.GetComponent<CharachterMovement>();
    }



    public void triggerFeauture(bool useInput = false, bool input = false)
    {
        if (!characterController.getPlayerStatus().isGrounded)
        {
            List<Collider2D> colliders = new List<Collider2D>();

            characterController.rb.OverlapCollider(contactFilter, colliders);

            if (colliders.Count > 0)
            {
                characterMovement.disableMovement();
                characterController.rb.gravityScale = 0;

                //Ich muss einfach an die Velocity Diggaaaaaa!!!
                characterController.rb.velocity = new Vector2(characterController.rb.velocity.x, 0);
                //Ich muss einfach an die Velocity Diggaaaaaa!!!
                characterMovement.setMaxSpeed(kamiBoostSpeed);
                characterController.rb.AddForce(Vector2.right * kamiBoostSpeed);


            }
        }
    }

    public void endFeauture()
    {
        characterController.rb.gravityScale = 1;
        characterMovement.enableMovement();

    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (LayerMask.LayerToName(collider.gameObject.layer) == defaultLayerName)
        {
            endFeauture();
        }
    }
}
