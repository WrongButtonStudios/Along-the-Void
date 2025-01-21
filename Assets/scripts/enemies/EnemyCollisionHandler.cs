using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{

    private Enemy _statusEffects;
    private BehaviourStateHandler _enemy;
    private Health _health;
    private bool _dealDamage = true;
    [SerializeField]
    private bool _isSlimeBall = false;
    [SerializeField]
    private LayerMask _ignoreLayer; 


    private void Start()
    {
        if (!_isSlimeBall)
        {
            _statusEffects = this.GetComponent<Enemy>();
            _enemy = this.GetComponent<BehaviourStateHandler>();
            _health = this.GetComponent<Health>(); 
        }
    }

    public void Init(Enemy status, BehaviourStateHandler aI )
    {
        _statusEffects = status;
        _enemy = aI; 
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterController player = collision.GetComponent<characterController>();
        if (_dealDamage)
        {
            if (player != null)
            {
                PlayerDamageHandler.GetDamage(0.35f, PlayerUttillitys.GetPlayerColor(player), FindAnyObjectByType<fairyController>()); //this is not final, because with that multiplayer wouldnt work!
                StartCoroutine(DamageCooldown(0.25f)); 
            }
        }

        if (player != null)
        {
            var warmode = player.gameObject.GetComponent<Warmodes>();
            bool playerusesRedWarmode = warmode.CurWarMode == characterController.playerStates.burntRed && warmode.IsActive; 
            if (playerusesRedWarmode)
            {
                _statusEffects.BurnEnemy(); 
            }
        }

        if (collision.gameObject.GetComponent<IceBullet>())
        {
            _statusEffects.FreezeEnemy();
            _health.GetDamage(0.1f); 
        }
    }

    private IEnumerator DamageCooldown(float time)
    {
        _dealDamage = false;
        yield return new WaitForSeconds(time / PhysicUttillitys.TimeScale);
        _dealDamage = true; 
    }

    public bool IsGrounded(float groundDist = 1.25f)
    {
        if (Physics2D.Raycast(transform.position, -Vector2.up, groundDist, ~_ignoreLayer))
        {
            return true;
        }
        return false;
    }

    public bool CheckForObstacle(float xDirection)
    {
        Vector3 rayOrigin = transform.position - new Vector3(0, 0.5f, 0);
        float rayDistance = 1f;

        RaycastHit2D hitLow = Physics2D.Raycast(rayOrigin, transform.right * xDirection, rayDistance, ~_ignoreLayer);
        RaycastHit2D hitMid = Physics2D.Raycast(transform.position, transform.right * xDirection, rayDistance, ~_ignoreLayer);
        if (hitLow || hitMid)
            return true;

        return false;
    }
}
