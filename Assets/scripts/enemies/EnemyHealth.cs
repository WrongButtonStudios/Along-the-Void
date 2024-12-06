using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField]
    private float _health = 1;
    [SerializeField]
    private GameObject _colorItem;


    public void GetDamage(float damage)
    {
        Debug.Log("Autsch! Hab Schaden genommen!"); 
        _health -= damage;
        if (_health <= 0)
        {
            DropColor();
            Die(); 
        }
    }

    private void Die()
    {
        this.gameObject.SetActive(false); 
    }
    private void DropColor()
    {
        //to-do: exchange with object pooling
        Instantiate(_colorItem, transform.position, Quaternion.identity);
    }
}
