using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.UI;
using UnityEngine;

public class playerClimbWall : MonoBehaviour, IplayerFeature
{
    private characterController characterController;

    public LayerMask layerMask;

    public void initFeauture(characterController characterController)
    {
        this.characterController = characterController;
    }

    public void triggerFeauture()
    {
        
    }
}
