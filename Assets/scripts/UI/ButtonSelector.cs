using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.LookDev;

public class ButtonSelector : MonoBehaviour
{
    public GameObject currentWindow;
    public GameObject currentWindowFirstButton;
    private PlayerInput playerInput;




    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        // if (playerInput.currentActionMap.name == "Controller")
        {
            SelectButton();
        }
    }

    private void Update()
    {
        //if (playerInput.currentActionMap.name == "Controller")
        {
            HandleNavigation();
        }
    }

    private void SelectButton()
    {
        if (currentWindow != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (currentWindowFirstButton != null)
            {
                EventSystem.current.SetSelectedGameObject(currentWindowFirstButton);
            }
        }
    }

    private void HandleNavigation()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            SelectButton();
        }
    }
}