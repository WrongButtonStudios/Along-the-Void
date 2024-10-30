
using UnityEngine;

public interface IAttackComponent 
{
    public void Attack(); 
}

public interface IPatrolComponent 
{
    public void Patrol();

    public void SetUpNewWayPoint();
    public float GetXDirection();

    public void LookAtTarget();

    public int GetNextWayPoint();

    public void Movement(Vector2 target); 
}

public interface IHauntingComponent 
{
    public void Haunt(Vector3 target);

    public float GetDistanceToTargetSqr(Vector2 dest, Vector2 start); 
}
