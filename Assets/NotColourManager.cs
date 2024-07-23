using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotColourManager : MonoBehaviour
{

    public ColorManager colorManager;
    public bool ncsIsTrue;
    //[SerializeField] PauseMenue pauseMenue;



    private void Start()
    {
        //pauseMenue = GameObject.FindObjectOfType<PauseMenue>();
    }
    public void ColourTrigger()
    {
        Debug.Log("ColourTrigger");
        if (ncsIsTrue == true && colorManager.greenIsActive)
        {
            Debug.Log("Green");
            colorManager.Green();
        }

        if (ncsIsTrue == true && colorManager.redIsActive)
        {
            Debug.Log("Reed");
            colorManager.Red();
        }

        //else if (ncs.isTriggerd == false && colorManager.greenIsActive)
        //{
        //    colorManager.Red();
        //}

        //else if (ncs.isTriggerd == false && colorManager.redIsActive)
        //{
        //    colorManager.Green();
        //}

    }

}