using UnityEngine;
using System.Collections.Generic;

public class playerFlipGravity : MonoBehaviour, IplayerFeature
{
    private characterController characterController;
    private ContactFilter2D contactFilter = new ContactFilter2D();
    private readonly string defaultLayerName = "redSlimeArea";

    private bool isActive = false;
    private bool isAscending = false;
    private float lastFlipTime;
    [SerializeField] private float groundCheckDelay = 0.01f;

    public LayerMask layerMask;
    public float ascendGravityScale = 3f;

    private void Awake()
    {
        layerMask = LayerMask.GetMask(defaultLayerName);

        contactFilter.useTriggers = true;
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask;
    }

    private void FixedUpdate()
    {
        if (!isActive) return;

        if (isAscending)
        {
            if (Time.time - lastFlipTime <= groundCheckDelay || !characterController.getPlayerStatus().isGrounded)
            {
                characterController.rb.gravityScale = -ascendGravityScale;
            }
            else
            {
                isAscending = false;
                characterController.rb.gravityScale = -1f;
            }
        }
    }

    public void initFeature(characterController characterController)
    {
        this.characterController = characterController;
    }

    public void triggerFeature(bool useInput = false, bool input = false)
    {
        if (!characterController.getPlayerStatus().isGrounded) return;
        List<Collider2D> colliders = new List<Collider2D>();
        characterController.rb.OverlapCollider(contactFilter, colliders);

        if (colliders.Count > 0)
        {
            Debug.Log("we are here");
            if (isActive)
            {
                endFeature();
            }
            else
            {
                isActive = true;
                isAscending = true;

                lastFlipTime = Time.time;
                
        Debug.Log("reverse gravity mf"); 
                characterController.rb.gravityScale = -ascendGravityScale;
            }
        }
    }

    public void endFeature()
    {
        isActive = false;
        isAscending = false;
        characterController.rb.gravityScale = 1f;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (LayerMask.LayerToName(collider.gameObject.layer) == defaultLayerName)
        {
            endFeature();
        }
    }
}