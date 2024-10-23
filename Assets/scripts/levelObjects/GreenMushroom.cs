using UnityEngine;

public class GreenMushroom : MonoBehaviour
{
    public float bounce = 20f;
    public float bounceDirectionX = 1f;
    public float bounceDirectionY = 1f;
    public int flightPathSteps = 50;
    public float simulationTime = 2f;
    public bool useForceMode = false;  // Toggle zwischen Geschwindigkeit und Kraft

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 bounceDirection = new Vector2(bounceDirectionX, bounceDirectionY).normalized;
                Vector2 force = bounceDirection * bounce;

                if (useForceMode)
                {
                    playerRb.AddForce(force, ForceMode2D.Impulse);
                    Debug.Log($"Applied force: {force}");
                }
                else
                {
                    playerRb.velocity = force;
                    Debug.Log($"Set velocity: {force}");
                }

                Debug.Log($"Player mass: {playerRb.mass}, gravity scale: {playerRb.gravityScale}");
                Debug.Log($"Collision normal: {collision.GetContact(0).normal}");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 startVelocity = new Vector2(bounceDirectionX, bounceDirectionY).normalized * bounce;
        Vector3 startPosition = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPosition, 0.2f);

        Vector3 previousPosition = startPosition;
        float timeStep = simulationTime / flightPathSteps;

        for (int i = 1; i <= flightPathSteps; i++)
        {
            float t = i * timeStep;
            Vector3 newPosition = CalculatePosition(startPosition, startVelocity, t);

            Gizmos.color = Color.Lerp(Color.yellow, Color.red, (float)i / flightPathSteps);
            Gizmos.DrawLine(previousPosition, newPosition);
            Gizmos.DrawSphere(newPosition, 0.1f);

            previousPosition = newPosition;
        }
    }

    private Vector3 CalculatePosition(Vector3 start, Vector2 initialVelocity, float time)
    {
        Vector2 gravity = Physics2D.gravity;
        return new Vector3(
            start.x + initialVelocity.x * time,
            start.y + initialVelocity.y * time + 0.5f * gravity.y * time * time,
            start.z
        );
    }
}