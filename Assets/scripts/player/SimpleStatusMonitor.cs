using UnityEngine;

public class SimpleStatusMonitor : MonoBehaviour
{
    void Update()
    {
        // Status direkt abfragen
        characterController.playerStatusData currentPlayerStatus = FindObjectOfType<characterController>().getPlayerStatus();

        // Status-spezifische Debug Logs
        if (currentPlayerStatus.isFrozen)
        {
            Debug.Log("Spieler ist eingefroren!");
        }

        if (currentPlayerStatus.isMoving)
        {
            Debug.Log("Spieler bewegt sich!");
        }

        if (currentPlayerStatus.isGrounded)
        {
            Debug.Log("Spieler ist am Boden!");
        }

        // State (Farbe) überprüfen
        switch (currentPlayerStatus.currentState)
        {
            case characterController.playerStates.green:
                Debug.Log("Spieler ist grün!");
                break;
            case characterController.playerStates.red:
                Debug.Log("Spieler ist rot!");
                break;
            case characterController.playerStates.blue:
                Debug.Log("Spieler ist blau!");
                break;
            case characterController.playerStates.yellow:
                Debug.Log("Spieler ist gelb!");
                break;
            case characterController.playerStates.dead:
                Debug.Log("Spieler ist tot!");
                break;
            case characterController.playerStates.burntGreen:
            case characterController.playerStates.burntRed:
            case characterController.playerStates.burntBlue:
            case characterController.playerStates.burntYellow:
                Debug.Log("Spieler ist verbrannt!");
                break;
        }
    }
}