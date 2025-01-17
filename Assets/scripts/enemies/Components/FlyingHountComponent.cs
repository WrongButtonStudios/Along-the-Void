using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingHauntComponent : MonoBehaviour, IHauntingComponent
{
    private SimpleAI _entity; 

    public void Haunt(Vector3 target)
    {
        Vector2 targetPosWithOffset = (Vector2)target + (Vector2.up * 5);
        Vector2 dir = (targetPosWithOffset - (Vector2)transform.position).normalized;
        Vector2 movementVelocity = dir * _entity.Speed * (Time.fixedDeltaTime * _entity.TimeScale);
        _entity.RB.velocity += movementVelocity; 
    }

    public float GetDistanceToTargetSqr(Vector2 dest, Vector2 start) 
    {
        float dist = dest.x - start.x;
        return dist * dist; 
    }

    public void Init(SimpleAI entity)
    {
        _entity = entity; 
    }
}


