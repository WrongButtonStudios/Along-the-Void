using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComponentSelecter : MonoBehaviour
{
    public static sbyte SelectNewWeapon(SimpleAI entity, sbyte curIndex)
    {
        if (entity.AttackComponents[curIndex].FinnishedAttack())
        {
            sbyte oldWeapon = curIndex;
            sbyte newWeaponIndex = (sbyte)Random.Range(0, entity.AttackComponents.Count - 1);
            entity.AttackComponents[oldWeapon].ResetAttackStatus();
            return newWeaponIndex; 
        }
        //enemy is still attacking
        return -1; 
    }

    public static sbyte SelectMovementComponent(SimpleAI entity, int curIndex)
    {
        if (entity.PatrolComponents[curIndex].ReachedDestination())
        {
            return (sbyte)Random.Range(0, entity.PatrolComponents.Count - 1);
        }
        //Enemy hasnt reached destination
        return -1; 
    }

}
