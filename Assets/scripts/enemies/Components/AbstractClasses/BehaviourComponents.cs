
using UnityEngine;

public abstract class AttackComponent : MonoBehaviour
{
    public abstract void Attack();
    public abstract bool FinnishedAttack();
    public abstract bool IsAttacking(); 
    public abstract void ResetAttackStatus();
    public abstract void Exit(); 
}

public abstract class PatrolComponent : MonoBehaviour
{
    public abstract void Patrol();
    public abstract void SetUpNewWayPoint();
    public abstract float GetXDirection();
    public abstract void LookAtTarget();
    public abstract int GetNextWayPoint();
    public abstract void Movement(Vector2 target);
    public abstract bool ReachedDestination(); 
}

public abstract class HuntingComponent : MonoBehaviour
{
    public abstract void Hunt();
    public abstract float GetDistanceToTargetSqr(Vector2 dest, Vector2 start); 
}
