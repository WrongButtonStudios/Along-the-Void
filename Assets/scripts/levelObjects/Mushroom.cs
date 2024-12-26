using UnityEngine;
using System.Collections; 
public class Mushroom : MonoBehaviour
{
    [ExecuteAlways]
    public float JumpForce = 25f;
    [SerializeField] private GameObject player;
    private characterController playerController;
    private bool _waitForEndOfDelay = false; 
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_waitForEndOfDelay)
        {
            if (playerController.getPlayerStatus().currentState == characterController.playerStates.green && other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
                StartCoroutine(Delay()); 
            }
        }
    }

    public int resolution = 30;
    public float timeStep = 0.02f; 
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (player == null)
        {
            Debug.LogWarning("Player is not assigned in TrajectoryLine script!");
            return;
        }

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb == null)
        {
            Debug.LogWarning("No Rigidbody2D found on Player for trajectory calculation!");
            return;
        }

        Vector2 startPosition = transform.position;
        Vector2 velocity = transform.up * JumpForce; // Initial velocity due to the impulse
        Vector2 gravity = Physics2D.gravity; // Use 2D gravity
        float time = 0; 
        Vector2 currentPosition = startPosition;

        for (int i = 0; i < resolution; i++)
        {
            // Simulate frame-by-frame movement
            time += Time.fixedDeltaTime;

            // Update position using current velocity
            Vector2 nextPosition = currentPosition + velocity;

            // Update velocity due to gravity
            velocity += gravity*time;

            // Draw line
            Gizmos.DrawLine(currentPosition, nextPosition);

            // Move to the next position
            currentPosition = nextPosition;
        }
    }
    private IEnumerator Delay()
    {
        _waitForEndOfDelay = true; 
        yield return new WaitForSeconds(0.3f);
        _waitForEndOfDelay = false;
    }
    private Vector2 CalculatePositionAtTime(Vector2 startPosition, Vector2 startVelocity, float time)
    {
        Vector2 gravity = Physics2D.gravity; // Use 2D gravity
        return startPosition + (startVelocity * time) + (0.5f * gravity * (time * time));
    }
}


/*
 
 
 
  
 
 
 
 */