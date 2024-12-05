using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warmodes : MonoBehaviour
{
    private bool _isActive = false;
    private CharacterDebuffs _characterDebuffs;
    private CollisionHandler _collisionHandler;
    private float _curHP; 
    [SerializeField, Tooltip("Defines the amount of the color/hp loss per second. Max Coloramount/Fairry = 1")]
    private float _colorLossAmount = 0.05f;

    public bool IsActive { get { return _isActive; } }

    private void Awake()
    {
        _characterDebuffs = this.GetComponent<CharacterDebuffs>();
        _collisionHandler = this.GetComponent<CollisionHandler>(); 
    }

    private void Update()
    {
        //early out if not in use
        if (!_isActive)
            return;

        _curHP =  _collisionHandler.GetDamage(_colorLossAmount*Time.deltaTime, _collisionHandler.GetPlayerColor());
        if (_curHP <= 0)
        {
            _isActive = false; 
        }
    }
    public void UseWarmode(characterController.playerStates color)
    {
        switch (color)
        {
            case characterController.playerStates.blue:
            case characterController.playerStates.burntBlue:
                break;
            case characterController.playerStates.red:
            case characterController.playerStates.burntRed:
                _isActive = true;  
                _characterDebuffs.setOnFire();
                break;
            default:
                Debug.LogWarning("Other stuff needs to be implemented");
                break; 
        }
    }


}
