using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int _sceneIndexToLoad = 1;
    [SerializeField] GameObject mainMenuCanvas; // warum namenskonvention nicht eingehalten
    [SerializeField] GameObject optionsCanvas; // warum namenskonvention nicht eingehalten
    [SerializeField] GameObject graphicsCanvas; // warum namenskonvention nicht eingehalten
    [SerializeField] GameObject creditsCanvas; // warum namenskonvention nicht eingehalten

    private PlayerInput playerInput; // warum namenskonvention nicht eingehalten
    private InputAction cancel; // warum namenskonvention nicht eingehalten
    private List<GameObject> menuWindows = new(); // warum namenskonvention nicht eingehalten

    private void Start() {
        CleanupInput(); // warum brauche ich das hier?
        playerInput = FindObjectOfType<PlayerInput>(); // find funktionen sind inperformant, semi no-go
        playerInput.SwitchCurrentActionMap("OpenMainMenu"); // gibt es besser möglichkeiten als über einen string?
        cancel = playerInput.actions["Cancel"]; // warum manuell?
        cancel.performed += OnCancelPerformed; // events lernen
    }

    private void OnDestroy() {
        CleanupInput(); // kann weg
    }

    private void CleanupInput() { // kann weg
        if(playerInput != null) {
            var menuActionMap = playerInput.actions.FindActionMap("Menu",true);
            if(menuActionMap != null) {
                if(menuActionMap.enabled) {
                    Debug.Log("Disabling Menu action map to prevent leaks.");
                    menuActionMap.Disable();
                }
            }
        }
    }

    private void OnCancelPerformed(InputAction.CallbackContext context) // keine sinnvolle benennenung
    {
        if(menuWindows.Count > 1) // >1 means 2 or more are open so full program - Close current, remove current from list, reload list, open new current window.
        {
            GameObject currentMenu = menuWindows[menuWindows.Count - 1];
            currentMenu.SetActive(false);
            menuWindows.Remove(currentMenu);

            menuWindows[menuWindows.Count - 1].SetActive(true);
        }
        if(menuWindows.Count == 1) // >1 means 2 or more are open so full program - Close current, remove current from list, reload list, open new current window.
        {
            GameObject currentMenu = menuWindows[menuWindows.Count - 1];
            currentMenu.SetActive(false);
            mainMenuCanvas.SetActive(true);
        }
    }

    public void Options() // keine sinnvolle benennenung
    {
        menuWindows.Add(optionsCanvas);
        optionsCanvas.SetActive(true);
        // warum geht main menu nicht zu?
    }

    public void Graphics() // keine sinnvolle benennenung
    {
        menuWindows.Add(graphicsCanvas);
        graphicsCanvas.SetActive(true);
        // warum geht main menu nicht zu?
    }






}