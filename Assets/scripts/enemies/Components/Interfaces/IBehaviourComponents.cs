
using UnityEngine;
using System.Collections.Generic; 
public interface IComponent
{
    public void Init(BehaviourStateHandler entity); 
} 
public interface IAttackComponent : IComponent 
{
    public void Attack();
    public bool FinnishedAttack();
    public bool IsAttacking(); 
    public void ResetAttackStatus();
    public void Exit(); 
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

    public void SetWayPoints(List<Transform> wps); 
 
}

public interface IHuntingComponent : IComponent
{
    public void Hunt(Vector3 target);

    public float GetDistanceToTargetSqr(Vector2 dest, Vector2 start); 
}
