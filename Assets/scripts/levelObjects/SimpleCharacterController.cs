using UnityEngine;

public class SimpleCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float moveForce = 50f;  // Neue Variable für die Bewegungskraft
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        Jump();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // Anstatt die Geschwindigkeit direkt zu setzen, wenden wir eine Kraft an
        rb.AddForce(new Vector2(moveInput * moveForce, 0));

        // Begrenzen der maximalen Horizontalgeschwindigkeit
        float clampedVelocityX = Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed);
        rb.velocity = new Vector2(clampedVelocityX, rb.velocity.y);
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }
}