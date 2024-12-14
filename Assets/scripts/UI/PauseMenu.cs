using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] GameObject optionsCanvas;
    private static PauseMenu instance;
    private Vector2 respawnPos = new Vector2(0, 0);
    private GameObject player;
    private @BaseInputActions charController;
    private PlayerInput playerInput;

    void Awake()
    {
        charController = new @BaseInputActions();
        charController.characterController.Pausemenu.performed += ctx => PauseeMenu();
        charController.characterController.Enable();
        if (GetComponent<characterController>() != null)
        {
            DontDestroyOnLoad(gameObject); // Verhindert, dass dieses Objekt zerstört wird
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject); // Zerstöre zusätzliche Instanzen
            }
            respawnPos = new Vector2(player.transform.position.x, player.transform.position.y);
        }
    }
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>(); //Gets Action Maps
        SwitchToMenu(); //Changes current Action Map to PauseMenu action map
    }
    private void PauseeMenu()
    {
        Time.timeScale = 0;
        pauseMenuCanvas.SetActive(true);
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
    }
}