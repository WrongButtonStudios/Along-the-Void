using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsMM : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject meinMenu;

    public GameObject optionsButton;
    public GameObject meinButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateOptions()
    {
        optionsMenu.gameObject.SetActive(true); 
        meinMenu.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsButton);
    }

    public void ActivateMain()
    {
        optionsMenu.gameObject.SetActive(false);
        meinMenu.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(meinButton);
    }
}
