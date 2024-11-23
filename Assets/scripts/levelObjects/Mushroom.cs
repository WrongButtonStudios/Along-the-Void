using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public float JumpForce = 25f;
    [SerializeField] private GameObject player;
    private characterController playerController;
    private void Start()
    {
        playerController = player.GetComponent<characterController>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerController.getPlayerStatus().currentState == characterController.playerStates.green && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
        }
    }
}