using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUttillitys : MonoBehaviour
{
    /// <summary>
    /// Returns the Color of the given state of the Player.
    /// </summary>
    /// <param name="cc"></param>
    /// <returns></returns>
    public static PlayerColor GetPlayerColor(characterController cc)
    {
        if (cc.getPlayerStatus().currentState == characterController.playerStates.red || cc.getPlayerStatus().currentState == characterController.playerStates.burntRed)
        {
            return PlayerColor.red;
        }
        else if (cc.getPlayerStatus().currentState == characterController.playerStates.green || cc.getPlayerStatus().currentState == characterController.playerStates.burntGreen)
        {
            return PlayerColor.green;
        }
        else if (cc.getPlayerStatus().currentState == characterController.playerStates.blue || cc.getPlayerStatus().currentState == characterController.playerStates.burntBlue)
        {
            return PlayerColor.blue;
        }
        else if (cc.getPlayerStatus().currentState == characterController.playerStates.yellow || cc.getPlayerStatus().currentState == characterController.playerStates.burntYellow)
        {
            return PlayerColor.yellow;
        }
        else
            return PlayerColor.unknown;
    }

    /// <summary>
    /// Translates a given state into a color.
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static PlayerColor GetPlayerColor(characterController.playerStates state)
    {
        if (state == characterController.playerStates.red || state == characterController.playerStates.burntRed)
        {
            return PlayerColor.red;
        }
        else if (state == characterController.playerStates.green || state == characterController.playerStates.burntGreen)
        {
            return PlayerColor.green;
        }
        else if (state == characterController.playerStates.blue || state == characterController.playerStates.burntBlue)
        {
            return PlayerColor.blue;
        }
        else if (state == characterController.playerStates.yellow || state == characterController.playerStates.burntYellow)
        {
            return PlayerColor.yellow;
        }
        else
            return PlayerColor.unknown;
    }
    /// <summary>
    /// Returns a fairy with color amount > 0
    /// </summary>
    /// <param name="fairyController"></param>
    /// <returns></returns>
    public static characterController.playerStates GetNewColor(fairyController fairyController)
    {
        for (int i = 0; i < fairyController.fairys.Capacity; ++i)
        {
            if (fairyController.fairys[i].colorAmount > 0)
            {
                return (characterController.playerStates)i + 1;
            }
        }

        throw new System.Exception("alle tot oder so, mies gelaufen");
    }
}

public enum PlayerColor
{
    green,
    red,
    blue,
    yellow,
    unknown
}

