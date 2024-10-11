using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public void OpenMenu()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}