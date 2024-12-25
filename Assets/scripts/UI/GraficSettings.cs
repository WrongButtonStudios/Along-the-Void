using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GraficSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown screenModeDropdown;
    Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private int currentResolutionIndex = 0;
    void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        resolutionDropdown.ClearOptions();

        // Get current resolution before filtering
        Resolution currentResolution = Screen.currentResolution;

        // Filter resolutions with matching refresh rate
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.numerator == currentResolution.refreshRateRatio.numerator &&
                resolutions[i].refreshRateRatio.denominator == currentResolution.refreshRateRatio.denominator)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();

        // Find current resolution in filtered list
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + "  " +
                                    filteredResolutions[i].refreshRateRatio + "Hz";
            options.Add(resolutionOption);

            // Check against Screen.currentResolution instead of Screen.width/height
            if (filteredResolutions[i].width == currentResolution.width &&
                filteredResolutions[i].height == currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Populate dropdown with options if not set manually
        if (screenModeDropdown != null)
        {
            screenModeDropdown.ClearOptions();
            screenModeDropdown.AddOptions(new System.Collections.Generic.List<string>
            {
                "Windowed",
                "Fullscreen",
                "Borderless Fullscreen"
            });

            // Set current mode as the selected option
            screenModeDropdown.value = (int)Screen.fullScreenMode;

            // Add listener for changes
            screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetScreenMode(int mode)
    {
        switch (mode)
        {
            case 0: // Windowed
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 1: // Fullscreen
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 2: // Borderless Fullscreen
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            default:
                Debug.LogWarning("Unknown screen mode selected!");
                break;
        }

        Debug.Log($"Screen mode set to: {Screen.fullScreenMode}");
    }
}
