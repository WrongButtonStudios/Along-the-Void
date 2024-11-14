using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStompAttack : MonoBehaviour, IplayerFeature
{
    private characterController characterController;

    [SerializeField] private float downForce = 500f;


    public void initFeauture(characterController characterController)
    {
        this.characterController = characterController;
    }

    public void triggerFeauture()
    {
        print("bububu");

        characterController.playerStatusData playerStatusData = characterController.getPlayerStatus();

        if(!playerStatusData.isGrounded)
        {
            characterController.rb.AddForce(Vector2.down * downForce, ForceMode2D.Impulse);
        }
    }
}
