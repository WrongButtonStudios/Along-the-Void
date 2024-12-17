using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    private PlayerInput playerInput;
    private void Start()
    {
        SwitchToMenu(); //Changes current Action Map to PauseMenu action map        
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
        playerInput = GetComponent<PlayerInput>(); //Gets Action Maps
        SwitchToIngame(); //Switches current Action Map to characterController on game start
    }

    public void PlayCredits()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
    private void SwitchToMenu()
    {
        playerInput.SwitchCurrentActionMap("PauseMenu");
    }
    private void SwitchToIngame()
    {
        playerInput.SwitchCurrentActionMap("characterController");
    }
}