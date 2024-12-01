using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{

    private Vector2 moveInput;
    private bool dashInput;
    private bool lastDashInput;
    private bool triggerPlayerFeatureInput;

    //public gettter 
    public bool DashInput { get { return dashInput; } }
    public bool LastDashInput; 
    public bool TriggerPlayerFeatureInput { get { return triggerPlayerFeatureInput; } }
    public Vector2 MoveInput { get { return moveInput; } }
    public void getMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log(moveInput); 
    }

    public void getDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            dashInput = true;

        }
        else if (context.canceled)
        {
            dashInput = false;
        }
    }

    public void getTriggerPlayerFeatureInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            triggerPlayerFeatureInput = true;
        }
        else if (context.canceled)
        {
            triggerPlayerFeatureInput = false;
        }
    }

    public void ResetTriggerPlayerFeature()
    {
        triggerPlayerFeatureInput = false; 
    }
}
