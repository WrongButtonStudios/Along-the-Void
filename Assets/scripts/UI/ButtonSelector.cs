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

    // Input System Actions
    [SerializeField] private InputAction navigateAction;

    void Awake()
    {
        // Singleton Setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Mouse Setup
        mouse = Mouse.current;
        if (mouse != null)
        {
            lastMousePosition = mouse.position.ReadValue();
        }
    }

    private void OnEnable()
    {
        // Enable the navigate action
        navigateAction.Enable();
        navigateAction.performed += OnNavigate;
    }

    private void OnDisable()
    {
        // Disable the navigate action
        navigateAction.performed -= OnNavigate;
        navigateAction.Disable();
    }

    void Update()
    {
        // Check for mouse movement
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

    public void OnNavigate(InputAction.CallbackContext context)
    {
        // Read navigation input
        Vector2 navigationInput = context.ReadValue<Vector2>();

        // Select the first button if no button is currently selected and there's vertical movement
        if (EventSystem.current.currentSelectedGameObject == null && Mathf.Abs(navigationInput.y) > 0.1f)
        {
            SelectButton();
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
