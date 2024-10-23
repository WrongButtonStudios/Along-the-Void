using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllDialogSpawn : MonoBehaviour
{
    bool DialogShown = false;
    public List<GameObject> spawns;
    [SerializeField]
    private List<string> _dialogs = new List<string>();
    private GameObject _dialog;
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !DialogShown) 
        {
            SpawnDialog(spawns[0].transform.position + (Vector3.up * 2));
            _dialog.SetActive(true);
            return; 

        }
        if (Input.GetKeyDown(KeyCode.A) && DialogShown)
        {
            DialogShown = false;
            _dialog.SetActive(false);
            _dialog = null;
            return;
        }

        if (Input.GetKeyDown(KeyCode.B) && !DialogShown)
        {
            SpawnDialog(spawns[1].transform.position + (Vector3.up * 2));
            _dialog.SetActive(true);
            return; 
        }
        if (Input.GetKeyDown(KeyCode.B) && DialogShown)
        {
            DialogShown = false;
            _dialog.SetActive(false);
            _dialog = null;
            return;
        }

        if (Input.GetKeyDown(KeyCode.C) && !DialogShown)
        {
            SpawnDialog(spawns[2].transform.position + (spawns[2].transform.up * 2));
            _dialog.SetActive(true);
            return;
        }
        if (Input.GetKeyDown(KeyCode.C) && DialogShown)
        {
            DialogShown = false;
            _dialog.SetActive(false);
            _dialog = null;
            return;
        }
    }

    void SpawnDialog(Vector3 pos)
    {
        if (_dialog != null && _dialog.activeInHierarchy == false)
        {
            Debug.LogError("Deactivated shit"); 
            _dialog.SetActive(false);
        }
        _dialog = DialogPool.Instance.GetPooledDialog();
        _dialog.transform.position = pos;
        _dialog.GetComponentInChildren<VisuellNovellFeature>().StartDialog(_dialogs); 
        DialogShown = true;
        Debug.Log($"_dialog spawned at {pos}.");

    }

}
