using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.UI;
using UnityEngine;

public class playerClimbWall : MonoBehaviour, IplayerFeature
{
    private characterController characterController;
    private ContactFilter2D contactFilter = new ContactFilter2D();
    private string defaultLayerName = "blueSlimeArea";


    [SerializeField] private float maxMovementSpeed = 60f;
    [SerializeField] private float maxSpeedChangeSpeed = 1f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private AnimationCurve accelerationFactorFromDot;

    public LayerMask layerMask;

    public bool climbMovementActive = false;

    public void Awake()
    {
        layerMask = LayerMask.GetMask(defaultLayerName);

        contactFilter.useTriggers = true;
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask;
    }

    public void FixedUpdate()
    {
        if(climbMovementActive)
        {
            Vector2 moveInput = characterController.returnMoveInput();

            moveClimbPlayer(moveInput);
        }
    }

    public void moveClimbPlayer(Vector2 moveInput)
    {
        float accelerationToAdd = acceleration * moveInput.normalized.y;

        Vector2 forceToAdd = transform.up * accelerationToAdd;

        forceToAdd = forceToAdd * accelerationFactorFromDot.Evaluate(Vector2.Dot(forceToAdd.normalized, characterController.rb.velocity.normalized));

        characterController.rb.AddForce(forceToAdd, ForceMode2D.Force);
    }

    public void initFeauture(characterController characterController)
    {
        this.characterController = characterController;

        accelerationFactorFromDot = characterController.returnAccelerationCurve();

    }

    public void triggerFeauture()
    {
        List<Collider2D> colliders = new List<Collider2D>();

        characterController.rb.OverlapCollider(contactFilter, colliders);

        if(colliders.Count > 0)
        {
            characterController.rb.gravityScale = 0;
            climbMovementActive = !climbMovementActive;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if(LayerMask.LayerToName(collider.gameObject.layer) == defaultLayerName)
        {
            characterController.rb.gravityScale = 1;
            climbMovementActive = false;
        }
    }
}
