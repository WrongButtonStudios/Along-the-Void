using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anfang : MonoBehaviour
{

    public Animator animator;
    public PlayerController playerController;
    public bool isStarting = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarting == true)
        {
            animator.Play("fast fall loop");
        }
        if(playerController.isGrounded == true)
        {
            isStarting = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerController.isGrounded == false)
        {
            isStarting = true;
        }
    }
}
