using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FairyHealth : MonoBehaviour
{
    [SerializeField] Slider[] sliderArray;
    [SerializeField] Slider healthSlider;
    public float maxHealth = 100;
    public float health;
    public bool fairyActive = false;
    public bool fairyDead = false;


    private void Start()
    {
        
        health = maxHealth;

        healthSlider.maxValue = maxHealth;
        // Initialize the slider's value to the current health
        healthSlider.value = health;

    }

    private void Update()
    {
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        if (!fairyActive)
        {
            //sliderArray[3+1];
        }

        //DamageOnPlayer(6942069);

        if (Input.GetKeyDown(KeyCode.Q)) // Take damage when pressing Space
        {
            DamageOnPlayer(10f);
        }

        if (Input.GetKeyDown(KeyCode.E)) // Heal when pressing H
        {
            PlayerHeal(5f);
        }
    }

    public void PlayerHeal(float amount)
    {
        health += amount;
        healthSlider.value = health;
    }

    public void DamageOnPlayer(float amount)
    {
        health -= amount;
        healthSlider.value = health;
        if (health <= 0)
        {
            fairyActive = false;
            fairyDead = true;
            health = 0;
        }
    }
    public class greenFairy : FairyHealth
    {
        
    }
}
