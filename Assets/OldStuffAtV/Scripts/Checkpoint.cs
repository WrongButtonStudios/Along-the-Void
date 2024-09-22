using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    public Animator animator;

    public bool CheckpointIsActive = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckpointIsActive == false && collision.CompareTag("CheckpointDetector"))
        {
            animator.SetBool("CheckpointIsActiveAnimation", true);
        }

        if (collision.CompareTag("Player"))
        {
            spawnPoint.position = this.transform.position;

        }
    }   
    void OnTriggerExit2D(Collider2D collision)
    {
        CheckpointIsActive = true;
        animator.Play("SafePointIdleFBF");
    }
}
