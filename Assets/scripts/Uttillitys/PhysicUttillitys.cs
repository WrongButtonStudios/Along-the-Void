using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicUttillitys : MonoBehaviour
{
    public static float TimeScale = 1f; 

    public static Vector2 ClampVelocityEnemy(SimpleAI entity)
    {
        if (entity.RB.velocity.magnitude >= entity.Speed)
        {
            return entity.RB.velocity.normalized * entity.Speed * TimeScale;
        }
        return entity.RB.velocity;
    }
}
