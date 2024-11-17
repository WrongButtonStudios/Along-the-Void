using System.Runtime.CompilerServices;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Vector2 respawnPos = new Vector2(110, 110);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            characterController cc = collision.gameObject.GetComponent<characterController>();
            cc.rb.velocity = Vector2.zero;
            cc.rb.position = respawnPos;
            Debug.Log("Teleport!");
        }

    }
}