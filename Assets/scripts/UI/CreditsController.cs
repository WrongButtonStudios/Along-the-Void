using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    [SerializeField] GameObject creditsCanvas;
    [SerializeField] GameObject mainMenu;


    void Update()
    {
        CloseThroughESC();
        CloseThroughLeftClick();
    }

    public void CloseThroughESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            creditsCanvas.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
    public void CloseThroughLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            creditsCanvas.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}
