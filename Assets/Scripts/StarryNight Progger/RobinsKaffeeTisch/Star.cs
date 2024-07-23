using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Star : MonoBehaviour, ICollectible
{
    public static event Action OnStarCollected;
    public Rigidbody2D starRB;
    public Animator starAnimation;

    bool hasTarget;
    Vector3 targetPosition;
    public float moveSpeed = 5f;

    private void Awake()
    {
        starRB = GetComponent<Rigidbody2D>();
    }
    public void Collect()
    {
        SoundManager.sndMan.PlayCollectedSound();
        Debug.Log("Star Collected");
        Destroy(gameObject);
        OnStarCollected?.Invoke();
    }
    private void FixedUpdate()
    {
        if (hasTarget)
        {
            starAnimation.enabled = false;
            Vector2 targetDirection = (targetPosition - transform.position).normalized;
            starRB.velocity = new Vector2(targetDirection.x, targetDirection.y)* moveSpeed;
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }
}
