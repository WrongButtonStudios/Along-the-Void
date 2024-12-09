using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedMonitor : MonoBehaviour
{
    public GameObject mainMenu, options, graphicSettings, credits, loadGame;
    public GameObject mainMenuFirstButton, optionsFirstButton, graphicFirstButton, creditsFirstButton, loadFirstButton;




    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
    }

    public void OpenMainMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
    }
    public void OpenOptions()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }
    public void OpenGraphicSettings()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(graphicFirstButton);
    }
    public void OpenCredits()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsFirstButton);
    }
    public void OpenLoadGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(loadFirstButton);
    }
}
