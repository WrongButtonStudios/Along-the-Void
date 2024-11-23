using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IplayerFeature
{
    public void initFeauture(characterController characterController);

    public void triggerFeauture(bool useInput = false, bool input = false);

    public void endFeauture();
}