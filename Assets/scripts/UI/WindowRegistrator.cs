using UnityEngine;

public class WindowRegistrator : MonoBehaviour
{
    public GameObject firstButton;

    void OnEnable()
    {
        if (ButtonSelector.Instance != null)
        {
            ButtonSelector.Instance.SetActiveWindow(gameObject, firstButton);
        }
        else
        {
            Debug.LogWarning($"ButtonSelector instance not found when registering {gameObject.name}!");
        }
    }
}