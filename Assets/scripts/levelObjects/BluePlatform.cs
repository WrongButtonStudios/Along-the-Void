using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class BluePlatform : MonoBehaviour
{
    public int speed = 5;
    public List<Vector3> waypoints = new List<Vector3>();
    int currentWaypoint = 0;
    bool vorwärts = true;
    void Start()
    {
        foreach (Transform child in transform)
        {
            waypoints.Add(child.transform.position);
        }
    }

    void Update()
    {
        if (currentWaypoint < waypoints.Count && vorwärts)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint], Time.deltaTime * speed);

            if (transform.position == waypoints[currentWaypoint])
            {
                currentWaypoint++;
                Console.WriteLine(currentWaypoint);
            }
        }
        if (currentWaypoint == waypoints.Count)
        {
            vorwärts = false;
            currentWaypoint--;
            Console.WriteLine(currentWaypoint);
        }
        if (currentWaypoint > waypoints.Count && !vorwärts)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint], Time.deltaTime * speed);

            if (transform.position == waypoints[currentWaypoint])
            {
                currentWaypoint--;
                Console.WriteLine(currentWaypoint);
            }
        }
        if (currentWaypoint == 0)
        {
            vorwärts = true;
            Console.WriteLine(currentWaypoint);
        }



    }
}
