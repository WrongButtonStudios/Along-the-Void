using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; 

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    [SerializeField] private CinemachineImpulseSource _impulse; 
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this); 
        }
        else
        {
            Destroy(this.gameObject); 
        }
    }

    public void ShakeCamera(float magnitude)
    {
        _impulse.GenerateImpulse(magnitude); 
    }
}
