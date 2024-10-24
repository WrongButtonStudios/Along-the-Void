using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaverExample : MonoBehaviour
{



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SaveGame();
        if (Input.GetKeyDown(KeyCode.T))
            LoadGame();
    }


    private void SaveGame()
    {

    }

    private void LoadGame()
    {
        SaveData saveData = SaveManager.LoadGameState();
        if (saveData != null)
        {


        }
    }
}