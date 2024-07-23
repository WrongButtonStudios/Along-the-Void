using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    [SerializeField] FadeToBlack fadeScript;
    [SerializeField] Transform spawnPoint;
    public PlayerController spawning;
    public bool willSpawn = false; 
   

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.transform.CompareTag("Player")) 
        {
            SoundManager.sndMan.PlayDeathSound();
            spawning.isDead = true;
            if (spawning.isDead)
            {
                fadeScript.FadeScreen(true,spawning);
            }

            willSpawn = true;
            spawning.Spawn();
            //collision.transform.position = spawnPoint.position;
            spawning.isDead = false;


            
        }
            
    }
    

}
