using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeColorDrain : MonoBehaviour
{
    [SerializeField] private Material _mat;

    private void FixedUpdate()
    {
        _mat.SetFloat("_ColorDrain", Timer.TimeInPercent * 2); 
    }
}
