using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StarText : MonoBehaviour
{
    public TextMeshProUGUI starText;
   

    

    public void IncrementStarCount(int starTotal) 
    {
       
        starText.text = $"{starTotal}";
    }
}
