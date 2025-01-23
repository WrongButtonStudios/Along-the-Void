using System.Collections.Generic;
using UnityEngine;

public class BluePlatform : MonoBehaviour
{
    public static BluePlatform Instance {
        get; private set;
    }
    public int speed = 5;
    private List<Vector2> waypoints = new List<Vector2>();
    int currentWaypoint = 0;
    public bool forward = true;
    private characterController playerController;
    Collider2D myCollider;
    private Rigidbody2D rb;
    private Vector2 _lastPosition;
    private Vector2 _startPos;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        _startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<characterController>();
        myCollider = GetComponent<Collider2D>();
        foreach(Transform child in transform) {
            waypoints.Add(child.transform.position);
            Debug.Log("Waypoint added");
        }
    }

    public void ResetPosition() {
        transform.position = _startPos;
    }

    void FixedUpdate() //bewegungen immer im FixedUpdate
    {
        if(playerController.getPlayerStatus().currentState == characterController.playerStates.blue || playerController.getPlayerStatus().currentState == characterController.playerStates.burntBlue) {
            myCollider.enabled = true;
            Vector2 newPosition = Vector2.MoveTowards(rb.position,waypoints[currentWaypoint],speed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }
        else {
            myCollider.enabled = false;
        }

        if(forward && Vector2.Distance(rb.position,waypoints[currentWaypoint]) < 0.1f && currentWaypoint < waypoints.Count) {
            Debug.Log(currentWaypoint);
            currentWaypoint++;
            if(currentWaypoint == waypoints.Count) {
                forward = false;
                currentWaypoint--;
                Debug.Log("Backwards");
            }
        }

        if(!forward && currentWaypoint > 0 && Vector2.Distance(rb.position,waypoints[currentWaypoint]) < 0.1f) {
            currentWaypoint--;
            Debug.Log("Further backwards");
            if(currentWaypoint == 0) {
                forward = true;
                Debug.Log("Forwards again");
            }
        }
        _lastPosition = rb.position;
    }

    public Vector2 GetDeltaPosition() {
        return rb.position - _lastPosition;
    }
}