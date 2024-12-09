using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class ControlsManager : MonoBehaviour
{
    public InputActionAsset baseInputActions;
    public TMP_Dropdown actionMapDropdown;

    private InputActionMap keyboardMap;
    private InputActionMap controllerMap;

    private void Start()
    {
        if (baseInputActions == null || actionMapDropdown == null)
        {
            Debug.LogError("Bitte baseInputActions und actionMapDropdown im Inspector zuweisen.");
            return;
        }
        keyboardMap = baseInputActions.FindActionMap("Keyboard", true);
        controllerMap = baseInputActions.FindActionMap("Controller", true);

        if (keyboardMap == null || controllerMap == null)
        {
            Debug.LogError("Eine oder beide Action Maps (Keyboard, Controller) wurden nicht gefunden!");
            return;
        }

        actionMapDropdown.ClearOptions();
        actionMapDropdown.AddOptions(new System.Collections.Generic.List<string> { "Keyboard", "Controller" });

        actionMapDropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        EnableActionMap(keyboardMap);
    }

    private void OnDropdownValueChanged(int index)
    {
        if (index == 0)
        {
            EnableActionMap(keyboardMap);
        }
        else if (index == 1)
        {
            EnableActionMap(controllerMap);
        }
    }

    private void EnableActionMap(InputActionMap actionMap)
    {
        foreach (var map in baseInputActions.actionMaps)
        {
            map.Disable();
        }

        actionMap.Enable();
        Debug.Log($"Action Map '{actionMap.name}' aktiviert.");
    }
}