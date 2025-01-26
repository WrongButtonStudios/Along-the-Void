using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLinesController : MonoBehaviour
{
    [SerializeField] private CharacterMovement _player;
    [SerializeField] private Material _mat; 
   
    void Update()
    {
        float _lineDecety = Mathf.Clamp(_player.GetSpeedInPercent(), 0, 0.8f); 
        _mat.SetFloat("_LineDensity", _lineDecety); 
    }
}
