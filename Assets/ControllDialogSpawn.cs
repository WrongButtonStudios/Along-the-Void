using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllDialogSpawn : MonoBehaviour
{
    bool DialogShown = false;
    public List<GameObject> spawns;
    GameObject Dialog;
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !DialogShown) 
        {
            SpawnDialog(spawns[0].transform.position + (Vector3.up * 2));
            Dialog.SetActive(true);
            return; 

        }
        if (Input.GetKeyDown(KeyCode.A) && DialogShown)
        {
            DialogShown = false;
            Dialog.SetActive(false);
            Dialog = null;
            return;
        }

        if (Input.GetKeyDown(KeyCode.B) && !DialogShown)
        {
            SpawnDialog(spawns[1].transform.position + (Vector3.up * 2));
            Dialog.SetActive(true);
            return; 
        }
        if (Input.GetKeyDown(KeyCode.B) && DialogShown)
        {
            DialogShown = false;
            Dialog.SetActive(false);
            Dialog = null;
            return;
        }

        if (Input.GetKeyDown(KeyCode.C) && !DialogShown)
        {
            SpawnDialog(spawns[2].transform.position + (spawns[2].transform.up * 2));
            Dialog.SetActive(true);
            return;
        }
        if (Input.GetKeyDown(KeyCode.C) && DialogShown)
        {
            DialogShown = false;
            Dialog.SetActive(false);
            Dialog = null;
            return;
        }
    }

    void SpawnDialog(Vector3 pos)
    {
        if (Dialog != null && Dialog.activeInHierarchy == false)
        {
            Debug.LogError("Deactivated shit"); 
            //Dialog.SetActive(false);
        }
        Dialog = DialogPool.Instance.GetPooledDialog();
        Dialog.transform.position = pos;
        Dialog.GetComponentInChildren<VisuellNovellFeature>().StartDialog(); 
        DialogShown = true;
        Debug.Log($"Dialog spawned at {pos}.");

    }

}
