using UnityEngine;
public class Mushroom : MonoBehaviour
{
    [ExecuteAlways]
    public float JumpForce = 25f;
    [SerializeField] private GameObject player;
    private characterController playerController;
    private void Start()
    {
        playerController = player.GetComponent<characterController>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure there is a GameObject tagged as 'Player'.");
        }
    }
    private void Update()
    {
        if (playerController.getPlayerStatus().currentState == characterController.playerStates.green)
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }

        else
        {
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerController.getPlayerStatus().currentState == characterController.playerStates.green && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
        }
    }

    public int resolution = 30;
    public float timeStep = 0.2f;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        // Get the player's Rigidbody2D to calculate trajectory correctly
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (player == null)
        {
            Debug.LogWarning("Player is not assigned in TrajectoryLine script!");
            return;
        }
        if (playerRb == null)
        {
            Debug.Log("No Rigidbody2D found on Player for trajectory calculation!");
        }
        // Get the starting position and velocity
        Vector2 startPosition = transform.position;
        Vector2 startVelocity = transform.up * JumpForce;
        // Simulate trajectory
        Vector2 previousPosition = startPosition;
        for (int i = 1; i <= resolution; i++)
        {
            float time = timeStep * i;
            Vector2 currentPosition = CalculatePositionAtTime(startPosition, startVelocity, time);
            // Draw a line segment between the previous and current position
            Gizmos.DrawLine(previousPosition, currentPosition);
            previousPosition = currentPosition;
        }
    }
    private Vector2 CalculatePositionAtTime(Vector2 startPosition, Vector2 startVelocity, float time)
    {
        Vector2 gravity = Physics2D.gravity; // Use 2D gravity
        return startPosition + (startVelocity * time) + (0.5f * gravity * time * time);
    }
}