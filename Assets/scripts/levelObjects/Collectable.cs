using UnityEngine;

public class Collectable : MonoBehaviour, ICollectable
{
    public float moveSpeed = 5f;
    private Transform playerTransform;
    private bool moveToPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CollectZone"))
        {
            playerTransform = collision.transform;
            moveToPlayer = true;
        }

        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Update()
    {
        if (moveToPlayer && playerTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }


    public void Collect()
    {
        Debug.Log("Collected!");
        Destroy(gameObject);
    }
}