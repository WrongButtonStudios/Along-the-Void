using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndLevel : MonoBehaviour
{
    public Timer timer;
    public GameObject timeScore;
    public GameManager gameManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && timer.timeValue < 420f)
        {
            StateName.stars = gameManager.starCount.ToString();
            StateName.difficulty = timer.timeValue.ToString();
            StateName.difficulty = string.Format("{0:00}:{1:00}:{2:000}", timer.minutes, timer.seconds, timer.milliseconds);
            SceneManager.LoadScene("EndScreen");
        }

        if (collision.transform.CompareTag("Player") && timer.timeValue >= 420f)
        {
            SceneManager.LoadScene("GameOver");
        }
    }




}