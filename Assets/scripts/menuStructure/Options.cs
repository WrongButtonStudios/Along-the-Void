using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioMixer effectMixer;
    [SerializeField] private Slider effectSlider;

    private void Start()
    {
        LoadEffectVolume();
        LoadEffectVolume();
    }

    public void SafeVolume()
    {
        SetMusicVolume();
        SetEffectVolume();
    }


    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        musicMixer.SetFloat("music", Mathf.Log10(volume) * 10);
    }

    public void SetEffectVolume()
    {
        float volume = effectSlider.value;
        effectMixer.SetFloat("effects", Mathf.Log10(volume) * 10);
    }
    private void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
    }
    private void LoadEffectVolume()
    {
        effectSlider.value = PlayerPrefs.GetFloat("effectVolume");
        SetEffectVolume();
    }
}


