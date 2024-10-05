using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioMixer effectMixer;
    [SerializeField] private Slider effectSlider;


    private void Start()
    {
        LoadMasterVolume();
        LoadMusicVolume();
        LoadEffectVolume();
    }

 



    //Set Volume
    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        masterMixer.SetFloat("master", Mathf.Log10(volume) * 10);
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

    //Load Volume
    private void LoadMasterVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("masterVolume");
        SetMasterVolume();
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