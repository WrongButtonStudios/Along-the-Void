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
    private GameObject _activeTurret;
    [SerializeField, Tooltip("Defines the amount of the color/hp loss per second. Max Coloramount/Fairry = 1")]
    private float _colorLossAmount = 0.05f;
    [SerializeField, Tooltip("The Ice Shooting turret for the Bluewarmode")]
    private GameObject _turret;
    [SerializeField, Tooltip("Time scale for slow motion (0-1)")]
    private float _slowMotionTimeScale = 0.5f;
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
        _curHP = _collisionHandler.GetDamage(_colorLossAmount * Time.unscaledDeltaTime, _collisionHandler.GetPlayerColor(_curWarMode));
        if (_curHP <= 0)
        {
            DeactivateWarmode();
        }
    }

    public void UseWarmode(characterController.playerStates color)
    {
        switch (color)
        {
            case characterController.playerStates.red:
                RedWarMode();
                break;
            case characterController.playerStates.blue:
                BlueWarMode();
                break;
            case characterController.playerStates.yellow:
                YellowWarMode();
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
        _activeTurret = Instantiate(_turret, transform.position, Quaternion.identity);
    }

    private void YellowWarMode()
    {
        _isActive = true;
        _cc.StatusData.currentState = characterController.playerStates.burntYellow;
        _curWarMode = _cc.StatusData.currentState;

        // Aktiviere Zeitlupe für die Welt
        Time.timeScale = _slowMotionTimeScale;
        // Time.fixedDeltaTime = Time.fixedDeltaTime * _slowMotionTimeScale;
    }

    private void DeactivateWarmode()
    {
        _isActive = false;
        var newState = _collisionHandler.GetNewColor();
        _cc.transitionToState(newState);
        switch (_curWarMode)
        {
            case characterController.playerStates.burntBlue:
                Destroy(_activeTurret);
                _activeTurret = null;
                break;
            case characterController.playerStates.burntYellow:
                // Setze Zeitlupe zurück
                Time.timeScale = 1f;
                // Time.fixedDeltaTime = Time.fixedDeltaTime / _slowMotionTimeScale;
                break;
            default:
                Debug.LogWarning("Other stuff is not implemented, does not need specific stuff to be done to deactivate");
                break;
        }
    }
}