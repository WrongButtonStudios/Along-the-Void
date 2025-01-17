using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private GameObject _soundSettingsCanvas;
    [SerializeField] private GameObject _graphicSettingsCanvas;
    [SerializeField] private GameObject _creditsCanvas;
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _play;
    [SerializeField] private GameObject _resume;
    [SerializeField] private GameObject _restart;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _mainmenu;
    [SerializeField] private GameObject _credits;
    [SerializeField] private GameObject _quit;
    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private GameObject _mainMenuFirstButton;
    [SerializeField] private GameObject _menuFirstButton;
    [SerializeField] private GameObject _soundSettingsFirstButton;
    [SerializeField] private GameObject _graphicSettingsFirstButton;
    private GameObject _currentFirstButton;

    private InputAction _move;
    private InputAction _escape;
    private List<GameObject> _windows = new();

    private void Awake()
    {
        _escape = _playerInput.actions["Menukey"];
    }

    private void OnEnable()
    {
        _escape.started += OnPauseMenuPerformed;
        _move.started += OnNavigatePerformed;
    }

    private void OnDisable()
    {
        _escape.started -= OnPauseMenuPerformed;
    }

    private void Start()
    {

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            HandleEscapeKey();
            _playerInput.actions.FindActionMap("characterController").Enable();
            _playerInput.actions.FindActionMap("Menu").Disable();
            _play.SetActive(true);
            _resume.SetActive(false);
            _restart.SetActive(false);
            _settings.SetActive(true);
            _mainmenu.SetActive(false);
            _credits.SetActive(true);
            _quit.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_mainMenuFirstButton);

        }
        else
        {
            _playerInput.actions.FindActionMap("characterController").Disable();
            _playerInput.actions.FindActionMap("Menu").Enable();
            _play.SetActive(false);
            _resume.SetActive(true);
            _restart.SetActive(true);
            _settings.SetActive(true);
            _mainmenu.SetActive(true);
            _credits.SetActive(true);
            _quit.SetActive(true);
        }
        _playerInput.actions.FindActionMap("Menukey").Enable();
        Time.timeScale = 1;
    }

    private void OnPauseMenuPerformed(InputAction.CallbackContext context)
    {
        HandleEscapeKey();
    }

    public void HandleEscapeKey()
    {

        if (_windows.Count == 0)
        {
            _menuCanvas.SetActive(true);
            _windows.Add(_menuCanvas);
            Time.timeScale = 0;
            _playerInput.actions.FindActionMap("characterController").Disable();
            _playerInput.actions.FindActionMap("Menu").Enable();
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                _currentFirstButton = _menuFirstButton;
                return;
            }
            else
                _currentFirstButton = _mainMenuFirstButton;
            return;
        }
        GameObject windowToClose = _windows[_windows.Count - 1];
        windowToClose.SetActive(false);
        _windows.Remove(windowToClose);
        if (_windows.Count == 0)
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                Time.timeScale = 1;
                _playerInput.actions.FindActionMap("characterController").Enable();
                _playerInput.actions.FindActionMap("Menu").Disable();
                return;
            }
        }
        GameObject windowToOpen = _windows[_windows.Count - 1];
        windowToOpen.SetActive(true);

        _settingsCanvas.SetActive(false);


        _graphicSettingsCanvas.SetActive(false);
        _soundSettingsCanvas.SetActive(false);

    }

    public void OnNavigatePerformed(InputAction.CallbackContext context)
    {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void RestartScene()
    {
        _windows.Clear();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenMainMenu()
    {
        _playerInput.actions.FindActionMap("characterController").Disable();
        _playerInput.actions.FindActionMap("Menu").Enable();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void OpenSettings()
    {
        if (!_settingsCanvas.activeSelf)
        {
            if (_windows.Count > 0)
            {
                _windows[_windows.Count - 1].SetActive(false);
            }
            _windows.Add(_settingsCanvas);
            _settingsCanvas.SetActive(true);
            OpenSoundSettings();
        }
    }
    public void OpenSoundSettings()
    {
        if (!_soundSettingsCanvas.activeSelf)
            _soundSettingsCanvas.SetActive(true);
        _currentFirstButton = _soundSettingsFirstButton;
        {
            if (_graphicSettingsCanvas.activeSelf)
                _graphicSettingsCanvas.SetActive(false);
        }

    }

    public void OpenGraphicSettings()
    {
        if (!_graphicSettingsCanvas.activeSelf)
        {
            _graphicSettingsCanvas.SetActive(true);
            _currentFirstButton = _graphicSettingsFirstButton;
            if (_soundSettingsCanvas.activeSelf)
                _soundSettingsCanvas.SetActive(false);
        }
    }

    public void OpenCredits()
    {
        _windows[_windows.Count - 1].SetActive(false);
        _windows.Add(_creditsCanvas);
        _creditsCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}