using UnityEngine;

public class TestingShitScript : MonoBehaviour
{
    [SerializeField] private characterController playerController; // Kann nun im Inspector zugewiesen werden
    private characterController.playerStates lastState;

    private void Start()
    {
        // Falls keine Referenz im Inspector gesetzt wurde, suche in der Szene
        if (playerController == null)
        {
            playerController = FindObjectOfType<characterController>();

            if (playerController == null)
            {
                Debug.LogError("Kein Character Controller in der Szene gefunden!");
                return;
            }
        }

        // Initialisiere den letzten Status
        lastState = playerController.getPlayerStatus().currentState;
        LogCurrentState("Initialer Status");
    }

    private void Update()
    {
        if (playerController == null) return;

        var currentStatus = playerController.getPlayerStatus();

        // Überprüfe ob sich der Status geändert hat
        if (currentStatus.currentState != lastState)
        {
            LogCurrentState("Status hat sich geändert");
            lastState = currentStatus.currentState;
        }

        // Detaillierte Status-Informationen
        LogDetailedStatus(currentStatus);
    }

    private void LogCurrentState(string context)
    {
        var status = playerController.getPlayerStatus();
        string stateColor = GetStateColor(status.currentState);
        Debug.Log($"[{context}] Character Status: {status.currentState} {stateColor}");
    }

    private void LogDetailedStatus(characterController.playerStatusData status)
    {
        string movementStatus = status.isMoving ? "bewegt sich" : "steht still";
        string groundedStatus = status.isGrounded ? "auf dem Boden" : "in der Luft";
        string frozenStatus = status.isFrozen ? "eingefroren" : "nicht eingefroren";

        Debug.Log($"Detaillierter Status:\n" +
                 $"- Bewegung: {movementStatus}\n" +
                 $"- Position: {groundedStatus}\n" +
                 $"- Zustand: {frozenStatus}");
    }

    private string GetStateColor(characterController.playerStates state)
    {
        switch (state)
        {
            case characterController.playerStates.green:
                return "I am green";
            case characterController.playerStates.red:
                return "I am red";
            case characterController.playerStates.blue:
                return "I am blue";
            case characterController.playerStates.yellow:
                return "I am yellow";
            case characterController.playerStates.burntGreen:
                return "I burn and am green";
            case characterController.playerStates.burntRed:
                return "I burn and am red";
            case characterController.playerStates.burntBlue:
                return "I can not burn, because I am blue";
            case characterController.playerStates.burntYellow:
                return "I burn and am yellow";
            case characterController.playerStates.dead:
                return "I am dead";
            default:
                return "default";
        }
    }
}