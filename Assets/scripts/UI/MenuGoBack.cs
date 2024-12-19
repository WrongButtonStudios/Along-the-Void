using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuGoBack : MonoBehaviour //Zieht sich Action maps, aktiviert den Back-Button, deaktiviert diesen wieder wenn tieferliegende Menueebenen geschlossen werden, schlie?t tieferliegende Ebenen und aktiviert hoeherliegende
{
    public GameObject higherMenu;
    private PlayerInput playerInput;
    private InputAction goBack;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        playerInput.SwitchCurrentActionMap("PauseMenu");
        goBack = playerInput.actions["BackButton"];
    }
    private void Start()
    {
        StartCoroutine(Delay());
    }

    private void OnEnable()
    {
        StartCoroutine(Delay()); 

    }
    private IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        goBack.performed += OnGoBackPerformed;
    }
    void OnGoBackPerformed(InputAction.CallbackContext context)
    {
        goBack.performed -= OnGoBackPerformed;
        higherMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}