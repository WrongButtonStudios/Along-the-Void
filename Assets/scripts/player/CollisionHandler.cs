using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class CollisionHandler : MonoBehaviour
{
    private fairyController _fairyController = null;
    private characterController _cc; 
    private CharachterMovement _movement;

    [SerializeField]
    private LayerMask _groundLayer; 
    private enum PlayerColor 
    {
        red, 
        green, 
        blue, 
        yellow,
        unknown
    }

    private void Start()
    {
        _fairyController = GameObject.FindObjectOfType<fairyController>();
        _cc = this.gameObject.GetComponent<characterController>();
        _movement = this.GetComponent<CharachterMovement>(); 
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

    public void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        Debug.Log("trigger entered"); 
        var playerColor = GetPlayerColor();
        
        if (collision.gameObject.layer == 19 && playerColor == PlayerColor.red) 
        {
            Debug.Log("Autsch roter spike"); 
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name)
            for(int i = 0; i < _fairyController.fairys.Capacity; ++i)
            {
                if (_fairyController.fairys[i].color == fairy.fairyColor.red)
                {
                    //deale damage
                    _fairyController.fairys[i].colorAmount -= 0.25f;
                    Debug.Log(_fairyController.fairys[i].colorAmount); 
                    break; 
                }
            }
        }
    }

    private PlayerColor GetPlayerColor() 
    {
        if (_cc.getPlayerStatus().currentState == characterController.playerStates.red || _cc.getPlayerStatus().currentState == characterController.playerStates.burntRed)
        {
            return PlayerColor.red;
        }
        else if (_cc.getPlayerStatus().currentState == characterController.playerStates.green || _cc.getPlayerStatus().currentState == characterController.playerStates.burntGreen)
        {
            return PlayerColor.green;
        }
        else if (_cc.getPlayerStatus().currentState == characterController.playerStates.blue || _cc.getPlayerStatus().currentState == characterController.playerStates.burntBlue)
        {
            return PlayerColor.blue;
        }
        else if (_cc.getPlayerStatus().currentState == characterController.playerStates.yellow || _cc.getPlayerStatus().currentState == characterController.playerStates.burntYellow)
        {
            return PlayerColor.yellow;
        }
        else
            return PlayerColor.unknown; 
    }
}
