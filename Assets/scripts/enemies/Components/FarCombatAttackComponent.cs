using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarCombatAttackComponent : MonoBehaviour, IAttackComponent
{
    private SimpleAI _entity;

    public FarCombatAttackComponent(SimpleAI entity)
    {
        _entity = entity;
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }
}
