using UnityEngine;

[System.Serializable]
public class GroundHuntingComponent : HuntingComponent
{
    [SerializeField] private EnemyMovement _movement; 
    [SerializeField] private Transform _player;
    
    public override float GetDistanceToTargetSqr(Vector2 dest, Vector2 start)
    {
        return (dest - start).sqrMagnitude; 
    }

    public override void Hunt()
    {
        _movement.Move(_player.position - transform.position); 
    }
}