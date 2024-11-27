using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] GameObject optionsCanvas;
    private static PauseMenu instance;
    void Awake()
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuCanvas.SetActive(true);
        }
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


}