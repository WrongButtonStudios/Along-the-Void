using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    private EnemyStatusEffect _statusEffects;
    private SimpleAI _enemy;
    private bool _dealDamage = true;
    [SerializeField]
    private bool _isSlimeBall = false; 

    private void Start()
    {
        if (!_isSlimeBall)
        {
            _statusEffects = this.GetComponent<EnemyStatusEffect>();
            _enemy = this.GetComponent<SimpleAI>();
        }
    }

    public void Init(EnemyStatusEffect status, SimpleAI aI )
    {
        _statusEffects = status;
        _enemy = aI; 
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_dealDamage)
        {
             characterController player = collision.GetComponent<characterController>();
            
             if (player != null)
             {
                 player.Collision.GetDamage(0.35f, player.Collision.GetPlayerColor());
                StartCoroutine(DamageCooldown(0.25f)); 
             }
        }
    }

    private IEnumerator DamageCooldown(float time)
    {
        _dealDamage = false;
        yield return new WaitForSeconds(time);
        _dealDamage = true; 
    }
}
