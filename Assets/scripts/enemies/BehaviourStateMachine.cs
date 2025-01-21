using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourStateMachine : MonoBehaviour
{
    public static State UpdateState(BehaviourStateHandler entity, State curState)
    {
        float distanceSqr = entity.HauntingComponents[0].GetDistanceToTargetSqr(entity.PlayerPos, entity.transform.position);
        if (distanceSqr < (entity.ReconizedPlayerRange * entity.ReconizedPlayerRange) && distanceSqr > (entity.AttackRange * entity.AttackRange) && curState != State.Hunt)
        {
            ExitState(entity, curState);
            return State.Hunt; 
        }

        if (entity.HauntingComponents[0].GetDistanceToTargetSqr(entity.PlayerPos, entity.transform.position) < (entity.AttackRange * entity.AttackRange) && curState != State.Attack)
        {
            ExitState(entity, curState);
            return State.Attack; 
        }

        if (entity.HauntingComponents[0].GetDistanceToTargetSqr(entity.PlayerPos, entity.transform.position) > (entity.ReconizedPlayerRange * entity.ReconizedPlayerRange) && curState != State.Patrol)
        {
            ExitState(entity, curState);
            return State.Patrol;
        }
        return curState; 
    }

    private static void ExitState(BehaviourStateHandler entity, State curState)
    {
        if (curState == State.Attack)
            entity.AttackComponents[0].Exit();
    }
}
