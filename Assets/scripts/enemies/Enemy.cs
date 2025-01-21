using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private BehaviourStateHandler _behaviorStateHandler;
    [SerializeField] private BehaviourStateMachine _behaviourStateMachine;
    [SerializeField] private DebuffStateHandler _debuffStateHandler;
    [SerializeField] private DebuffStateMachine _debuffStateMachine;

    public Health Health {get => _health;private set => _health = value;}
    public DebuffStateMachine Debuffs {get => _debuffStateMachine; private set => _debuffStateMachine = value;}
    public BehaviourStateMachine Behaviour {get => _behaviourStateMachine; private set => _behaviourStateMachine = value;}
    private void Update()
    {
        Debuffs debuffs = _debuffStateMachine.Debuffs;
        _debuffStateHandler.HandleDebuffs(_health,debuffs);
    }
}