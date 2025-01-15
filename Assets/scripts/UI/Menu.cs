using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _creditsCanvas;
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
        if(SceneManager.GetActiveScene().buildIndex == 0) {
            HandleEscapeKey();
            _playerInput.actions.FindActionMap("characterController").Enable();
            _playerInput.actions.FindActionMap("Menu").Disable();
        }
        else {
            _playerInput.actions.FindActionMap("characterController").Disable();
            _playerInput.actions.FindActionMap("Menu").Enable();
        }
        _playerInput.actions.FindActionMap("Menukey").Enable();
        Time.timeScale = 1;
    }

    private void OnPauseMenuPerformed(InputAction.CallbackContext context) {
        HandleEscapeKey();
    }

    public void HandleEscapeKey() {
        if(_windows.Count == 0) {
            _menuCanvas.SetActive(true);
            _windows.Add(_menuCanvas);
            Time.timeScale = 0;
            _playerInput.actions.FindActionMap("characterController").Disable();
            _playerInput.actions.FindActionMap("Menu").Enable();
            return;
        }
        GameObject windowToClose = _windows[_windows.Count - 1];
        windowToClose.SetActive(false);
        _windows.Remove(windowToClose);
        if(_windows.Count == 0) {
            if(SceneManager.GetActiveScene().buildIndex != 0) {
                Time.timeScale = 1;
                _playerInput.actions.FindActionMap("characterController").Enable();
                _playerInput.actions.FindActionMap("Menu").Disable();
                return;
            }
        }
        GameObject windowToOpen = _windows[_windows.Count - 1];
        windowToOpen.SetActive(true);
    }

    public void PlayGame() {
        SceneManager.LoadScene(1);
    }

    public void RestartScene() {
        _windows.Clear();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenMainMenu() {
        _playerInput.actions.FindActionMap("characterController").Disable();
        _playerInput.actions.FindActionMap("Menu").Enable();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void OpenSettings() {
        _windows[_windows.Count - 1].SetActive(false);
        _windows.Add(_settingsCanvas);
        _settingsCanvas.SetActive(true);
    }

    public void OpenCredits() {
        _windows[_windows.Count - 1].SetActive(false);
        _windows.Add(_creditsCanvas);
        _creditsCanvas.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }
}