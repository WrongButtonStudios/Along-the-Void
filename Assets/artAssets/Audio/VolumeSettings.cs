using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;


    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVolume();
            SetMusicVolume();
            SetEffectVolume();
        }
    }





    //Set Volume
    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        myMixer.SetFloat("master", Mathf.Log10(volume) * 21);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 21);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetEffectVolume()
    {
        float volume = effectSlider.value;
        myMixer.SetFloat("effects", Mathf.Log10(volume) * 21);
        PlayerPrefs.SetFloat("effectsVolume", volume);
    }

    //Load Volume
    private void LoadVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        effectSlider.value = PlayerPrefs.GetFloat("effectsVolume");
        SetMasterVolume();
        SetMusicVolume();
        SetEffectVolume();
    }
}