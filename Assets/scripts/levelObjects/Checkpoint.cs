using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private DeathZone deathZone;
    private void Start()
    {
        deathZone = FindObjectOfType<DeathZone>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            deathZone.respawnPos = this.transform.position;
        }
    }
}