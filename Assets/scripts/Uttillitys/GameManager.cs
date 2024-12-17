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

    public static GameManager Instance;

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


    public void InitializeGame()
    {
        Vector3 playerSpawn = GameObject.FindObjectOfType<PlayerSpawn>().transform.position;
        Instantiate(_playerPref, playerSpawn, Quaternion.identity);
        Instantiate(_enemyPoolPref, transform.position, Quaternion.identity);
        Instantiate(_iceBulletPoolPref, transform.position, Quaternion.identity);
        Instantiate(_slimeBallPoolPref, transform.position, Quaternion.identity);
    }

}
