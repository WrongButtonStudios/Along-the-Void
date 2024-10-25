using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugPlayerColorChanger : MonoBehaviour
{
    public characterController player;

    [SerializeField] private Color greenColor;
    [SerializeField] private Color redColor;
    [SerializeField] private Color blueColor;
    [SerializeField] private Color yellowColor;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        switch (player.getPlayerStatus().currentState)
        {
            case characterController.playerStates.green:
                spriteRenderer.color = greenColor;
                break;

            case characterController.playerStates.red:
                spriteRenderer.color = redColor;
                break;

            case characterController.playerStates.blue:
                spriteRenderer.color = blueColor;
                break;

            case characterController.playerStates.yellow:
                spriteRenderer.color = yellowColor;
                break;
        }
    }
}
