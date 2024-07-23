using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllColorController: MonoBehaviour
{
    PlayerColorSwitch playerColorSwitch;

    public float Direction = 0f;// erstellt einen privaten Float namens "Direction" auf 0
    //public float DirectionVertical = 0f; // erstellt einen privaten Float für namens "DirectionVertical" auf 0

    [SerializeField] private Vector3 redSize = new Vector3(0.1f, 0.1f, 1f);


    private Vector3 startSize;

    public Material[] material;
    public Renderer rend2;

    public bool isFacingRight = true;

    [SerializeField] private Rigidbody2D Player;

    void Start()
    {
        Player = GetComponent<Rigidbody2D>(); // Erstellt am Anfang des Games einen Bezug zum Rigidbody namens "Player"
        rend2 = GetComponent<Renderer>();
        rend2.enabled = true;
        //rend2.sharedMaterial = material[0];
        startSize = transform.localScale;


    }



    void Update()
    {



        if (rend2.sharedMaterial = material[0])
        {

            //    playerColorSwitch.rend.sharedMaterial = (rend2.sharedMaterial);
            Debug.Log("Fischsuppe");
    }
        else 
        { Debug.Log("NöööÖ"); }

        //if (playerColorSwitch.rend.sharedMaterial = material[1])
        //{

        //    //rend2.sharedMaterial = material[1];
        //    Debug.Log("Fischsuppe2");
        //}

        //Direction = Input.GetAxisRaw("Horizontal");


        //if (Input.GetKeyUp(KeyCode.K) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        //{
        //    rend2.sharedMaterial = material[1];
        //    //ChangeSize(redSize);

        //}

        ////Grüne Fähigkeit
        //if (Input.GetKeyUp(KeyCode.J) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        //{
        //    rend2.sharedMaterial = material[0];
        //    //ChangeSize(startSize);
        //}
    }
    //private void FixedUpdate()
    //{
    //    if (playerColorSwitch.rend.sharedMaterial = material[0])
    //    {
            
    //        rend2.sharedMaterial = material[0];
    //    }
    //    if (playerColorSwitch.rend.sharedMaterial = material[1])
    //    {

    //        rend2.sharedMaterial = material[1];
    //    }
    //}
}