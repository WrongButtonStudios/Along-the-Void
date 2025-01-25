using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ColorColliderHandler : MonoBehaviour
{
    [SerializeField] private PlayerColor _colorToInteract;
    [SerializeField] private characterController _characterController;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Material _deactivated;
    [SerializeField] private Material _activated;
    [SerializeField] private SpriteRenderer _renderer; 
    private PlayerColor _currentPlayerColor;

    private void Start()
    {
        CheckForColliderToggle(PlayerUttillitys.GetPlayerColor(_characterController));
    }
    void Update()
    {
        PlayerColor pC = PlayerUttillitys.GetPlayerColor(_characterController);
        if (pC == _currentPlayerColor)
        {
            return; //early out
        }
        CheckForColliderToggle(pC); 
    }

    private void CheckForColliderToggle(PlayerColor pC) 
    {
        _currentPlayerColor = pC;
        if (pC == _colorToInteract)
        {
            _collider.isTrigger = false;
            _renderer.material = _activated;
        }
        else
        {
            _collider.isTrigger = true;
            _renderer.material = _deactivated;
        }
    }
}
