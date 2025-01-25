using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerStompAttack : MonoBehaviour, IplayerFeature
{
    private characterController characterController;

    [SerializeField] private float downForce = 300f;
    [SerializeField] private float maxSpeed = 150f;
    [SerializeField] private CharacterMovement _movement;
    [SerializeField] private float _strenght = 2.5f; 
    public bool UseStompAttack { get; private set; }

    private bool doStompAttack = false;

    private void Start()
    {
        _movement = this.GetComponent<CharacterMovement>(); 
    }

    public void FixedUpdate()
    {
        if(characterController.getPlayerStatus().isGrounded)
        {
            endFeature();
        }

        if(doStompAttack)
        {
            _movement.setMaxSpeed(maxSpeed);
            UseStompAttack = true; 
            characterController.rb.AddForce(Vector2.down * downForce, ForceMode2D.Force);
        }
    }

    public void initFeature(characterController characterController)
    {
        this.characterController = characterController;
    }

    public void triggerFeature(bool useInput = false, bool input = false)
    {
        if(!useInput)
        {
            Debug.LogError("this feature needs input to work!");
            return;
        }

        if(!characterController.getPlayerStatus().isGrounded)
        {
            doStompAttack = input;
        }
    }
    public void DealDamage()
    {
        if (UseStompAttack==false)
        {
            return;
        }
        CameraShake.Instance.ShakeCamera(_strenght); 
        Debug.Log("Will damage austeilen"); 
        UseStompAttack = false;
        List<Collider2D> colliders = new();
        ContactFilter2D filter = new();
        filter.NoFilter();
        Physics2D.OverlapCircle(transform.position, 5f, filter, colliders);
        foreach (Collider2D col in colliders)
        {
            var enemy = col.gameObject.GetComponent<Health>();
            if (enemy == null)
            {
                continue;
            }
            enemy.GetDamage(10f);
        }
    }
    public void endFeature()
    {
        doStompAttack = false;
    }
}
