using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.UI;
using UnityEngine;

public class playerFlipGravity : MonoBehaviour, IplayerFeature
{
    private characterController characterController;
    private ContactFilter2D contactFilter = new ContactFilter2D();
    private string defaultLayerName = "redSlimeArea";

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
        List<Collider2D> colliders = new List<Collider2D>();

        characterController.rb.OverlapCollider(contactFilter, colliders);

        if(colliders.Count > 0)
        {
            characterController.rb.gravityScale = -characterController.rb.gravityScale;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if(LayerMask.LayerToName(collider.gameObject.layer) == defaultLayerName)
        {
            characterController.rb.gravityScale = Mathf.Abs(characterController.rb.gravityScale);
        }
    }
}
