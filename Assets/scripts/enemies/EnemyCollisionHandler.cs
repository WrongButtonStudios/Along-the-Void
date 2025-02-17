using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; 
public class EnemyCollisionHandler : MonoBehaviour
{
    [SerializeField] private bool _isSlimeBall = false;
    [SerializeField] private LayerMask _ignoreLayer;
    [SerializeField] private float _playerPushBackForce = 50f;
    [SerializeField] private float _cameraShakeStrength = 5f; 
    private Enemy _entity;
    private BehaviourStateHandler _enemy;
    private Health _health;
    private bool _dealDamage = true;
    private Rigidbody2D _rb;  

    private void Start()
    {
        if (!_isSlimeBall)
        {
            _entity = this.GetComponent<Enemy>();
            _enemy = this.GetComponent<BehaviourStateHandler>();
            _health = this.GetComponent<Health>(); 
        }
        _rb = this.GetComponent<Rigidbody2D>();
    }

    public void Init(Enemy status, BehaviourStateHandler aI )
    {
        _entity = status;
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
                player.rb.AddForce(transform.right * (_playerPushBackForce * PhysicUttillitys.GetDirectionMofifyer(transform.position, player.transform.position)), ForceMode2D.Impulse);
                CameraShake.Instance.ShakeCamera(_cameraShakeStrength); 
                StartCoroutine(DamageCooldown(0.25f)); 
            }
        }

        if (player != null)
        {
            var warmode = player.gameObject.GetComponent<Warmodes>();
            bool playerusesRedWarmode = warmode.CurWarMode == characterController.playerStates.burntRed && warmode.IsActive; 
            if (playerusesRedWarmode)
            {
                _entity.Debuffs.AddDebuff(Debuffs.Burning, 3.0f); 
            }
        }

        if (collision.gameObject.GetComponent<IceBullet>())
        {
            _entity.Debuffs.AddDebuff(Debuffs.Frozen, 3.0f);
            _health.GetDamage(0.1f); 
        }

        if (collision.gameObject.GetComponent<GreenBullet>())
        {
            Debug.Log("Enemy Gets Damage");
            _health.GetDamage(100f);
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
