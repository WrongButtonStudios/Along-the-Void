using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourComponentSelecter : MonoBehaviour
{
    public static sbyte SelectAttackComponent(BehaviourStateHandler entity, sbyte curIndex)
    {
        if (entity.AttackPatterns[curIndex].FinnishedAttack() == false)
            return curIndex;

        entity.AttackPatterns[curIndex].ResetAttackStatus();
        return (sbyte)Random.Range(0, entity.AttackPatterns.Count - 1);
    }

    public static sbyte SelectMovementComponent(BehaviourStateHandler entity, sbyte curIndex)
    {
        if (entity.PatrolPatterns[curIndex].ReachedDestination() == false)
            return curIndex; 

        return (sbyte)Random.Range(0, entity.PatrolPatterns.Count - 1);
    }
}