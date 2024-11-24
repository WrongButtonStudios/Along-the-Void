using UnityEngine;
using System.Collections.Generic;
public class BluePlatform : MonoBehaviour
{
    public int speed = 5;
    public List<Vector2> waypoints = new List<Vector2>();
    int currentWaypoint = 0;
    public bool forward;
    private characterController playerController;
    Collider2D myCollider;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<characterController>();
        myCollider = GetComponent<Collider2D>();

        foreach (Transform child in transform)
        {
            waypoints.Add(child.transform.position);
        }
    }

    void Update()
    {
        if (playerController.getPlayerStatus().currentState == characterController.playerStates.blue || playerController.getPlayerStatus().currentState == characterController.playerStates.burntBlue)
        {
            myCollider.enabled = true;
        }

        else
        {
            myCollider.enabled = false;
        }

        Vector2 newPosition = Vector2.MoveTowards(rb.position, waypoints[currentWaypoint], speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        if (forward && Vector2.Distance(rb.position, waypoints[currentWaypoint]) < 0.1f && currentWaypoint < waypoints.Count)
        {
            Debug.Log(currentWaypoint);
            currentWaypoint++;
            if (currentWaypoint == waypoints.Count)
            {
                forward = false;
                currentWaypoint--;
                Debug.Log("Backwards");
            }
        }

        if (!forward && currentWaypoint > 0 && Vector2.Distance(rb.position, waypoints[currentWaypoint]) < 0.1f)
        {
            currentWaypoint--;
            Debug.Log("Further backwards");
            if (currentWaypoint == 0)
            {
                forward = true;
                Debug.Log("Forwards again");
            }
        }
    }
}
