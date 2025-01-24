using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDrop : MonoBehaviour
{
    private fairyController _fairyController;
    private void Awake()
    {
        _fairyController = FindObjectOfType<fairyController>();
        if (!_fairyController)
            throw new System.NullReferenceException("There is no FairyController in Scene!"); 
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealLowestFairy(); 
        }
    }
    private void HealLowestFairy()
    {
        int i = 0; 
        float lowestHP = float.MaxValue;
        fairy.fairyData fairyToHeal = new fairy.fairyData(); 
        foreach (fairy.fairyData hp in _fairyController.fairys)
        {
            if (lowestHP < hp.colorAmount)
            {
                Debug.Log("Bin raus " + i); 
                continue;
            }
            i++; 
            lowestHP = hp.colorAmount; 
        }

        _fairyController.fairys[i].colorAmount = 1f;
        Debug.Log("Habe Fairy: " + _fairyController.fairys[i].color + "Aufgefüllt"); 
        Destroy(this.gameObject); 
    }
}
