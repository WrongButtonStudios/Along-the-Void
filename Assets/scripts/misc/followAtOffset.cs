using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followAtOffset : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 offset;


    private void Update()
    {
        this.transform.position = targetTransform.position + offset;
    }
}
