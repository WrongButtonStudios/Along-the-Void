using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStompAttack : MonoBehaviour, IplayerFeature
{
    private characterController characterController;

    [SerializeField] private float downForce = 50f;
    [SerializeField] private float maxSpeed = 150f;


    public void initFeauture(characterController characterController)
    {
        this.characterController = characterController;
    }

    public void triggerFeauture()
    {
        if(!characterController.getPlayerStatus().isGrounded)
        {
            characterController.rb.AddForce(Vector2.down * downForce, ForceMode2D.Impulse);
        }
    }

    public void endFeauture()
    {
        
    }
}
