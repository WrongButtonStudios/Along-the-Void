using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ButtonSelector : MonoBehaviour
{
    public static ButtonSelector Instance { get; private set; }
    private Mouse mouse;
    private Vector2 lastMousePosition;

    // Keep track of the active window
    private GameObject activeWindow;
    private GameObject activeWindowFirstButton;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mouse = Mouse.current;
        if (mouse != null)
        {
            lastMousePosition = mouse.position.ReadValue();
        }
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null &&
            Input.GetAxisRaw("Vertical") != 0)
        {
            SelectButton();
        }

        if (mouse != null)
        {
            Vector2 currentMousePosition = mouse.position.ReadValue();
            if (currentMousePosition != lastMousePosition)
            {
                MouseIsUsed();
                lastMousePosition = currentMousePosition;
            }
        }
    }

    public void SetActiveWindow(GameObject window, GameObject firstButton)
    {
        activeWindow = window;
        activeWindowFirstButton = firstButton;
    }

    private void SelectButton()
    {
        if (activeWindow != null && activeWindow.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(activeWindowFirstButton);
        }
    }

    private void MouseIsUsed()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

}