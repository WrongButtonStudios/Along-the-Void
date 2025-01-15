using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuCanvas;
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private PlayerInput _playerInput;

    private InputAction _escape;
    private List<GameObject> _windows = new();

    private void Awake() {
        _escape = _playerInput.actions["Menukey"];
    }

    private void OnEnable() {
        _escape.started += OnPauseMenuPerformed;
    }

    private void OnDisable() {
        _escape.started -= OnPauseMenuPerformed;
    }

    private void Start() {
        _playerInput.actions.FindActionMap("characterController").Enable();
        _playerInput.actions.FindActionMap("Menu").Disable();
        _playerInput.actions.FindActionMap("Menukey").Enable();
    }

    private void OnPauseMenuPerformed(InputAction.CallbackContext context) {
        HandleEscapeKey();
    }

    public void HandleEscapeKey() {
        if(_windows.Count == 0) {
            _pauseMenuCanvas.SetActive(true);
            _windows.Add(_pauseMenuCanvas);
            Time.timeScale = 0;
            _playerInput.actions.FindActionMap("characterController").Disable();
            _playerInput.actions.FindActionMap("Menu").Enable();
            return;
        }
        GameObject windowToClose = _windows[_windows.Count - 1];
        windowToClose.SetActive(false);
        _windows.Remove(windowToClose);
        if(_windows.Count == 0) {
            Time.timeScale = 1;
            _playerInput.actions.FindActionMap("characterController").Enable();
            _playerInput.actions.FindActionMap("Menu").Disable();
            return;
        }
        GameObject windowToOpen = _windows[_windows.Count - 1];
        windowToOpen.SetActive(true);
    }

    public void RestartScene() {
        _windows.Clear();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenMainMenu() {
        _playerInput.SwitchCurrentActionMap("OpenMainMenu");
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void OpenSettings() {
        Debug.Log("test");
        _windows[_windows.Count - 1].SetActive(false);
        _windows.Add(_settingsCanvas);
        _settingsCanvas.SetActive(true);
    }
}