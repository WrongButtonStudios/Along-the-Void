using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourComponentSelecter : MonoBehaviour
{
    public static sbyte SelectAttackComponent(SimpleAI entity, sbyte curIndex)
    {
        if (entity.AttackComponents[curIndex].FinnishedAttack() == false)
            return curIndex;

        entity.AttackComponents[curIndex].ResetAttackStatus();
        return (sbyte)Random.Range(0, entity.AttackComponents.Count - 1);
    }

    public static sbyte SelectMovementComponent(SimpleAI entity, sbyte curIndex)
    {
        if (entity.PatrolComponents[curIndex].ReachedDestination() == false)
            return curIndex; 

        return (sbyte)Random.Range(0, entity.PatrolComponents.Count - 1);
    }
}