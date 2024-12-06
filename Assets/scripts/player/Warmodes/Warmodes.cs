using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warmodes : MonoBehaviour
{
    private bool _isActive = false;
    private CharacterDebuffs _characterDebuffs;
    private CollisionHandler _collisionHandler;
    private characterController _cc;
    private float _curHP;

    [SerializeField, Tooltip("Defines the amount of the color/hp loss per second. Max Coloramount/Fairry = 1")]
    private float _colorLossAmount = 0.05f;
    [SerializeField, Tooltip("The Ice Shooting turret for the Bluewarmode")]
    private GameObject _turret;

    private characterController.playerStates _curWarMode; 

    public bool IsActive { get { return _isActive; } }

    private void Start()
    {
        _characterDebuffs = this.GetComponent<CharacterDebuffs>();
        _collisionHandler = this.GetComponent<CollisionHandler>();
        _cc = this.GetComponent<characterController>(); 
    }

    private void Update()
    {
        //early out if not in use
        if (!_isActive)
            return;

        _curHP =  _collisionHandler.GetDamage(_colorLossAmount*Time.deltaTime, _collisionHandler.GetPlayerColor(_curWarMode));
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
                BlueWarMode(); 
                break;
            case characterController.playerStates.red:
                RedWarMode(); 
                break;
            default:
                Debug.LogWarning("Other stuff needs to be implemented/Or allready in warmode");
                break; 
        }
    }

    private void RedWarMode()
    {
        _isActive = true;
        _cc.StatusData.currentState = characterController.playerStates.burntRed;
        _curWarMode = _cc.StatusData.currentState;
        _characterDebuffs.setOnFire();
    }

    private void BlueWarMode()
    {
        _isActive = true;
        _cc.StatusData.currentState = characterController.playerStates.burntBlue;
        _curWarMode = _cc.StatusData.currentState; 
        Instantiate(_turret, transform.position, Quaternion.identity);
    }

}
