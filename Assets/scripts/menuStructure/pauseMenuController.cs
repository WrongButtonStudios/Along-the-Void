using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenuController : MonoBehaviour
{
    bool pauseMenuActive = false;
    public GameObject pauseMenuCanvas;




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {


            if (!pauseMenuActive)
            {
                OpenPauseMenu();
            }
            else
            {
                ClosePauseMenu();
            }
        }
    }

    void OpenPauseMenu()
    {
        pauseMenuCanvas.SetActive(true);
        pauseMenuActive = true;
        Time.timeScale = 0;
    }

    void ClosePauseMenu()
    {
        pauseMenuCanvas.SetActive(false);
        pauseMenuActive = false;
        Time.timeScale = 1;
    }
}
