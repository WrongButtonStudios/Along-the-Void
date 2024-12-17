using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class pools any enemy of the Game to recycle them through the Game. This class is used as a singleton with the reference Name "PooledEnemys"
/// </summary>
public class EnemyPool : MonoBehaviour
{
    [Tooltip("Singleton reference of the Enemy pool")]
    public static EnemyPool PooledEnemys;

    [SerializeField, Tooltip("Amount of Ground Closecombat Enemys to Pool")]
    private int _groundCloseCombatEnemyCount = 5;
    [SerializeField, Tooltip("Amount of Ground far combat Enemys to Pool")]
    private int _groundFarCombatEnemyCount = 5;
    [SerializeField, Tooltip("Amount of Flying Enemys to Pool")]
    private int _FlyingEnemyCount = 5;
    [SerializeField, Tooltip("CC = Close Combat")]
    private GameObject _groundCCEnemyPref;
    [SerializeField, Tooltip("FC = Far Combat")]
    private GameObject _groundFCEnemyPref;
    [SerializeField]
    private GameObject _flyingEnemyPref;

    private List<GameObject> _closeCombatGroundEnemys = new List<GameObject>();
    private List<GameObject> _farCombatGroundEnemys = new List<GameObject>();
    private List<GameObject> _flyingEnemys = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        if (EnemyPool.PooledEnemys == null)
        {
            EnemyPool.PooledEnemys = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (EnemyPool.PooledEnemys.gameObject != this.gameObject)
        {
            Destroy(this.gameObject);
        }

        InstantiateEnemys(); 
    }


    private void InstantiateEnemys()
    {
        //if amount of every enemy type is the same, do it in one loop
        if (_groundCloseCombatEnemyCount == _groundFarCombatEnemyCount && _groundFarCombatEnemyCount == _FlyingEnemyCount)
        {
            for (int i = 0; i < _groundCloseCombatEnemyCount; ++i)
            {
                CreateGroundCloseCombatEnemy();
                CreateGroundFarCombatEnemy();
                CreateFlyingEnemy();
            }
        }
        else //other wise create the enemy types in seperate loops
        {
            for (int i = 0; i < _groundCloseCombatEnemyCount; ++i)
            {
                CreateGroundCloseCombatEnemy();
            }

            for (int i = 0; i < _groundFarCombatEnemyCount; ++i)
            {
                CreateGroundFarCombatEnemy();
            }

            for (int i = 0; i < _FlyingEnemyCount; ++i)
            {
                CreateFlyingEnemy();
            }
        }
    }
    /// <summary>
    /// Use this function to get a Close Combat Ground Enemy from the pool
    /// </summary>
    /// <returns>Close Combat Enemy  GameObject if pool isnt empty. If pool is empty, it will return null</returns>
    public GameObject GetPooledCCGEnemy()
    {
        foreach (GameObject g in _closeCombatGroundEnemys)
        {
            if (g.activeInHierarchy == false)
            {
                return g; 
            }
        }

        Debug.LogError("Pool is currently empty");
        return null; 
    }

    /// <summary>
    /// Use this function to get a Far Combat Ground Enemy from the pool
    /// </summary>
    /// <returns>Far Combat Enemy  GameObject if pool isnt empty. If pool is empty, it will return null</returns>
    public GameObject GetPooledFCGEnemy()
    {
        foreach (GameObject g in _farCombatGroundEnemys)
        {
            if (g.activeInHierarchy == false)
            {
                return g;
            }
        }

        Debug.LogError("Pool is currently empty");
        return null;
    }

    /// <summary>
    /// Use this function to get a Flying Enemy from the pool
    /// </summary>
    /// <returns>Flying Enemy GameObject if pool isnt empty. If pool is empty, it will return null</returns>
    public GameObject GetPooledFlyingEnemy()
    {
        foreach (GameObject g in _flyingEnemys)
        {
            if (g.activeInHierarchy == false)
            {
                return g;
            }
        }

        Debug.LogError("Pool is currently empty");
        return null;
    }

    private void CreateGroundCloseCombatEnemy()
    {
        var GCCEnemy = Instantiate(_groundCCEnemyPref, transform.position, Quaternion.identity);
        GCCEnemy.SetActive(false);
        _closeCombatGroundEnemys.Add(GCCEnemy);
    }

    private void CreateGroundFarCombatEnemy()
    {
        var GFCEnemy = Instantiate(_groundFCEnemyPref, transform.position, Quaternion.identity);
        GFCEnemy.SetActive(false);
        _farCombatGroundEnemys.Add(GFCEnemy);
    }

    private void CreateFlyingEnemy()
    {
        var FlyingEnemy = Instantiate(_flyingEnemyPref, transform.position, Quaternion.identity);
        FlyingEnemy.SetActive(false);
        _flyingEnemys.Add(FlyingEnemy);
    }


}
