using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.InteropServices.WindowsRuntime;

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI starsScore;



    private void Start()
    {
        starsScore.text = StateName.stars;
        difficultyText.text = StateName.difficulty;
    }
    private void Update()
    {

    }
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Replay()
    {
        SceneManager.LoadScene("TestLevel02");
    }

    public void DisplayTime(float timeToDisplay)
    {
        //if (timeToDisplay < 0)
        //{
        //    timeToDisplay = 0;
        //}

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = timeToDisplay % 1 * 1000;


        //StateName.difficulty = difficultyText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        difficultyText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        //timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

    }
}