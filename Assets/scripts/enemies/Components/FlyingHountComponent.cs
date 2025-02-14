using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlyingHuntComponent : HuntingComponent
{
    [SerializeField] private EnemyMovement _movement; 
    [SerializeField] private Transform _player;

    public override void Hunt()
    {
        _movement.Move(_movement.CalculateDirection(transform.position, _player.position, 5f)); 
    }

    public override float GetDistanceToTargetSqr(Vector2 dest, Vector2 start) 
    {
        float dist = dest.x - start.x;
        return dist * dist; 
    }
}