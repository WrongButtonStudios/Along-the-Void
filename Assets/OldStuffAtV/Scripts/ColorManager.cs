using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ColorManager : MonoBehaviour
{

    private static ColorManager _instance;
    public static ColorManager Instance { get => _instance; }
    [SerializeField] PauseMenue pauseMenue;
    public Material[] Material;

    public PlayerController Size;
    //public NotColourManager ncm;

    public bool greenIsActive = false;
    public bool redIsActive = false;
    //public bool blueIsActive = false;
    //public bool yellowIsActive = false;
    public Vector3 redSize = new Vector3(0.1f, 0.1f, 1f);
    private Vector3 startSize = new Vector3(0.4f, 0.4f, 1f);

    public GameObject Object;
    public GameObject Object1;
    public GameObject Object2;
    public GameObject Object3;
    public GameObject Object4;
    public GameObject Object5;
    public GameObject Object6;
    public GameObject Object7;
    public GameObject Object8;
    public GameObject Object9;
    public GameObject Object10;
    public GameObject Object11;
    public GameObject Object12;
    public GameObject Object13;
    public GameObject Object14;
    public GameObject Object15;
    public GameObject Object16;
    public GameObject Object17;
    public GameObject Object18;



    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        pauseMenue = GameObject.FindObjectOfType<PauseMenue>();
        greenIsActive = true;


    }
    public void Green()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && !Size.isRedGrounded /*&& !ncm.ncsIsTrue*/ ||
            Input.GetKey(KeyCode.DownArrow) && !Size.isRedGrounded/* && !ncm.ncsIsTrue*/)
        {
            SoundManager.sndMan.PlayColorSound();
            redIsActive = false;
            //blueIsActive = false;
            //yellowIsActive = false;
            greenIsActive = true;
        }
    }
    public void Red()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1)/* && !ncm.ncsIsTrue */||
            Input.GetKey(KeyCode.RightArrow)/* && !ncm.ncsIsTrue*/)
        {
            SoundManager.sndMan.PlayColorSound();
            greenIsActive = false;
            //blueIsActive = false;
            //yellowIsActive = false;
            redIsActive = true;
        }
    }
    //public void Blue()
    //{
    //    if (Input.GetKeyDown(KeyCode.Joystick1Button2) && !Size.isRedGrounded || Input.GetKey("h") && !Size.isRedGrounded)
    //    {
    //        greenIsActive = false;
    //        redIsActive = false;
    //        yellowIsActive = false;
    //        blueIsActive = true;
    //    }
    //}
    //public void Yellow()
    //{
    //    if (Input.GetKeyDown(KeyCode.Joystick1Button3) && !Size.isRedGrounded || Input.GetKey("u") && !Size.isRedGrounded)
    //    {
    //        greenIsActive = false;
    //        redIsActive = false;
    //        blueIsActive = false;
    //        yellowIsActive = true;
    //    }
    //}


    private void Update()
    {
        if (pauseMenue.GameIsPaused)
        {
            return;
        }

        Green();
        Red();
        //Blue();
        //Yellow();



        if (greenIsActive)
        {
            Object.GetComponent<Renderer>().material = Material[0];
            Object1.GetComponent<Renderer>().material = Material[0];
            Object2.GetComponent<Renderer>().material = Material[0];
            Object3.GetComponent<Renderer>().material = Material[0];
            Object4.GetComponent<Renderer>().material = Material[0];
            Object5.GetComponent<Renderer>().material = Material[0];
            Object6.GetComponent<Renderer>().material = Material[0];
            Object7.GetComponent<Renderer>().material = Material[0];
            Object8.GetComponent<Renderer>().material = Material[0];
            Object9.GetComponent<Renderer>().material = Material[0];
            Object10.GetComponent<Renderer>().material = Material[0];
            Object11.GetComponent<Renderer>().material = Material[0];
            Object12.GetComponent<Renderer>().material = Material[0];
            Object13.GetComponent<Renderer>().material = Material[0];
            Object14.GetComponent<Renderer>().material = Material[0];
            Object15.GetComponent<Renderer>().material = Material[0];
            Object16.GetComponent<Renderer>().material = Material[0];
            Object17.GetComponent<Renderer>().material = Material[0];
            Object18.GetComponent<Renderer>().material = Material[0];


        }
        if (redIsActive)
        {
            Object.GetComponent<Renderer>().material = Material[1];
            Object1.GetComponent<Renderer>().material = Material[1];
            Object2.GetComponent<Renderer>().material = Material[1];
            Object3.GetComponent<Renderer>().material = Material[1];
            Object4.GetComponent<Renderer>().material = Material[1];
            Object5.GetComponent<Renderer>().material = Material[1];
            Object6.GetComponent<Renderer>().material = Material[1];
            Object7.GetComponent<Renderer>().material = Material[1];
            Object8.GetComponent<Renderer>().material = Material[1];
            Object9.GetComponent<Renderer>().material = Material[1];
            Object10.GetComponent<Renderer>().material = Material[1];
            Object11.GetComponent<Renderer>().material = Material[1];
            Object12.GetComponent<Renderer>().material = Material[1];
            Object13.GetComponent<Renderer>().material = Material[1];
            Object14.GetComponent<Renderer>().material = Material[1];
            Object15.GetComponent<Renderer>().material = Material[1];
            Object16.GetComponent<Renderer>().material = Material[1];
            Object17.GetComponent<Renderer>().material = Material[1];
            Object18.GetComponent<Renderer>().material = Material[1];


        }
        //if (blueIsActive)
        //{
        //    Object.GetComponent<Renderer>().material = Material[2];
        //    Object1.GetComponent<Renderer>().material = Material[2];
        //    Object2.GetComponent<Renderer>().material = Material[2];
        //    Object3.GetComponent<Renderer>().material = Material[2];
        //    Object4.GetComponent<Renderer>().material = Material[2];
        //    Object5.GetComponent<Renderer>().material = Material[2];
        //    Object6.GetComponent<Renderer>().material = Material[2];
        //    Object7.GetComponent<Renderer>().material = Material[2];
        //    Object8.GetComponent<Renderer>().material = Material[2];
        //    Object9.GetComponent<Renderer>().material = Material[2];
        //    Object10.GetComponent<Renderer>().material = Material[2];
        //    Object11.GetComponent<Renderer>().material = Material[2];
        //    Object12.GetComponent<Renderer>().material = Material[2];
        //    Object13.GetComponent<Renderer>().material = Material[2];
        //    Object14.GetComponent<Renderer>().material = Material[2];
        //    Object15.GetComponent<Renderer>().material = Material[2];
        //    Object16.GetComponent<Renderer>().material = Material[2];
        //    Object17.GetComponent<Renderer>().material = Material[2];
        //    Object18.GetComponent<Renderer>().material = Material[2];


        //}
        //if (yellowIsActive)
        //{
        //    Object.GetComponent<Renderer>().material = Material[3];
        //    Object1.GetComponent<Renderer>().material = Material[3];
        //    Object2.GetComponent<Renderer>().material = Material[3];
        //    Object3.GetComponent<Renderer>().material = Material[3];
        //    Object4.GetComponent<Renderer>().material = Material[3];
        //    Object5.GetComponent<Renderer>().material = Material[3];
        //    Object6.GetComponent<Renderer>().material = Material[3];
        //    Object7.GetComponent<Renderer>().material = Material[3];
        //    Object8.GetComponent<Renderer>().material = Material[3];
        //    Object9.GetComponent<Renderer>().material = Material[3];
        //    Object10.GetComponent<Renderer>().material = Material[3];
        //    Object11.GetComponent<Renderer>().material = Material[3];
        //    Object12.GetComponent<Renderer>().material = Material[3];
        //    Object13.GetComponent<Renderer>().material = Material[3];
        //    Object14.GetComponent<Renderer>().material = Material[3];
        //    Object15.GetComponent<Renderer>().material = Material[3];
        //    Object16.GetComponent<Renderer>().material = Material[3];
        //    Object17.GetComponent<Renderer>().material = Material[3];
        //    Object18.GetComponent<Renderer>().material = Material[3];


        //}


        if (Size.isRedGrounded && redIsActive)
        {
            ChangeSize(redSize);


        }
        else
        {
            ChangeSize(startSize);
        }
    }

    void ChangeSize(Vector3 groesseAendern)
    {
        if (Size.isFacingRight)
        {
            Size.Player.gameObject.transform.localScale = groesseAendern;
        }

        else
        {
            Size.Player.gameObject.transform.localScale = new Vector3(-groesseAendern.x, groesseAendern.y, groesseAendern.z);
        }
    }
}