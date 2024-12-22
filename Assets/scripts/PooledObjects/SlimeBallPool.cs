using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBallPool : MonoBehaviour
{

    public static SlimeBallPool Instance;

    [SerializeField]
    private GameObject _slimeBallPref;
    [SerializeField]
    private int _slimeBallCount = 30;

    private List<GameObject> _slimeBalls = new();

    // Start is called before the first frame update
    void Awake()
    {
        if (SlimeBallPool.Instance == null)
        {
            SlimeBallPool.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (SlimeBallPool.Instance != null)
        {
            Destroy(this.gameObject);
            return; 
        }
        InstantiateSlimeBalls();
    }

    private void InstantiateSlimeBalls()
    {
        for (int i = 0; i < _slimeBallCount; ++i)
        {
            var slimeball = Instantiate(_slimeBallPref, transform.position, Quaternion.identity);
            slimeball.SetActive(false);
            _slimeBalls.Add(slimeball); 
        }
    }

    public GameObject GetPooledSlimeBall()
    {
        foreach (GameObject g in _slimeBalls)
        {
            if (g.activeInHierarchy)
            {
                return g; 
            }
        }

        Debug.LogError("There is currently no slimeball available, or list is empty.");
        return null; 
    }


}
