using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingHauntComponent : MonoBehaviour, IHauntingComponent
{
    private SimpleAI _entity; 

    public FlyingHauntComponent(SimpleAI entity) 
    {
        _entity = entity;
    }
    public void Haunt(System.Numerics.Vector3 target)
    {
        throw new System.NotImplementedException();
    }
}
