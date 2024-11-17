using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DeathZone : MonoBehaviour
{
    public Vector2 respawnPos = new Vector2(0, 0);
    public GameObject player;

    private void Start()
    {
        respawnPos = new Vector2(player.transform.position.x, player.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            characterController cc = collision.gameObject.GetComponent<characterController>();
            cc.rb.velocity = Vector2.zero;
            cc.rb.position = respawnPos;
        }
    }
}