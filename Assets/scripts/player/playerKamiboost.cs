using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerKamiboost : MonoBehaviour, IplayerFeature
{

    private characterController characterController;
    private ContactFilter2D contactFilter = new ContactFilter2D();
    private string defaultLayerName = "yellowDustArea";

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
    }

    public void triggerFeauture()
    {
        if (!characterController.getPlayerStatus().isGrounded)
        {
            List<Collider2D> colliders = new List<Collider2D>();

            characterController.rb.OverlapCollider(contactFilter, colliders);

            if (colliders.Count > 0)
            {
                characterController.rb.gravityScale = 0;
                characterController.rb.velocity = new Vector2 (characterController.rb.velocity.x, 0);
                characterController.rb.AddForce(Vector2.right * 20000);
            }
        }
    }

    public void endFeauture()
    {
        characterController.rb.gravityScale = 1;
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (LayerMask.LayerToName(collider.gameObject.layer) == defaultLayerName)
        {
            endFeauture();
        }
    }
}
