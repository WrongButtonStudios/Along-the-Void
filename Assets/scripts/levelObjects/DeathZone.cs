using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DeathZone : MonoBehaviour
{
    public static DeathZone Instance {
        get; private set;
    }

    public Vector2 respawnPos = new Vector2(0,0);
    public GameObject player;

    private void Awake() {
        Instance = this;
    }


    private void Start() {
        respawnPos = new Vector2(player.transform.position.x,player.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player")) {
            Die();
            //characterController cc = collision.gameObject.GetComponent<characterController>();
            //cc.rb.velocity = Vector2.zero;
            //cc.rb.position = respawnPos;
        }
    }

    public void Die() {
        characterController cc = player.GetComponent<characterController>();
        cc.rb.velocity = Vector2.zero;
        cc.rb.position = respawnPos;
        fairyController.Instance.spawnFairys();
        BluePlatform.Instance.ResetPosition();
    }
}