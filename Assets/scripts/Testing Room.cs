using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingRoom : MonoBehaviour
{
    // Start is called before the first frame update



    public void CharackterController()
    {
        SceneManager.LoadScene("characterController");
    }
    public void AITestingRoom()
    {
        SceneManager.LoadScene("EnemyTestScene");
    }
    public void VisuelNovel()
    {
        SceneManager.LoadScene("visuelnovel");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void PauseMenu()
    {
        SceneManager.LoadScene("Level 1");
    }

}
