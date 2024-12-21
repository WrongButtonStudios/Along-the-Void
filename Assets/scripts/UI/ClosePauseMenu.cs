using UnityEngine;
using UnityEngine.InputSystem;

public class ClosePauseMenu : MonoBehaviour //Zieht sich Action Maps, aktiviert den Close Pause Menu Button, schliesst PauseMenu und wechselt Action Map zu ingame
{
    private PlayerInput playerInput;
    private InputAction closePauseMenu;
    public GameObject pauseMenu;
    void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        closePauseMenu = playerInput.actions["Pausemenu"];
        playerInput.SwitchCurrentActionMap("PauseMenu");
        closePauseMenu = playerInput.actions["ClosePauseMenu"];
    }
    private void OnEnable()
    {
        playerInput.SwitchCurrentActionMap("PauseMenu");
        closePauseMenu.performed += OnClosePauseMenu;
    }
    private void OnClosePauseMenu(InputAction.CallbackContext context)
    {
        closePauseMenu.performed -= OnClosePauseMenu;
        Debug.Log("Megaschwängelllle");
        playerInput.SwitchCurrentActionMap("characterController");
        this.gameObject.SetActive(false);
    }
}