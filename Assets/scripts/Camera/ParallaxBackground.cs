using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float parallaxSpeed = 2f;
    private float startPosition;
    private float length;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float distanceMoved = mainCamera.transform.position.x;
        float parallaxEffect = (distanceMoved * parallaxSpeed);

        transform.position = new Vector3(startPosition + parallaxEffect, transform.position.y, transform.position.z);
    }
}