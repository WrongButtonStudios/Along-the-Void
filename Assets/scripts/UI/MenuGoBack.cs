using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuGoBack : MonoBehaviour
{
    public GameObject higherMenu;
    private @BaseInputActions controls;

    private void Awake()
    {
        controls = new @BaseInputActions();
        controls.MainMenu.BackButton.performed += ctx => GoBack();
        controls.MainMenu.Enable();
    }

    void GoBack()
    {
        higherMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}