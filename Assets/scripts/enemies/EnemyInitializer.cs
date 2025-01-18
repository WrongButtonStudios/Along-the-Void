using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    GroundEnemy,
    FlyingEnemy
}

public enum WeaponsAttached
{
    CloseCombat,
    FarCombat
}

public class EnemyInitializer : MonoBehaviour
{
    public static void AddPatrolAndHauntComponent(SimpleAI aI, List<Transform> wayPoints, List<EnemyType> types)
    {
        foreach (EnemyType type in types)
        {
            switch (type)
            {
                case EnemyType.FlyingEnemy:
                    var FlyingHaunting = aI.gameObject.AddComponent<FlyingHauntComponent>();
                    FlyingHaunting.Init(aI);
                    aI.HauntingComponents.Add(FlyingHaunting);
                    var FlyingPatrol = aI.gameObject.AddComponent<FlyingPatrolComponent>();
                    FlyingPatrol.Init(aI);
                    aI.PatrolComponents.Add(FlyingPatrol);
                    FlyingPatrol.SetWayPoints(wayPoints);
                    break;
                case EnemyType.GroundEnemy:
                    var groundHaunting = aI.gameObject.AddComponent<GroundHauntingComponent>();
                    groundHaunting.Init(aI);
                    aI.HauntingComponents.Add(groundHaunting);
                    var groundPatrol = aI.gameObject.AddComponent<GroundPatrolComponent>();
                    groundPatrol.Init(aI);
                    aI.PatrolComponents.Add(groundPatrol);
                    groundPatrol.SetWayPoints(wayPoints);
                    break;
            }

        }
    }

    public static void AddAttackComponent(SimpleAI aI, List<WeaponsAttached> weapons)
    {
        foreach (WeaponsAttached weapon in weapons)
        {
            switch (weapon)
            {
                case WeaponsAttached.CloseCombat:
                    var closeCombat = aI.gameObject.AddComponent<CloseCombatAttackComponent>();
                    closeCombat.Init(aI);
                    aI.AttackComponents.Add(closeCombat);
                    break;
                case WeaponsAttached.FarCombat:
                    var farCombat = aI.gameObject.AddComponent<FarCombatAttackComponent>();
                    farCombat.Init(aI);
                    aI.AttackComponents.Add(farCombat);
                    break;
            }
        }
    }
}
