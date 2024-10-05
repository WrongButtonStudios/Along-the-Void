using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void PlayGame()
    {
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