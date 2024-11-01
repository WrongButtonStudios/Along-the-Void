using UnityEngine;

public class CloseCombatAttackComponent : MonoBehaviour, IAttackComponent
{
    private SimpleAI _entity;

    public CloseCombatAttackComponent(SimpleAI entity)
    {
        _entity = entity;
    }
    public void Attack()
    {
        throw new System.NotImplementedException();
    }
}
