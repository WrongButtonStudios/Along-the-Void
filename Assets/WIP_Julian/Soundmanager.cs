using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;

public class SoundManager : MonoBehaviour
{
    private struct SoundEmittingComponent
    {
        public GameObject gameObject;
        public Dictionary<string,EventInstance> registeredSounds;
    }

    private Dictionary<GameObject,SoundEmittingComponent> _registeredSoundEmitters = new();

    public static SoundManager instance {
        get; private set;
    }

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public void RegisterNewSound(GameObject emittingObject,Rigidbody rigidbody,string soundEventName) {
        if(!_registeredSoundEmitters.ContainsKey(emittingObject)) {
            SoundEmittingComponent newComponent = new() {
                gameObject = emittingObject,
                registeredSounds = new Dictionary<string,EventInstance>(),
            };
            _registeredSoundEmitters.Add(emittingObject,newComponent);
        }
        if(!_registeredSoundEmitters[emittingObject].registeredSounds.ContainsKey(soundEventName)) {
            EventInstance emitter = RuntimeManager.CreateInstance(soundEventName);
            RuntimeManager.AttachInstanceToGameObject(
                emitter,
                emittingObject.transform,
                rigidbody);
            emitter.start();
            _registeredSoundEmitters[emittingObject].registeredSounds.Add(soundEventName,emitter);
        }
    }

    public void UnregisterSound(GameObject emittingObject,string path) {
        EventInstance emitter = _registeredSoundEmitters[emittingObject].registeredSounds[path];
        emitter.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        emitter.setCallback(EventCallback,FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED);
        _registeredSoundEmitters[emittingObject].registeredSounds.Remove(path);
    }

    private FMOD.RESULT EventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type,IntPtr instancePtr,IntPtr parameters) {
        if(type == FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED) {
            FMOD.Studio.EventInstance eventInstance = new FMOD.Studio.EventInstance(instancePtr);
            eventInstance.release();
        }
        return FMOD.RESULT.OK;
    }

    public void SetParameterToEventEmitter(GameObject emitter,string path,string parameterName,float parameterValue) {
        _registeredSoundEmitters[emitter].registeredSounds[path].setParameterByName(parameterName,parameterValue);
    }

    public void PlayOneShot2D(string path) {
        RuntimeManager.PlayOneShot(path);
    }

    public void PlayOneShot3D(string path,Vector3 position) {
        RuntimeManager.PlayOneShot(path,position);
    }

    // will be needed later
    // GetComponent -> needs another architecture
    public void PlaySound3DwithParameter(string path,GameObject gameObject,string parameterName,float parameterValue) {
        EventInstance eventInstance = RuntimeManager.CreateInstance(path);
        RuntimeManager.AttachInstanceToGameObject(eventInstance,gameObject.transform,gameObject.GetComponent<Rigidbody>());
        eventInstance.setParameterByName(parameterName,parameterValue);
        eventInstance.start();
        eventInstance.release();
    }
}