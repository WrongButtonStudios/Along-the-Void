using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _wayPoints = new();
    [SerializeField]
    private bool _forFlyingEnemy = false;

    public List<Transform> WayPoints { get { return _wayPoints; } }
    public bool ForFlyingEnemy { get { return _forFlyingEnemy;  } }
}
