using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    /// <summary>
    /// Deals damage to the Player
    /// </summary>
    /// <param name="v">the amount of the Damage.</param>
    /// <param name="colorRequiered">the color of the fairy that should take damage.</param>
    /// <param name="fairyController">the fairy controller that controlls the fairys.</param>
    /// <returns></returns>
    public static float GetDamage(float v, PlayerColor colorRequiered, fairyController fairyController)
    {
        for (int i = 0; i < fairyController.fairys.Capacity; ++i)
        {
            if ((int)fairyController.fairys[i].color == (int)colorRequiered)
            {
                //deale damage
                fairyController.fairys[i].colorAmount -= v;
                return fairyController.fairys[i].colorAmount;
            }
        }
        Debug.LogError("fairy for this color does not exist");
        return -1;
    }

    public static float GetHealth(PlayerColor colorRequiered, fairyController fairyController)
    {
        for (int i = 0; i < fairyController.fairys.Capacity; ++i)
        {
            if ((int)fairyController.fairys[i].color == (int)colorRequiered)
            {
                return fairyController.fairys[i].colorAmount;
            }
        }
        Debug.LogError("fairy for this color does not exist");
        return -1;
    }
}
