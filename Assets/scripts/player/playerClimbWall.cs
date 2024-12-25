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
    public LayerMask layerMask;

    public blueSlime currentBlueSlime;

    public float popOffForce = 20f;
    
    public float maxClimbSpeed = 0.8f;
    public float climbAcceleration = 5f;
    public float climbDeceleration = 2f;
    public float initialVelocityTransfer = 0.05f;
    
    private float currentClimbVelocity = 0f;
    
    private float currentPosOnLine;
    private InputController _input;
    private CharacterMovement _movement;

    public void Awake()
    {
        layerMask = LayerMask.GetMask(defaultLayerName);

        contactFilter.useTriggers = true;
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask;
    }

    void Start() 
    {
        _input = this.GetComponent<InputController>(); 
        _movement = this.GetComponent<CharacterMovement>();
    }
    
    public void FixedUpdate()
    {
        if(currentBlueSlime == null)
        {
            return;
        }

        float targetVelocity = -_input.MoveInput.y * maxClimbSpeed;
        
        if (Mathf.Abs(targetVelocity) > Mathf.Abs(currentClimbVelocity))
        {
            currentClimbVelocity = Mathf.MoveTowards(currentClimbVelocity, targetVelocity, climbAcceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentClimbVelocity = Mathf.MoveTowards(currentClimbVelocity, targetVelocity, climbDeceleration * Time.fixedDeltaTime);
        }

        currentClimbVelocity = Mathf.Clamp(currentClimbVelocity, -maxClimbSpeed, maxClimbSpeed);

        currentPosOnLine += currentClimbVelocity * Time.fixedDeltaTime;

        getPositionOnLine(currentBlueSlime.getLine(), currentPosOnLine, out Vector2 pos);
        characterController.rb.MovePosition(pos);
 
        StartCoroutine(popOff());
    }

    public IEnumerator popOff()
    {
        float normalizedForce = Mathf.Abs(currentClimbVelocity) / maxClimbSpeed;
        float finalForce = popOffForce * normalizedForce;

        if(currentPosOnLine > 1)
        {
            endFeauture();

            yield return new WaitForFixedUpdate();

            characterController.rb.AddForce(Vector2.down * finalForce, ForceMode2D.Impulse);
        }

        if(currentPosOnLine < 0)
        {
            endFeauture();

            yield return new WaitForFixedUpdate();

            characterController.rb.AddForce(Vector2.up * finalForce, ForceMode2D.Impulse);
        }
    }

    public void initFeauture(characterController characterController)
    {
        this.characterController = characterController;
    }

    public void triggerFeauture(bool useInput = false, bool input = false)
    {
        List<Collider2D> colliders = new List<Collider2D>();

        characterController.rb.OverlapCollider(contactFilter, colliders);

        if(colliders.Count > 0)
        {
            if(currentBlueSlime == null)
            {
                currentBlueSlime = colliders[0].GetComponent<blueSlime>();

                _movement.disableMovement();

                getClosestPointOnLine(out Vector2 closestPointOnLine, out float posOnLine);
                
                float playerVerticalVelocity = characterController.rb.velocity.y;
                
                currentClimbVelocity = -playerVerticalVelocity * initialVelocityTransfer;
                currentClimbVelocity = Mathf.Clamp(currentClimbVelocity, -maxClimbSpeed, maxClimbSpeed);
                
                characterController.rb.MovePosition(closestPointOnLine);
                characterController.rb.velocity = Vector2.zero;

                currentPosOnLine = posOnLine;
            }
            else
            {
                endFeauture();
            }
        }
    }

    public void endFeauture()
    {
        currentBlueSlime = null;
        currentClimbVelocity = 0f;
        _movement.enableMovement();
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if(LayerMask.LayerToName(collider.gameObject.layer) == defaultLayerName)
        {
            endFeauture();
        }
    }

    public bool getClosestPointOnLine(out Vector2 closestPointOnLine, out float posOnLine)
    {
        if(currentBlueSlime != null)
        {
            blueSlime.line line = currentBlueSlime.getLine();

            Vector2 lineVector = line.pointB - line.pointA;
            Vector2 playerVector = characterController.rb.position - line.pointA;
            
            float projectionLength = Vector3.Dot(playerVector, lineVector) / Vector3.Dot(lineVector, lineVector);
            float t = Mathf.Clamp01(projectionLength);
        
            closestPointOnLine = line.pointA + t * lineVector; 

            posOnLine = t;
            return true;
        }

        posOnLine = -1;
        closestPointOnLine = Vector2.zero;
        return false;
    }

    public bool getPositionOnLine(blueSlime.line line, float t, out Vector2 position)
    {
        position = line.pointA + t * (line.pointB - line.pointA);
        return true;
    }
}