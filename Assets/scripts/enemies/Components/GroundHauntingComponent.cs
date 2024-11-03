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
        Vector2 movementForce = dir * _entity.Speed;
        _entity.RB.AddForce(movementForce * Time.fixedDeltaTime, ForceMode2D.Impulse); 
    }

    public void Init(SimpleAI entity)
    {
        _entity = entity; 
    }
}
