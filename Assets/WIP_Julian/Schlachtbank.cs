using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Schlachtbank : MonoBehaviour
{
    public class ParentEventInstance
    {
        public EventInstance EventInstance {
            get;
        }
        public ParentEventInstance(string name,ParentEventInstance myEventInstance) {
            if(myEventInstance != null) {
                myEventInstance.Stop();
            }
            EventInstance = FMODUnity.RuntimeManager.CreateInstance(name);
            EventInstance.start();
        }
        public void Stop() {
            if(IsPlaying()) {
                EventInstance.stop(STOP_MODE.ALLOWFADEOUT);
            }
        }
        public bool IsPlaying() {
            EventInstance.getPlaybackState(out PLAYBACK_STATE state);
            return state == PLAYBACK_STATE.PLAYING;
        }
        public void SetParameter(string parameter,float parametervalue) {
            EventInstance.setParameterByName(parameter,parametervalue);
        }
    }
}