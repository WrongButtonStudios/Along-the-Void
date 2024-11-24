using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class CollisionHandler : MonoBehaviour
{
    private fairyController _fairyController = null;
    private characterController _cc; 
    
    private enum PlayerColor 
    {
        red, 
        green, 
        blue, 
        yellow,
        unknown
    }

    private void Start()
    {
        _fairyController = GameObject.FindObjectOfType<fairyController>();
        _cc = this.gameObject.GetComponent<characterController>(); 
    }
    public void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        Debug.Log("trigger entered"); 
        var playerColor = GetPlayerColor();
        
        if (collision.gameObject.layer == 19 && playerColor == PlayerColor.red) 
        {
            Debug.Log("Autsch roter spike"); 
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name)
            for(int i = 0; i < _fairyController.fairys.Capacity; ++i)
            {
                if (_fairyController.fairys[i].color == fairy.fairyColor.red)
                {
                    //deale damage
                    _fairyController.fairys[i].colorAmount -= 0.25f;
                    Debug.Log(_fairyController.fairys[i].colorAmount); 
                    break; 
                }
            }
        }
    }

    private PlayerColor GetPlayerColor() 
    {
        if (_cc.getPlayerStatus().currentState == characterController.playerStates.red || _cc.getPlayerStatus().currentState == characterController.playerStates.burntRed)
        {
            return PlayerColor.red;
        }
        else if (_cc.getPlayerStatus().currentState == characterController.playerStates.green || _cc.getPlayerStatus().currentState == characterController.playerStates.burntGreen)
        {
            return PlayerColor.green;
        }
        else if (_cc.getPlayerStatus().currentState == characterController.playerStates.blue || _cc.getPlayerStatus().currentState == characterController.playerStates.burntBlue)
        {
            return PlayerColor.blue;
        }
        else if (_cc.getPlayerStatus().currentState == characterController.playerStates.yellow || _cc.getPlayerStatus().currentState == characterController.playerStates.burntYellow)
        {
            return PlayerColor.yellow;
        }
        else
            return PlayerColor.unknown; 
    }
}
