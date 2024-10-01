//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MovingPlatform : MonoBehaviour
//{
//    public float speed;
//    public int startingPoint;
//    public Transform[] points;
    

//    private int i;

//    public ColorManager blue;

//    private void Start()
//    {
//        transform.position = points[startingPoint].position;
//    }

//    private void Update()
//    {
       
//        if (blue.blueIsActive)
//        {
//            Moving();
//        }



//    }
//    private void OnCollisionStay2D(Collision2D collision)
//    {
//         collision.transform.SetParent(transform);
//    }

//    private void OnCollisionExit2D(Collision2D collision)
//    {
//        collision.transform.SetParent(null);
//    }

//    public void Moving()
//    {
//        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
//        {
//            i++;
//            if (i == points.Length)
//            {
//                i = 0;
//            }
//        }


//        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
//    }
//}
