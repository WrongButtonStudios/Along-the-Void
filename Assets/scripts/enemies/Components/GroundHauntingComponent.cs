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
        Vector2 movementVel = dir.normalized * _entity.Speed * (Time.fixedDeltaTime * _entity.TimeScale);
        _entity.RB.velocity += movementVel; 
    }

    public void Init(SimpleAI entity)
    {
        _entity = entity; 
    }
}
