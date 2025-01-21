using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourStateMachine : MonoBehaviour
{
    public static BehaviourState UpdateState(BehaviourStateHandler entity, BehaviourState curState)
    {
        float distanceSqr = entity.HuntPatterns[0].GetDistanceToTargetSqr(entity.Player.position, entity.transform.position);

        if(distanceSqr <= (entity.AttackRange * entity.AttackRange)) {
            return BehaviourState.Attack;
        }

        if(curState == BehaviourState.Attack) {
            entity.AttackPatterns[0].Exit();
        }

        if (distanceSqr <= (entity.ReconizedPlayerRange * entity.ReconizedPlayerRange))
        {
            return BehaviourState.Hunt;
        }

        return BehaviourState.Patrol;
    }
}