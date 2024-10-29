
using System.Numerics;

public interface IAttackComponent 
{
    public void Attack(); 
}

public interface IMovementComponent 
{
    public void Movement(); 
}

public interface IHauntingComponent 
{
    public void Haunt(Vector3 target);
}
