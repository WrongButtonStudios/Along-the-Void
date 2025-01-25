using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IplayerFeature
{
    public void initFeature(characterController characterController);

    public void triggerFeature(bool useInput = false, bool input = false);

    public void endFeature();
}