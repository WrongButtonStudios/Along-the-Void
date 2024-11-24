using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class blueSlime : MonoBehaviour
{
    public struct line
    {
        public Vector2 pointA;
        public Vector2 pointB;
    }

    public Transform pointA;
    public Transform pointB;

    private line myLine;

    public void Start()
    {
        myLine = new line();

        myLine.pointA = pointA.position;
        myLine.pointB = pointB.position;
    }

    public line getLine()
    {
        return myLine;
    }
}
