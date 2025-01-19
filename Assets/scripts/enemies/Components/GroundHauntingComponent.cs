using UnityEngine;

public class GroundHauntingComponent : MonoBehaviour, IHauntingComponent
{
    private SimpleAI _entity;
    
    public float GetDistanceToTargetSqr(Vector2 dest, Vector2 start)
    {
        return (dest - start).sqrMagnitude; 
    }

    public void Haunt(Vector3 target)
    {
        Vector2 dir = (target - transform.position).normalized;
        _entity.Movement.Move(dir); 
    }

    public void Init(SimpleAI entity)
    {
        _entity = entity; 
    }
}
