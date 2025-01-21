using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingHuntComponent : MonoBehaviour, IHuntingComponent
{
    private BehaviourStateHandler _entity;

    public void Hunt(Vector3 target)
    {
        Vector2 targetPosWithOffset = (Vector2)target + (Vector2.up * 5);
        Vector2 dir = (targetPosWithOffset - (Vector2)transform.position).normalized;
        _entity.Movement.Move(dir); 
    }

    public float GetDistanceToTargetSqr(Vector2 dest, Vector2 start) 
    {
        float dist = dest.x - start.x;
        return dist * dist; 
    }

    public void Init(BehaviourStateHandler entity)
    {
        _entity = entity; 
    }
}