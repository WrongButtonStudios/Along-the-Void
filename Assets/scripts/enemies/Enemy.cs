using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private BehaviourStateHandler _behaviorStateHandler;
    [SerializeField] private BehaviorStateMachine _behaviourStateMachine;
    [SerializeField] private DebuffStateHandler _debuffStateHandler;
    [SerializeField] private DebuffStateMachine _debuffStateMachine;

    public Health Health {get => _health;private set => _health = value;}
    public DebuffStateHandler Debuffs {get => _debuffStateMachine; private set => _debuffStateMachine = value;}
    public BehaviorStateMachine Behaviour {get => _behaviour; private set => _behaviour = value;}
    private void Update()
    {
        _debuffStateHandler.HandleDebuffs(_health, _debuff);
        _behaviorStateHandler.HandleState();
    }
}