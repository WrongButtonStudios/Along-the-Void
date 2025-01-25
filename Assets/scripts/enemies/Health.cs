using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _health = 1;
    [SerializeField] private GameObject _colorItem;

    public void GetDamage(float damage)
    {
        if (_health < 0)
        {
            return;
        }

        _health -= damage;
        if (_health <= 0)
        {
            Die(); 
        }
        
    }

    private void Die()
    {
        DropColor();
        this.gameObject.SetActive(false); 
    }

    private void DropColor()
    {
        Instantiate(_colorItem, transform.position, Quaternion.identity);
    }
}