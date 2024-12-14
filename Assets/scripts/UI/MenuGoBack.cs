using UnityEngine;
using UnityEngine.InputSystem;

public class MenuGoBack : MonoBehaviour //Zieht sich Action maps, aktiviert den Back-Button, deaktiviert diesen wieder wenn tieferliegende Menueebenen geschlossen werden, schlieﬂt tieferliegende Ebenen und aktiviert hoeherliegende
{
    public GameObject higherMenu;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("MainMenu");
        controls.MainMenu.BackButton.performed += ctx => GoBack();
    }

    void GoBack()
    {
        controls.MainMenu.BackButton.performed -= ctx => GoBack();
        higherMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}