using UnityEngine;
using System;

public class GameInstance : MonoBehaviour
{
    public static GameInstance Instance { get; private set; }
    public int playerStarCounter = 0;
    public float playerHealth = 100f;
    public event Action<int> OnStarsChanged;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame();

            // Stellen sicher, dass die Instance gesetzt ist, bevor andere Objekte sie nutzen
            Debug.Log("GameInstance wurde initialisiert");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initial UI update
        OnStarsChanged?.Invoke(playerStarCounter);
    }

    public void AddStar(int amount)
    {
        playerStarCounter += amount;
        Debug.Log($"AddStar wurde aufgerufen! Neue Anzahl: {playerStarCounter}");

        if (OnStarsChanged == null)
        {
            Debug.LogError("FEHLER: Keine Listener am OnStarsChanged Event registriert!");
        }

        OnStarsChanged?.Invoke(playerStarCounter);
        Debug.Log("Event OnStarsChanged wurde ausgelöst!");
    }





    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            playerStarCounter = playerStarCounter,
            playerHealth = playerHealth,
        };
        SaveSystem.SaveGame(data);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        playerStarCounter = data.playerStarCounter;
        playerHealth = data.playerHealth;
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
