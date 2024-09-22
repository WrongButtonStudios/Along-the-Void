using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VolumeSafe : MonoBehaviour
{
    [SerializeField] private Slider slider = null;


    // Start is called before the first frame update
    void Start()
    {
        LoadVolume();
    }

    public void VolumeSlider(float volume)
    {
        slider.value = volume;
    }

    // Update is called once per frame
    void Update()
    {
        float volumeValue = slider.value;
        PlayerPrefs.SetFloat("VolumeValue", volumeValue);
        LoadVolume();
    }

    public void LoadVolume()
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        slider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }
}