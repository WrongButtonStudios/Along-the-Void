using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadSceneAsync(1);
    }

    public void NewGame()
    {

    }

    public void Options()
    {

    }

    public void PlayCredits()
    {

    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}