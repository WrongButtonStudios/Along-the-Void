using UnityEngine;

public class GroundHuntingComponent : MonoBehaviour, IHuntingComponent
{
    private BehaviourStateHandler _entity;
    
    public float GetDistanceToTargetSqr(Vector2 dest, Vector2 start)
    {
        return (dest - start).sqrMagnitude; 
    }

    public void Hunt(Vector3 target)
    {
        Vector2 dir = (target - transform.position).normalized;
        _entity.Movement.Move(dir); 
    }

    public void Init(BehaviourStateHandler entity)
    {
        _entity = entity; 
    }
}
