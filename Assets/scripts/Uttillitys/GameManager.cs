using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
/// <summary>
/// Singleton class, that loads in and manages all kinds of Prefarbs 
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPref;
    [SerializeField]
    private GameObject _enemyPoolPref;
    [SerializeField]
    private GameObject _iceBulletPoolPref;
    [SerializeField]
    private GameObject _slimeBallPoolPref;
    private bool _initizedGame = false; 
    public static GameManager Instance;
    private Scene _activeScene; 
    
    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            GameManager.Instance = this;
        }
        else if(GameManager.Instance.gameObject != this.gameObject)
        {
            Destroy(this.gameObject); 
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        _activeScene = scene;
        Debug.Log(_activeScene.name); 
        if (!_initizedGame && SceneManagementUttillitys.SceneNameContains(scene, "Level"))
        {
            InitializeGame();
            //SetUpEnemys(); 
        } else if (SceneManagementUttillitys.SceneNameContains(scene, "Level"))
        {
            Debug.Log("Set up enemys for next level...");
            //SetUpEnemys();
        }
        Debug.Log("Scene loaded..."); 
    }

    private void SetUpEnemys()
    {
        var spawns =FindObjectsByType<EnemySpawn>(FindObjectsSortMode.None);
        foreach (EnemySpawn spawn in spawns)
        {
            if (SceneManagementUttillitys.CompareScene(spawn.gameObject.scene, _activeScene))
            {
                CreateAndPlaceEnemy(spawn);
                Debug.Log("Placed enemy..."); 
            }
        }
    }

    private void CreateAndPlaceEnemy(EnemySpawn spawn)
    {
        GameObject enemyToPlace = null;
        if (spawn.ForFlyingEnemy)
            enemyToPlace = EnemyPool.Instance.GetPooledFlyingEnemy();
        else
        {
            int rand = Random.Range(0, 1);
            if (rand == 0)
                enemyToPlace = EnemyPool.Instance.GetPooledCCGEnemy();
            else
                enemyToPlace = EnemyPool.Instance.GetPooledFCGEnemy();
        }
        enemyToPlace.GetComponent<BehaviourStateHandler>().SetScene(_activeScene); 
        enemyToPlace.transform.position = spawn.transform.position;
        Debug.Log(enemyToPlace.transform.position); 
        enemyToPlace.SetActive(true); 
    }

    public void InitializeGame()
    {
        _initizedGame = true;
        Vector3 playerSpawn = GameObject.FindObjectOfType<PlayerSpawn>().transform.position;
        Instantiate(_playerPref, playerSpawn, Quaternion.identity);
        Instantiate(_enemyPoolPref, transform.position, Quaternion.identity);
        Instantiate(_iceBulletPoolPref, transform.position, Quaternion.identity);
        Instantiate(_slimeBallPoolPref, transform.position, Quaternion.identity);
    }

    private IEnumerator SetUpEnemysWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SetUpEnemys(); 
    }

}
