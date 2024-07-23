using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject firstMenuButton;
    public OptionsMenu optionsMenu;
    public bool selle = true;

    private void Start()
    {
        //ChangeScene();
    }

    public void Update()
    {
        //ChangeScene();
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    //public void ChangeScene()
    //{
    //    if (selle == true)
    //    {
    //        EventSystem.current.SetSelectedGameObject(null);
    //        EventSystem.current.SetSelectedGameObject(firstMenuButton);
    //        selle = false;
    //        optionsMenu.solle = true;
    //    }


    //}

    
}
