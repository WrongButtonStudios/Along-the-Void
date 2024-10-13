using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPool : MonoBehaviour
{
    [SerializeField]
    private int _numberOfDialogs;
    [SerializeField]
    private GameObject _dialog;
    public List<GameObject> PooledDialogs = new List<GameObject>();
    public static DialogPool Instance; 
    // Start is called before the first frame update
    void Awake()
    {
        if(DialogPool.Instance != null) 
        {
            Destroy(this.gameObject);
            return; 
        }
        DialogPool.Instance = this;
        DontDestroyOnLoad(this.gameObject);
        GenerateDialoges();
    }

    private void GenerateDialoges() 
    {
        for(int i = 0; i < _numberOfDialogs; ++i) 
        {
            var dialog = Instantiate(_dialog, Vector3.zero, Quaternion.identity);
            PooledDialogs.Add(dialog);
            DontDestroyOnLoad(dialog);
            dialog.SetActive(false);
        } 
            Debug.Log("Cool"); 
    }

    public GameObject GetPooledDialog() 
    {
        foreach(GameObject dialog in PooledDialogs) 
        {
            if(dialog.activeSelf == false) 
            {
                Debug.Log("found dialog succefully"); 
                return dialog; 
            }
        }
        throw new System.Exception("There is no Dialog left"); 
    }
}
