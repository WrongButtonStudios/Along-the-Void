using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject pauseMenu;

    public GameObject optionsButton;
    public GameObject pauseButton;  

    void Update()
    {
        
    }

   public void ActivateOptions()
    {
        optionsMenu.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsButton);
    }

    public void ActivatePause()
    {
        optionsMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseButton);
    }
}
