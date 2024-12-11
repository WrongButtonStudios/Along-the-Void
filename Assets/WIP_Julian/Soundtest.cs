using UnityEngine;

public class Soundtest : MonoBehaviour
{
    void Update() {
        if(Input.GetKeyDown("space")) {
            //SoundManager.instance.PlayJumpSoundAtPosition(Vector3.zero);
            SoundManager.instance.PlayOneShotByPath("event:/Jump");
            Debug.Log("space key was pressed");
        }
    }
}