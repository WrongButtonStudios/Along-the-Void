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
    private characterController _cc;
    private playerKamiboost _kBR;
    private Warmodes _warmode; 
    
    //public gettter 
    public bool DashInput { get { return dashInput; } }
    public bool LastDashInput; 
    public bool TriggerPlayerFeatureInput { get { return triggerPlayerFeatureInput; } }
    public Vector2 MoveInput { get { return moveInput; } }

    private void Start()
    {
        _cc = this.GetComponent<characterController>();
        _warmode = this.GetComponent<Warmodes>(); 
        _kBR = this.GetComponent<playerKamiboost>();
    }
    
    public void getMoveInput(InputAction.CallbackContext context)
    {
        //Debug.Log(moveInput); 
        moveInput = context.ReadValue<Vector2>();
    }

    public void getDashInput(InputAction.CallbackContext context)
    {
        if (context.performed && _kBR.getKamiboostStatus() == false)
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

    public void ActivateWarmode()
    {
        if (_warmode.IsActive == false)
            _warmode.UseWarmode(_cc.StatusData.currentState);
        else
            Debug.Log("All ready using warmode...");  
    }
}
