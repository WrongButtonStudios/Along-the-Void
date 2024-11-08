
using UnityEngine;

public interface IComponent
{
    public void Init(SimpleAI entity); 
} 
public interface IAttackComponent : IComponent 
{
    public void Attack();
    public bool FinnishedAttack();
    public void ResetAttackStatus(); 
}

public interface IPatrolComponent : IComponent
{
    public void Patrol();

    public void SetUpNewWayPoint();
    public float GetXDirection();

    public void LookAtTarget();

    public int GetNextWayPoint();

    public void Movement(Vector2 target);

    public bool ReachedDestination();

    // auf klo
}

public interface IHauntingComponent : IComponent
{
    public void Haunt(Vector3 target);

    public float GetDistanceToTargetSqr(Vector2 dest, Vector2 start); 
}
