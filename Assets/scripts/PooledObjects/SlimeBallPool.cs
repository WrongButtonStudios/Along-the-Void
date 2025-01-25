using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBallPool : MonoBehaviour
{

    public static SlimeBallPool Instance; 

    [SerializeField]
    private GameObject _slimeBallPref;
    private List<GameObject> _inactiveSlimeball = new();
    private void Awake()
    {
        if(Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(this); 
        } else 
        {
            Destroy(this); 
        }
    }
    public GameObject GetPooledSlimeBall()
    {
        GameObject slimeball = null; 
        if(_inactiveSlimeball.Count == 0) 
        {
             slimeball = Instantiate(_slimeBallPref, transform.position, Quaternion.identity);
            slimeball.SetActive(false);
            return slimeball; 
        }
        slimeball = _inactiveSlimeball[0]; 
        _inactiveSlimeball.Remove(slimeball);
        return slimeball; 
    }

    public void DeactivateSlimeball(GameObject slimeball) 
    {
        slimeball.GetComponent<MoveSlimeball>().Deactivate(); 
        _inactiveSlimeball.Add(slimeball.gameObject);
        slimeball.gameObject.SetActive(false); 
    }
}