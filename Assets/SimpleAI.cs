using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleAI : MonoBehaviour
{
    private enum State 
    {
        Patrol, 
        Hount, 
        Attack
    }

    State _curState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ExecuteState() 
    {
        switch(_curState)
        {
            case State.Patrol:
                Patrol(); 
                break;
            case State.Hount:
                Hount(); 
                break;
            case State.Attack:
                Attack(); 
                break;

        }
    }

    private void Patrol() 
    {
        throw new System.NotImplementedException(); 
    }

    private void Hount()
    {
        throw new System.NotImplementedException();
    }

    private void Attack()
    {
        throw new System.NotImplementedException();
    }
}
