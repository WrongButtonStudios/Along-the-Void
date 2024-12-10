using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class CollisionHandler : MonoBehaviour
{
    private fairyController _fairyController = null;
    private characterController _cc; 
    private CharacterMovement _movement;
    private Warmodes _warmode; 

    [SerializeField]
    private LayerMask _groundLayer;

    private void Start()
    {
        _fairyController = GameObject.FindObjectOfType<fairyController>();
        _cc = this.gameObject.GetComponent<characterController>();
        _movement = this.GetComponent<CharacterMovement>();
        _warmode = this.GetComponent<Warmodes>(); 
    }

    public RaycastHit2D doGroundedCheck()
    {
        _cc.StatusData.isGrounded = checkGrounded(out RaycastHit2D groundHit);

        if (_cc.StatusData.isGrounded)
        {
            transform.up = groundHit.normal * _cc.rb.gravityScale;
        }
        else
        {
            transform.up = Vector2.Lerp(transform.up, Vector2.up, Time.deltaTime * _movement.InAirTurnSpeed);

            //this ads downwards force to make the gravity more gamey. does alot for gamefeel
            if (_cc.rb.velocity.y * _cc.rb.gravityScale < 0)
            {
                _cc.rb.AddForce((Physics2D.gravity * _cc.rb.gravityScale) * _movement.DeccendGravityMultiplier, ForceMode2D.Force);
            }
        }

        return groundHit;
    }

    public bool checkGrounded(out RaycastHit2D hit)
    {
        hit = Physics2D.Raycast(transform.position, -transform.up * _cc.rb.gravityScale, Mathf.Infinity, _groundLayer);

        if (hit.collider != null)
        {
            if (_cc.StatusData.isGrounded)
            {
                return hit.distance <= _movement.MaxRideHight;
            }
            else
            {
                return hit.distance <= _movement.GroundDistance;
            }
        }

        return false;
    }

    private void OnCollisionStay2D()
    {
        _cc.StatusData.isDash = false;
    }

    public void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        Debug.Log("trigger entered");         
        if (collision.gameObject.layer == 19 && PlayerUttillitys.GetPlayerColor(_cc) == PlayerColor.red) 
        {
            Debug.Log("Autsch roter spike");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name)
            PlayerDamageHandler.GetDamage(0.25f, PlayerColor.red, _fairyController);
        }

        bool destroyObstacle = collision.gameObject.layer == 14 && _warmode.CurWarMode == characterController.playerStates.burntRed && _warmode.IsActive; 
        if (destroyObstacle)
        {
            Destroy(collision.gameObject); 
        }
    }
}
