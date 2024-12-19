using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] GameObject optionsCanvas;
    private static PauseMenu instance;
    private Vector2 respawnPos = new Vector2(0, 0);
    private GameObject player;
    private PlayerInput playerInput;
    private InputAction openPauseMenu;
    private InputAction closePauseMenu;
    private bool isOpend = false;
    private bool isDebounce = false; // F?gt ein neues Flag f?r Debouncing hinzu.
    private void Start()
    {
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
            DontDestroyOnLoad(gameObject); // Verhindert, dass dieses Objekt zerst?rt wird
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject); // Zerst?re zus?tzliche Instanzen
            }
            respawnPos = new Vector2(player.transform.position.x, player.transform.position.y);
        }
    }
    private IEnumerator ResetDebounce()
    {
        yield return new WaitForSecondsRealtime(0.1f); // Wartet 0.1 Sekunden (reale Zeit, unabh?ngig von TimeScale)
        isDebounce = false;
    }

    private void OnPauseMenuPerformed(InputAction.CallbackContext context)
    {
        if (!isOpend && !isDebounce) // Nur ausf?hren, wenn Debounce inaktiv
        {
            isDebounce = true; // Aktiviert Debouncing
            StartCoroutine(ResetDebounce()); // Debouncing-Timer starten

            pauseMenuCanvas.SetActive(true);
            playerInput.SwitchCurrentActionMap("PauseMenu");
            Time.timeScale = 0;
            openPauseMenu.performed -= OnPauseMenuPerformed;
            closePauseMenu.performed += OnPauseMenuClosePerformed;
            isOpend = true;
        }
    }

    private void OnPauseMenuClosePerformed(InputAction.CallbackContext context)
    {
        if (isOpend && !isDebounce) // Nur ausf?hren, wenn Debounce inaktiv
        {
            isDebounce = true; // Aktiviert Debouncing
            StartCoroutine(ResetDebounce()); // Debouncing-Timer starten

            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1;
            playerInput.SwitchCurrentActionMap("characterController");
            closePauseMenu.performed -= OnPauseMenuClosePerformed;
            openPauseMenu.performed += OnPauseMenuPerformed;
            isOpend = false;
        }
    }
    public void Resume()
    {
        SwitchToIngame();
        Time.timeScale = 1;
        pauseMenuCanvas.SetActive(false);
        
    }
    public void Restart()
    {
        SwitchToIngame();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Options()
    {
        isOpend = false; 
        optionsCanvas.SetActive(true);
    }
    public void MainMenu()
    {
        SwitchToMenu();
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
    public void RespawnButton()
    {
        SwitchToIngame();
        if (GetComponent<characterController>() != null)
        {
            characterController cc = gameObject.GetComponent<characterController>();
            cc.rb.velocity = Vector2.zero;
            cc.rb.position = respawnPos;
        }
    }
    private void SwitchToMenu()
    {
        playerInput.SwitchCurrentActionMap("PauseMenu");
    }
    private void SwitchToIngame()
    {
        playerInput.SwitchCurrentActionMap("characterController");
        openPauseMenu.performed += OnPauseMenuPerformed;
    }
}