using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraficSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
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
    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }
}
