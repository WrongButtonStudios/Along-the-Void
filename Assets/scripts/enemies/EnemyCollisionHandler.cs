using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    private EnemyStatusEffect _statusEffects;
    private SimpleAI _enemy; 

    private void Start()
    {
        _statusEffects = this.GetComponent<EnemyStatusEffect>();
        _enemy = this.GetComponent<SimpleAI>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterController player = collision.GetComponent<characterController>();
        if (player != null && _enemy.GetActiveAttackComponent().IsAttacking())
        {
            player.Collision.GetDamage(0.35f, player.Collision.GetPlayerColor()); 
        }
    }
}
