using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] GameObject optionsCanvas;
    [SerializeField] GameObject graphicsCanvas;
    private static PauseMenu instance;
    private Vector2 respawnPos = new Vector2(0, 0);
    private GameObject player;
    private PlayerInput playerInput;
    private InputAction openPauseMenu;
    private InputAction closePauseMenu;
    private List<GameObject> menuWindows = new();

    private void Start()
    {
        StartCoroutine(CleanupInput());
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found! Ensure it is tagged as 'Player'.");
            return;
        }

        playerInput = FindObjectOfType<PlayerInput>();
        closePauseMenu = playerInput.actions["ClosePauseMenu"];
        openPauseMenu = playerInput.actions["Pausemenu"];
        openPauseMenu.performed += OnPauseMenuPerformed;

        if (GetComponent<characterController>() != null)
        {
            DontDestroyOnLoad(gameObject);
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            respawnPos = new Vector2(player.transform.position.x, player.transform.position.y);
        }
    }

    //private void OnDestroy()
    //{
    //    StartCoroutine(CleanupInput());
    //}
    private IEnumerator CleanupInput()
    {
        yield return new WaitForEndOfFrame();
        if (openPauseMenu != null)
            openPauseMenu.performed -= OnPauseMenuPerformed;

        if (closePauseMenu != null)
            closePauseMenu.performed -= OnPauseMenuPerformed;

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

    void Update()
    {

        if (pauseMenuCanvas.activeSelf)
        {
            pauseMenuCanvasActivated();
        }
    }

    private void pauseMenuCanvasActivated()
    {
        closePauseMenu.performed += OnPauseMenuPerformed;
    }

    private void OnPauseMenuPerformed(InputAction.CallbackContext context)
    {
        if (menuWindows.Count > 1) // >1 means 2 or more are open so full program - Close current, remove current from list, reload list, open new current window.
        {
            GameObject currentMenu = menuWindows[menuWindows.Count - 1];
            currentMenu.SetActive(false);
            menuWindows.Remove(currentMenu);

            menuWindows[menuWindows.Count - 1].SetActive(true);
        }

        else if (menuWindows.Count == 1) // ==1 means just menu window itself is open, so close is and remove is from list.
        {
            GameObject currentMenu = menuWindows[menuWindows.Count - 1];
            if (currentMenu != null)
            {
                currentMenu.SetActive(false);
            }
            menuWindows.Remove(currentMenu);
            playerInput.SwitchCurrentActionMap("characterController");
        }

        else if (menuWindows.Count < 1) // <1 means no menu window open, so open and add to list.
        {
            if (pauseMenuCanvas != null)
            {
                pauseMenuCanvas.SetActive(true);
            }
            menuWindows.Add(pauseMenuCanvas);
            playerInput.SwitchCurrentActionMap("Menu");
        }
    }

    public void Resume()
    {
        GameObject currentMenu = menuWindows[menuWindows.Count - 1];
        currentMenu.SetActive(false);
        menuWindows.Remove(currentMenu);
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1;
        playerInput.SwitchCurrentActionMap("characterController");
        openPauseMenu.performed += OnPauseMenuPerformed;
    }

    public void Restart()
    {
        menuWindows.Clear();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Options()
    {
        menuWindows.Add(optionsCanvas);
        optionsCanvas.SetActive(true);
    }

    public void MainMenu()
    {
        playerInput.SwitchCurrentActionMap("MainMenu");
        openPauseMenu.performed -= OnPauseMenuPerformed;
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
        Destroy(this);
    }

    public void RespawnButton()
    {
        playerInput.SwitchCurrentActionMap("characterController");
        openPauseMenu.performed += OnPauseMenuPerformed;

        if (GetComponent<characterController>() != null)
        {
            characterController cc = gameObject.GetComponent<characterController>();
            cc.rb.velocity = Vector2.zero;
            cc.rb.position = respawnPos;
        }
    }

    public void GraphicSettings()
    {
        menuWindows.Add(graphicsCanvas);
        graphicsCanvas.SetActive(true);
    }
}