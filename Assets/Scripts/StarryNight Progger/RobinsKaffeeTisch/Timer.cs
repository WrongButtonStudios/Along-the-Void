using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices.WindowsRuntime;

public class Timer : MonoBehaviour
{
    public float timeValue = 90;
    public TextMeshProUGUI timerText;
    public GameObject endScreen;
    public GameObject meta;

    public int minutes;
    public int seconds;
    public float milliseconds;

    //Update is called once per frame
    void Update()
    {
        if (timeValue > 0)
        {
            timeValue += Time.deltaTime;
        }
        else
        {
            timeValue = 0;
        }


        DisplayTime(timeValue);

    }

    public void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        minutes = Mathf.FloorToInt(timeToDisplay / 60);
        seconds = Mathf.FloorToInt(timeToDisplay % 60);
        milliseconds = timeToDisplay % 1 * 1000;

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

    }
}