using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerStompAttack : MonoBehaviour, IplayerFeature
{
    private characterController characterController;

    [SerializeField] private float downForce = 300f;
    [SerializeField] private float maxSpeed = 150f;
    [SerializeField] private CharachterMovement _movement; 

    private bool doShit = false;

    private void Start()
    {
        _movement = this.GetComponent<CharachterMovement>(); 
    }

    public void FixedUpdate()
    {
        if(characterController.getPlayerStatus().isGrounded)
        {
            endFeauture();
        }

        if(doShit)
        {
         _movement.setMaxSpeed(maxSpeed);
            characterController.rb.AddForce(Vector2.down * downForce, ForceMode2D.Force);
        }
    }

    public void initFeauture(characterController characterController)
    {
        this.characterController = characterController;
    }

    public void triggerFeauture(bool useInput = false, bool input = false)
    {
        if(!useInput)
        {
            Debug.LogError("this feature needs input to work!");
            return;
        }

        if(!characterController.getPlayerStatus().isGrounded)
        {
            doShit = input;
        }
    }

    public void endFeauture()
    {
        doShit = false;
    }
}
