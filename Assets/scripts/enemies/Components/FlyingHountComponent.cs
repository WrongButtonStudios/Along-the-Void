using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingHauntComponent : MonoBehaviour, IHauntingComponent
{
    private SimpleAI _entity;
    private EnemyMovement _movement; 
    private void Start()
    {
        _movement = this.GetComponent<EnemyMovement>();    
    }

    public void Haunt(Vector3 target)
    {
        Vector2 targetPosWithOffset = (Vector2)target + (Vector2.up * 5);
        Vector2 dir = (targetPosWithOffset - (Vector2)transform.position).normalized;
        _movement.Move(dir); 
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


