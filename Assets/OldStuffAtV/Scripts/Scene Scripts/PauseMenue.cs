using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenue : MonoBehaviour
{
    public bool GameIsPaused = false;
    public GameObject PauseMenueUI;

    public GameObject firstMenuButton;
    public GameObject optionsMenu;


    //private void Awake()
    //{
    //    PauseMenueUI.SetActive(false);
    //    Time.timeScale = 1f;
    //    GameIsPaused = false;
    //}
    private void Start()
    {
        PauseMenueUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7) )

        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume ()
    {
        
        Time.timeScale = 1f;
        GameIsPaused = false;

        EventSystem.current.SetSelectedGameObject(null); 
        EventSystem.current.SetSelectedGameObject(firstMenuButton);
        optionsMenu.SetActive(false);
        PauseMenueUI.SetActive(false);
    }

    void Pause()
    {
        PauseMenueUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    //public void optionsOpen()
    //{
    //    optionsMenu.ChangeScene();
    //}
}
