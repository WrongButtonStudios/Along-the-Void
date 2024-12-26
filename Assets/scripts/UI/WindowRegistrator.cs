using UnityEngine;

public class WindowRegistrator : MonoBehaviour
{
    public GameObject firstButton;

    void OnEnable()
    {
        ButtonSelector.Instance.SetActiveWindow(gameObject, firstButton);
    }
}
