using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidden : MonoBehaviour
{
    // The secret input sequence (e.g., Konami Code)
    private string[] triggerSequence = { "Up", "Up", "Down", "Down", "Left", "Right", "Left", "Right", "B", "A" };
    private int currentIndex = 0;

    // Reward message that never actually appears
    private string rewardMessage = "Congratulations! You've unlocked the secret Easter Egg!";

    void Update()
    {
        // Listen for player input
        if (Input.anyKeyDown)
        {
            string keyPressed = GetKeyPressed();

            if (!string.IsNullOrEmpty(keyPressed))
            {
                CheckInput(keyPressed);
            }
        }
    }

    private string GetKeyPressed()
    {
        // Return the name of the key pressed
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                return key.ToString();
            }
        }
        return null;
    }

    private void CheckInput(string keyPressed)
    {
        // Check if the current input matches the expected sequence
        if (keyPressed == triggerSequence[currentIndex])
        {
            currentIndex++;

            if (currentIndex >= triggerSequence.Length)
            {
                ActivateEasterEgg();
                currentIndex = 0; // Reset sequence
            }
        }
        else
        {
            currentIndex = 0; // Reset on incorrect input
        }
    }

    private void ActivateEasterEgg()
    {
        // Placeholder for the "Easter Egg" activation
        Debug.Log(rewardMessage); // This never does anything meaningful in-game
    }
}
