using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PilzDetector : MonoBehaviour
{
    
    public bool isGreenShining;
    [SerializeField] PauseMenue pauseMenue;





    private void OnTriggerStay2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "ColorDetector" && !ColorManager.Instance.greenIsActive && ColorManager.Instance.redIsActive ||
            collision.gameObject.tag == "ColorDetector" && ColorManager.Instance.greenIsActive && !ColorManager.Instance.redIsActive)
        {

            isGreenShining = true;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ColorDetector" && !ColorManager.Instance.greenIsActive ||
          collision.gameObject.tag == "ColorDetector" && ColorManager.Instance.greenIsActive)

        {
            isGreenShining = false;
        }
    }



    void Start()
    {
        pauseMenue = GameObject.FindObjectOfType<PauseMenue>();
    }

    void Update()
    {
        if (pauseMenue.GameIsPaused)
        {
            return;
        }
        ColorManager.Instance.Green();
        ColorManager.Instance.Red();
        //Green.Blue();
        //Green.Yellow();
    }
}
