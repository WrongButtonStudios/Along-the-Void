using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelector : MonoBehaviour
{
    public GameObject currentWindow;
    public GameObject currentWindowFirstButton;





    private void Start()
    {
        SelectButton(); //Selects the chosen first button on Menu Window start
    }

    private void Update()
    {
        HandleNavigation(); //Re-selects the chosen first button on Menu, when no button is selected
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