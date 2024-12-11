using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using System;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private EventReference _jump;

    private List<EventInstance> _events;

    private struct EventEmitterObject
    {
        public GameObject GameObject;
        public Transform Transform;
        //public Rigidbody Rigidbody; // every 3D sound will "need" this, but it can be set later manually
        public Dictionary<string,EventInstance> EventInstances; // there can be several dictionaries which manage different things
    }

    private Dictionary<GameObject,EventEmitterObject> eventEmitter = new(); // for organizing 3D Sounds

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
        _events = new List<EventInstance>();
    }

    public void RegisterEventEmitter(GameObject krachmacher,string path) {
        if(!eventEmitter.ContainsKey(krachmacher)) {
            EventEmitterObject emitter = new() {
                GameObject = krachmacher,
                Transform = krachmacher.transform,
                //Rigidbody = GameObject.Rigidbody,
                EventInstances = new Dictionary<string,EventInstance>(),
            };
            eventEmitter.Add(krachmacher,emitter);
        }
        if(!eventEmitter[krachmacher].EventInstances.ContainsKey(path)) {
            EventInstance eventEmitterInstance = FMODUnity.RuntimeManager.CreateInstance(path);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventEmitterInstance,eventEmitter[krachmacher].Transform,eventEmitter[krachmacher].GameObject.GetComponent<Rigidbody>());
            eventEmitterInstance.start();
            eventEmitter[krachmacher].EventInstances.Add(path,eventEmitterInstance);
        }
    }

    public void UnregisterEventEmitter(GameObject emitter,string path) {
        var eventInstance = eventEmitter[emitter].EventInstances[path];
        eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        eventInstance.setCallback(EventCallback,FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED);
        eventEmitter[emitter].EventInstances.Remove(path);
    }

    private FMOD.RESULT EventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type,IntPtr instancePtr,IntPtr parameters) {
        if(type == FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED) {
            FMOD.Studio.EventInstance eventInstance = new FMOD.Studio.EventInstance(instancePtr);
            eventInstance.release();
        }
        return FMOD.RESULT.OK;
    }

    public void SetParameterToEventEmitter(GameObject emitter,string path,string parameterName,float parameterValue) {
        eventEmitter[emitter].EventInstances[path].setParameterByName(parameterName,parameterValue);
    }

    public void PlayOneShot2D(string path) {
        RuntimeManager.PlayOneShot(path);
    }

    public void PlayOneShot3D(string path,Vector3 position) {
        RuntimeManager.PlayOneShot(path,position);
    }

    // will need needed later
    // GetComponent -> needs another architecture
    public void PlaySound3DwithParameter(string path,GameObject gameObject,string parameterName,float parameterValue) {
        EventInstance eventInstance = RuntimeManager.CreateInstance(path);
        RuntimeManager.AttachInstanceToGameObject(eventInstance,gameObject.transform,gameObject.GetComponent<Rigidbody>());
        eventInstance.setParameterByName(parameterName,parameterValue);
        eventInstance.start();
        eventInstance.release();
    }
}