using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] StarText starText;
    //public TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI highScoreText;
    public int starCount;






    private void OnEnable()
    {
        Star.OnStarCollected += HandleStarPickup;
    }

    private void OnDisable()
    {
        Star.OnStarCollected -= HandleStarPickup;
    }

    private void Start()
    {
        //  UpdateHighScoreText();
    }

    void HandleStarPickup()
    {
        starCount++;
        starText.IncrementStarCount(starCount);
        CheckHighScore();


        void CheckHighScore()
        {
            if (starCount > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", starCount);


                UpdateHighScoreText();
            }

        }
        void UpdateHighScoreText()
        {
            highScoreText.text = $"HighScore: {PlayerPrefs.GetInt("HighScore", 0)}";
        }

    }
}