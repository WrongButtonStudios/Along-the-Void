using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LoadNextScene : MonoBehaviour
{
    [SerializeField]
    private string _targetScene = "bullshit";
    [SerializeField]
    private bool _loadScene = true;
    [SerializeField]
    private Transform _levelStart;
    [SerializeField]
    private Transform _levelEnd;
    private Vector2 _offset;
    private bool _canBeTriggered = false;
    private bool _finnished = false;


    private void Start()
    {
        if(_loadScene)
            _offset = ((Vector2)_levelEnd.position - (Vector2)_levelStart.position) *0.75f;
        StartCoroutine(WaitForActivation()); 
    }

    private void LoadInLevel()
    {
        if (_finnished)
            return;
        _finnished = true; 
        Debug.Log(_targetScene); 
        Debug.Log("Guck mal ich lad ne scene");
        SceneManager.LoadSceneAsync(_targetScene, LoadSceneMode.Additive).completed += operation =>
        {
            Scene scene = SceneManager.GetSceneByName(_targetScene);

            if (scene.IsValid())
            {
                foreach (GameObject obj in scene.GetRootGameObjects())
                {
                    obj.transform.position += (Vector3)_offset;
                }
                Debug.Log($"Scene {_targetScene} verschoben um {_offset}");
            }
        };
    }

    private void UnloadLevel()
    {
        Debug.Log("Guck mal ich entlade ne scene, hihi ich hab entladen gesagt");
        DeactivateOldEnemys(); 
        SceneManager.UnloadSceneAsync(_targetScene);
    }

    private void DeactivateOldEnemys()
    {
        var enemys = GameObject.FindObjectsOfType<SimpleAI>();
        foreach (SimpleAI enemy in enemys)
        {
            if (enemy.Scene.name == _targetScene)
            {
                enemy.gameObject.SetActive(false);
                Debug.Log("Deactivated Enemy from scene: " + enemy.Scene.name); 
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_canBeTriggered)
        {
             if (collision.GetComponent<characterController>())
             {
                 if (_loadScene)
                     LoadInLevel();
                 else
                     UnloadLevel(); 
             }
        }
    }

    IEnumerator WaitForActivation()
    {
        yield return new WaitForSeconds(2);
        _canBeTriggered = true; 
    }
}
