using UnityEngine;

public class CloseCombatAttackComponent : MonoBehaviour, IAttackComponent
{
    private SimpleAI _entity;

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Init(SimpleAI entity)
    {
        _entity = entity; 
    }
}
