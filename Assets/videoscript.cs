using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class videoscript : MonoBehaviour
{

    public VideoPlayer video;
    public bool isPlaying;
    public bool isPaused = false;
    public GameObject videoBackground;


    void Start()
    {
        //video = GetComponent<VideoPlayer>();
        video.Play();
        StartCoroutine("WaitForMovieEnd");
        isPlaying = true;
    }

    //private void Update()
    //{
    //    if (!video.isPlaying)
    //    {
    //        OnMovieEnded();
    //    }
    //}

    public IEnumerator WaitForMovieEnd()
    {

        Debug.Log("Heyhohey");
        yield return new WaitForSeconds(11);
        isPaused = true;
        Debug.Log("Pauseses");
        OnMovieEnded();

    }

    void OnMovieEnded()
    {
        if (isPaused)
        {
            videoBackground.gameObject.SetActive(false);
        }

    }
}