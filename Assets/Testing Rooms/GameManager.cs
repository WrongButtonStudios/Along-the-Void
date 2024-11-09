using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneDropdownManager : MonoBehaviour
{
    public static SceneDropdownManager Instance { get; private set; }
    public Dropdown sceneDropdown; // Reference to the Dropdown component


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        // Ensure the Dropdown component is assigned
        if (sceneDropdown == null)
        {
            Debug.LogError("SceneDropdown is not assigned. Please assign a Dropdown component.");
            return;
        }

        // Populate the dropdown with scenes from the Build Settings
        PopulateDropdown();

        // Add a listener to handle scene loading on dropdown selection
        sceneDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }



    // Populate the dropdown menu with scene names
    private void PopulateDropdown()
    {
        List<string> sceneNames = new List<string>();

        // Loop through all scenes in the Build Settings and add their names
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            sceneNames.Add(sceneName);
        }

        // Clear existing options and add new scene names to the dropdown
        sceneDropdown.ClearOptions();
        sceneDropdown.AddOptions(sceneNames);
    }

    // Handle dropdown value change (when a new scene is selected)
    private void OnDropdownValueChanged(int index)
    {
        // Load the scene corresponding to the selected index
        SceneManager.LoadScene(index);
    }

    private void OnDestroy()
    {
        // Remove the listener when the object is destroyed
        sceneDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }
}