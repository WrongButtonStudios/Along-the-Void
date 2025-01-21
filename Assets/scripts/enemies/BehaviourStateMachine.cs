using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourStateMachine : MonoBehaviour
{
    public static State UpdateState(BehaviourStateHandler entity, State curState)
    {
        float distanceSqr = entity.HauntingComponents[0].GetDistanceToTargetSqr(entity.PlayerPos, entity.transform.position);

        if(distanceSqr <= (entity.AttackRange * entity.AttackRange)) {
            return State.Attack;
        }

        if(curState == State.Attack) {
            entity.AttackComponents[0].Exit();
        }

        if (distanceSqr <= (entity.ReconizedPlayerRange * entity.ReconizedPlayerRange))
        {
            return State.Hunt;
        }

        return State.Patrol;
    }
}