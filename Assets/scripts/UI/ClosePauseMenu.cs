using UnityEngine;
using UnityEngine.InputSystem;

public class ClosePauseMenu : MonoBehaviour //Zieht sich Action Maps, aktiviert den Close Pause Menu Button, schliesst PauseMenu und wechselt Action Map zu ingame
{
    public @BaseInputActions pauseMenu;
    private PlayerInput playerInput;
    void Start()
    {
        playerInput.SwitchCurrentActionMap("PauseMenu");
        pauseMenu = new @BaseInputActions();
        pauseMenu.PauseMenu.ClosePauseMenu.performed += ctx => CloseePauseMenu();
        pauseMenu.PauseMenu.Enable();
        playerInput = GetComponent<PlayerInput>();
    }

    private void CloseePauseMenu()
    {
        pauseMenu.PauseMenu.ClosePauseMenu.performed -= ctx => CloseePauseMenu();
        pauseMenu.PauseMenu.Disable();
        playerInput.SwitchCurrentActionMap("characterController");
        this.gameObject.SetActive(false);
    }
}
