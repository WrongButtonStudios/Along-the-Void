using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int _sceneIndexToLoad = 1;
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject optionsCanvas;
    [SerializeField] GameObject graphicsCanvas;
    [SerializeField] GameObject creditsCanvas;
    private PlayerInput playerInput;
    private InputAction cancel;
    private List<GameObject> menuWindows = new();
    private void Start()
    {
        CleanupInput();
        playerInput = FindObjectOfType<PlayerInput>();
        playerInput.SwitchCurrentActionMap("MainMenu");
        cancel = playerInput.actions["Cancel"];
        cancel.performed += OnCancelPerformed;
    }

    private void OnDestroy()
    {
        CleanupInput();
    }
    private void CleanupInput()
    {
        if (playerInput != null)
        {
            var menuActionMap = playerInput.actions.FindActionMap("Menu", true);
            if (menuActionMap != null)
            {
                if (menuActionMap.enabled)
                {
                    Debug.Log("Disabling Menu action map to prevent leaks.");
                    menuActionMap.Disable();

                }
            }
        }
    }
    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        if (menuWindows.Count > 1) // >1 means 2 or more are open so full program - Close current, remove current from list, reload list, open new current window.
        {
            GameObject currentMenu = menuWindows[menuWindows.Count - 1];
            currentMenu.SetActive(false);
            menuWindows.Remove(currentMenu);

            menuWindows[menuWindows.Count - 1].SetActive(true);
        }
        if (menuWindows.Count == 1) // >1 means 2 or more are open so full program - Close current, remove current from list, reload list, open new current window.
        {
            GameObject currentMenu = menuWindows[menuWindows.Count - 1];
            currentMenu.SetActive(false);
            mainMenuCanvas.SetActive(true);
        }
    }
    public void Options()
    {
        menuWindows.Add(optionsCanvas);
        optionsCanvas.SetActive(true);
    }
    public void Graphics()
    {
        menuWindows.Add(graphicsCanvas);
        graphicsCanvas.SetActive(true);
    }
    public void Credits()
    {
        menuWindows.Add(creditsCanvas);
        creditsCanvas.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(_sceneIndexToLoad);
        Destroy(this);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}