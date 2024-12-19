using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private int _sceneIndexToLoad = 1; 
    private PlayerInput playerInput;
    private void Start()
    {
        SwitchToMenu(); //Changes current Action Map to PauseMenu action map        
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(_sceneIndexToLoad);
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