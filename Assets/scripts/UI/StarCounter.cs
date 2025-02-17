using UnityEngine;
using TMPro;
using System.Collections;

public class StarCounter : MonoBehaviour
{
    public TextMeshProUGUI starText;

    private void Awake()
    {
        // Warten einen Frame, um sicherzustellen, dass GameInstance initialisiert ist
        StartCoroutine(DelayedSubscribe());
    }

    private IEnumerator DelayedSubscribe()
    {
        yield return null; // Warte einen Frame
        TrySubscribeToEvent();
    }

    private void TrySubscribeToEvent()
    {
        if (GameInstance.Instance != null)
        {
            GameInstance.Instance.OnStarsChanged -= UpdateStarText;
            GameInstance.Instance.OnStarsChanged += UpdateStarText;
            Debug.Log("StarCounter hat sich erfolgreich am Event angemeldet!");
            UpdateStarText(GameInstance.Instance.playerStarCounter);
        }
        else
        {
            Debug.LogError("FEHLER: GameInstance.Instance ist NULL! Event konnte nicht registriert werden.");
        }
    }

    public void UpdateStarText(int starCount) // Jetzt public
    {
        if (starText != null)
        {
            Debug.Log($"UpdateStarText() wurde aufgerufen! Neuer Wert: {starCount}");
            starText.text = "Stars: " + starCount;
        }
        else
        {
            Debug.LogError("FEHLER: starText Reference ist NULL!");
        }
    }

    private void OnDisable()
    {
        if (GameInstance.Instance != null)
        {
            GameInstance.Instance.OnStarsChanged -= UpdateStarText;
        }
    }
}