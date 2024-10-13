//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerHealth : MonoBehaviour
//{

//    public int health;
//    private int maxHealth = numberOfFairies;
//    private int healthPerFairy = 100;

//    public PlayerData numberOfGreenFairy;
//    public PlayerData numberOfRedFairy;
//    public PlayerData numberOfBlueFairy;
//    public PlayerData numberOfYellowFairy;
//    static PlayerData numberOfFairies;



//    private void Start()
//    {
//        health = maxHealth;
//    }

//    private void Update()
//    {
//        if(health >= maxHealth)
//        {
//            health = maxHealth;
//        }
//    }

//    public void PlayerHeal(int amount)
//    {
//        health += amount;
//    }

//    public void DMGOnPlayer(int amount)
//    {
//        health -= amount;
//        if (health < 0)
//        {
//            Destroy(gameObject);
//        }
//    }


//}
