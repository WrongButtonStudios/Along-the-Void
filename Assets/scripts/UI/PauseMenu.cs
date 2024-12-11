using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] GameObject optionsCanvas;
    private static PauseMenu instance;
    private Vector2 respawnPos = new Vector2(0, 0);
    private GameObject player;
    private @BaseInputActions charController;

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
    private void PauseeMenu()
    {
        Time.timeScale = 0;
        pauseMenuCanvas.SetActive(true);
    }
    public void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void Options()
    {
        optionsCanvas.SetActive(true);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }
    public void RespawnButton()
    {

        if (GetComponent<characterController>() != null)
        {
            characterController cc = gameObject.GetComponent<characterController>();
            cc.rb.velocity = Vector2.zero;
            cc.rb.position = respawnPos;
        }
    }
}