using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ColorColliderHandler : MonoBehaviour
{
    [SerializeField] private PlayerColor _colorToInteract;
    [SerializeField] private characterController _characterController;
    [SerializeField] private Collider2D _collider;
    private PlayerColor _currentPlayerColor; 

    void Update()
    {
        PlayerColor pC = PlayerUttillitys.GetPlayerColor(_characterController); 
        if(pC == _currentPlayerColor) 
        {
            return; //early out
        }
        _currentPlayerColor = pC; 
        if (pC == _colorToInteract) 
        {
            _collider.isTrigger = false; 
        } else 
        {
            _collider.isTrigger = true;
        }
    }
}
