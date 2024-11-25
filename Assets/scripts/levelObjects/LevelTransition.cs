using UnityEngine.SceneManagement; 
using UnityEngine;
using System.Collections.Generic;
using System.Collections; 

public class LevelTransition : MonoBehaviour
{
    [SerializeField]
    private string _levelName;
    [SerializeField]
    private bool _hasStoryBetween = false;
    [SerializeField]
    private VisuellNovellFeature _visualNovel;
    [SerializeField]
    private List<string> _dialogs; 

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!_hasStoryBetween)
                SceneManager.LoadScene(_levelName);
            Instantiate(_visualNovel.gameObject, transform.position, Quaternion.identity);
            _visualNovel.StartDialog(_dialogs);
            StartCoroutine(PlayerDialog()); 
        }

    }

    private IEnumerator PlayerDialog()
    {
        while (_visualNovel.FinnishedDialog() == false)
        {
            yield return null;
        }

        SceneManager.LoadScene(_levelName);
    }
}
