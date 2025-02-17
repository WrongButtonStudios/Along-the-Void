using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicUttillitys : MonoBehaviour
{
    public static float TimeScale = 1f; 

    public static Vector2 ClampVelocity(Vector2 curVel, float maxVel)
    {
        if (curVel.magnitude >= maxVel)
        {
            return curVel.normalized * (maxVel * TimeScale);
        }
        return curVel;
    }

    public static float GetDirectionMofifyer(Vector2 start, Vector2 end) 
    {
        return Mathf.Sign(end.x - start.x); 
    } 
}
