using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class CollisionHandler : MonoBehaviour
{
    public void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        Debug.Log("trigger entered"); 
        if (CompareName("SpikeRed", collision.gameObject)) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        }
    }

    private bool CompareName(string name, GameObject objToCompare) 
    {
        string otherName  = string.Empty;
        char[] fullName = objToCompare.name.ToCharArray(); 
        for(int i = 0; i < name.Length; ++i)
        {
            otherName += fullName[i]; 
        }
        return otherName==name; 
    }
}
