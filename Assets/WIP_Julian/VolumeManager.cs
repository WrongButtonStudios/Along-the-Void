using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    private Bus MusicBus, SFXBus, SFXNoUIBus, MasterBus;

    private float _masterVolume = 100.0f;
    private float _sfxVolume = 100.0f;
    private float _musicVolume = 100.0f;

    private float pauseVolumeReduction = 0.6f;

    private void Awake() {
        MusicBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        SFXBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        SFXNoUIBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX/SFXNoUI");
        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/");

        // load volume settings here
        // store to ui also
    }

    public void MusicBusSetVolume(float volume) {
        _musicVolume = volume;
        MusicBus.setVolume(volume);
    }
    public void SFXBusSetVolume(float volume) {
        _sfxVolume = volume;
        SFXBus.setVolume(volume);
    }
    public void MasterBusSetVolume(float volume) {
        _masterVolume = volume;
        MasterBus.setVolume(volume);
    }
    public void Pause() {
        SFXNoUIBus.setPaused(true);
        MusicBus.setVolume(_musicVolume * pauseVolumeReduction);
    }
    public void Resume() {
        SFXNoUIBus.setPaused(false);
        MusicBus.setVolume(_musicVolume);
    }
}