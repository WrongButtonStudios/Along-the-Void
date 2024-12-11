using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private EventInstance _musicInstance;
    [SerializeField] private EventInstance _ambientInstance;

    public static MusicManager instance;

    private bool _isMusicPlaying;
    private bool _isAmbientPlaying;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public void StartMusic() {
        if(!_musicInstance.isValid()) {
            _musicInstance = RuntimeManager.CreateInstance("event:/Music/MusicMain");
        }
        if(_isMusicPlaying) {
            Debug.Log("Can't start Music since it is already playing.");
            return;
        }
        _musicInstance.start();
        _isMusicPlaying = true;
    }

    public void StopMusic() {
        if(!_musicInstance.isValid()) {
            Debug.Log("Can't stop Music since it is not set.");
            return;
        }
        if(!_isMusicPlaying) {
            Debug.Log("Can't stop Music since it is not playing.");
            return;
        }
        _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _isMusicPlaying = false;
    }

    public void ChangeMusic(string parameter,float value) {
        if(!_musicInstance.isValid()) {
            Debug.Log("Can't change Music since it is not set.");
            return;
        }
        if(_isMusicPlaying) {
            Debug.Log("Can't change Music since it is not playing.");
            return;
        }
        _musicInstance.setParameterByName(parameter,value);
    }

    public void StartAmbient() {
        if(!_ambientInstance.isValid()) {
            _ambientInstance = RuntimeManager.CreateInstance("event:/SFX/AmbientMain");
        }
        if(_isAmbientPlaying) {
            Debug.Log("Can't start Ambient since it is already playing.");
            return;
        }
        _ambientInstance.start();
        _isAmbientPlaying = true;
    }

    public void StopAmbient() {
        if(!_ambientInstance.isValid()) {
            Debug.Log("Can't stop Ambient since it is not set.");
            return;
        }
        if(!_isAmbientPlaying) {
            Debug.Log("Can't stop Ambient since it is not playing.");
            return;
        }
        _ambientInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _isAmbientPlaying = false;
    }

    public void ChangeAmbient(string parameter,float value) {
        if(!_ambientInstance.isValid()) {
            Debug.Log("Can't change Ambient since it is not set.");
            return;
        }
        if(!_isAmbientPlaying) {
            Debug.Log("Can't change Ambient since it is not playing.");
            return;
        }
        _ambientInstance.setParameterByName(parameter,value);
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnApplicationQuit() {
        StopMusic();
        StopAmbient();
    }

    void OnSceneLoaded(Scene scene,LoadSceneMode mode) {
        float currentScene = (float) SceneManager.GetActiveScene().buildIndex;
        StartMusic();
        _musicInstance.setParameterByName("Level",currentScene);
        if(currentScene > 0 && _ambientInstance.isValid() && _isAmbientPlaying) {
            StopAmbient();
            return;
        }
        StartAmbient();
        _ambientInstance.setParameterByName("Level",currentScene);
    }
}