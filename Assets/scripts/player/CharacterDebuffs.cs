using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static characterController;

public class CharacterDebuffs : MonoBehaviour
{
    // Start is called before the first frame update
    private characterController _controller; 
    public void setOnFire()
    {
        switch (_controller.StatusData.currentState)
        {
            case playerStates.green:
            case playerStates.burntGreen:
                _controller.StatusData.isOnFire = true;
                break;
        }
    }

    public void freeze()
    {
        switch (_controller.StatusData.currentState)
        {
            case playerStates.blue:
            case playerStates.burntBlue:
                _controller.StatusData.isFrozen = true;
                break;
        }
    }
}
