using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDrop : MonoBehaviour
{
    private fairyController _fairyController;
    private bool _allreadyUsed = false;
    private void Awake()
    {
        _fairyController = FindObjectOfType<fairyController>();
        if (!_fairyController)
            throw new System.NullReferenceException("There is no FairyController in Scene!"); 
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_allreadyUsed)
        {
            _allreadyUsed = true; 
            HealLowestFairy(); 
        }
    }
    private void HealLowestFairy()
    {
        int fairyIndexToHeal = 0;
        int i = 0; 
        float lowestHP = float.MaxValue;
        foreach (fairy.fairyData hp in _fairyController.fairys)
        {
            if (lowestHP > hp.colorAmount)
            {
                fairyIndexToHeal = i; 
                lowestHP = hp.colorAmount;
            }
            i++;
        }

        _fairyController.fairys[fairyIndexToHeal].colorAmount = 1f;
        Destroy(this.gameObject); 
    }
}
