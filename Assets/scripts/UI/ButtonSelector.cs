using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class ButtonSelector : MonoBehaviour
{
    public static ButtonSelector Instance { get; private set; }
    private Mouse mouse;
    private Vector2 lastMousePosition;
    private GameObject activeWindow;
    private GameObject activeWindowFirstButton;
    private BaseInputActions navigateAction;
    private Selectable activeFirstSelectable;
    private Navigation originalNavigation;
    private bool isProcessingNavigation = false;

    void Awake()
    {
        mouse = Mouse.current;
        if (Instance == null)
        {
            Instance = this;
            navigateAction = new BaseInputActions();
            navigateAction.Menu.Enable();
            navigateAction.Menu.Move.performed += OnNavigate;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        if (mouse != null)
        {
            lastMousePosition = mouse.position.ReadValue();
        }
    }
    private void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

    private void Update()
    {
        Vector2 currentMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (currentMousePosition != lastMousePosition)
        {
            MouseIsUsed();
            lastMousePosition = currentMousePosition;
        }
    }

    public void SetActiveWindow(GameObject window, GameObject firstButton)
    {
        if (window != null && firstButton != null && window.activeInHierarchy)
        {
            activeWindow = window;
            activeWindowFirstButton = firstButton;
        }
    }



    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (isProcessingNavigation) return;

        Vector2 navigationInput = context.ReadValue<Vector2>();
        if (EventSystem.current.currentSelectedGameObject == null && Mathf.Abs(navigationInput.y) > 0.1f)
        {
            isProcessingNavigation = true;
            SelectButton();
            if (activeWindowFirstButton != null)
            {
                activeFirstSelectable = activeWindowFirstButton.GetComponent<Selectable>();
                if (activeFirstSelectable != null)
                {
                    originalNavigation = activeFirstSelectable.navigation;
                    var newNavigation = new Navigation { mode = Navigation.Mode.None };
                    activeFirstSelectable.navigation = newNavigation;
                    StartCoroutine(RestoreNavigation());
                }
            }
        }
    }

    private System.Collections.IEnumerator RestoreNavigation()
    {
        yield return new WaitForEndOfFrame();

        if (activeFirstSelectable != null)
        {
            activeFirstSelectable.navigation = originalNavigation;
        }
        isProcessingNavigation = false;
    }

    private void SelectButton()
    {
        if (activeWindow != null && activeWindow.activeInHierarchy &&
            activeWindowFirstButton != null && activeWindowFirstButton.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(activeWindowFirstButton);
        }
    }

    private void MouseIsUsed()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnDestroy()
    {
        if (navigateAction != null)
        {
            navigateAction.Menu.Move.performed -= OnNavigate;
            navigateAction.Dispose();
        }
    }
}