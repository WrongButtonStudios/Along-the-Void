using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private SimpleAI _entity;

    [SerializeField]
    private float _maxJumpHight = 2.5f; 
    private void Awake()
    {
        _entity = this.GetComponent<SimpleAI>(); 
    }

    public bool Jump()
    {
        if (_entity.transform.position.y < _maxJumpHight)
        {
            Move(Vector2.up, _entity.JumpForce); 
            return true; 
        }
        return false; 
    }

    public void Move(Vector2 dir)
    {
        _entity.RB.velocity += dir.normalized * (_entity.Speed * (Time.fixedDeltaTime * PhysicUttillitys.TimeScale)); 
    }


    public void Move(Vector2 dir, float speed)
    {
        _entity.RB.velocity += dir.normalized * (speed * (Time.fixedDeltaTime * PhysicUttillitys.TimeScale));
    }
}