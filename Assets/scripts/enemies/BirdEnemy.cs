using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdEnemy : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Transform _playerPos;
    [SerializeField]
    private float _dashSpeedFactor = 1.5f;
    [SerializeField]
    private GameObject _slimeball;
    [SerializeField]
    private Transform _slimeballSpawner;


    private Rigidbody2D _rb;
    private bool _canSpitSlimeball = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>(); 
    }
    public void Attack()
    {
        int randomAttack = Random.Range(0, 1);

            if (randomAttack == 0)
                DashAttack();
            else
                SlimeBallAttack();
    }

    public  void Movement(Vector3 targetPos)
    {
        _rb.AddForce(CalculateMovementForce(targetPos) * _speed * Time.fixedDeltaTime, ForceMode2D.Impulse); 
    }

    private Vector2 CalculateMovementForce(Vector3 targetPos)
    {
        Vector2 dir = (targetPos - transform.position).normalized;
        Vector2 force = dir * _speed;
        return force;
    }


    //Nahkampf Attacke 
    private void DashAttack()
    {
        _rb.AddForce(CalculateMovementForce(_playerPos.position) * (_speed*_dashSpeedFactor) * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    //Fernkampf Attacke
    private void SlimeBallAttack()
    {
        _rb.velocity = Vector2.zero;
        if (_canSpitSlimeball)
        {
            _canSpitSlimeball = false; 
             var slimeball = Instantiate(_slimeball, _slimeballSpawner.position, Quaternion.identity);
             var slimeballRb = slimeball.GetComponent<Rigidbody2D>();
             if (slimeballRb == null)
                 slimeballRb = slimeball.AddComponent<Rigidbody2D>();
             Vector2 force = transform.right * (-1 * Mathf.Sign(_rb.velocity.x) * 5f); 
             slimeballRb.AddForce(force, ForceMode2D.Impulse);
            StartCoroutine(CoolDown()); 
        }
    }
    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(1);
        _canSpitSlimeball = true; 
    }

    public void Haunt()
    {
        Vector2 targetPos = _playerPos.position + (Vector3.up * 3);
        _rb.AddForce(CalculateMovementForce(targetPos) * _speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
}
