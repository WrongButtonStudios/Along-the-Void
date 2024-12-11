using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundtest : MonoBehaviour
{
    void Start() {

    }
    void Update() {
        if(Input.GetKeyDown("space")) {
            Debug.Log("space key was pressed");
        }
    }
}