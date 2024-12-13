using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LoadNextScene : MonoBehaviour
{
    [SerializeField]
    private string _sceneToLoad = "bullshit";
    [SerializeField]
    private bool _loadScene = true;
    [SerializeField]
    private Transform _levelStart;
    [SerializeField]
    private Transform _levelEnd;
    private Vector2 _offset; 

    private void Start()
    {
        if(_loadScene)
            _offset = ((Vector2)_levelEnd.position - (Vector2)_levelStart.position)/2; 
    }

    private void LoadInLevel()
    {
        Debug.Log(_sceneToLoad); 
        Debug.Log("Guck mal ich lad ne scene");
        SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive).completed += operation =>
        {
            Scene loadedScene = SceneManager.GetSceneByName(_sceneToLoad);

            if (loadedScene.IsValid())
            {
                foreach (GameObject obj in loadedScene.GetRootGameObjects())
                {
                    obj.transform.position += (Vector3)_offset;
                }
                Debug.Log($"Scene {_sceneToLoad} verschoben um {_offset}");
            }
        };
    }

    private void UnloadLevel()
    {
        Debug.Log("Guck mal ich entlade ne scene, hihi ich hab entladen gesagt");
        SceneManager.UnloadSceneAsync(_sceneToLoad); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<characterController>())
        {
            if (_loadScene)
                LoadInLevel();
            else
                //UnloadLevel(); 
        }
    }
}
