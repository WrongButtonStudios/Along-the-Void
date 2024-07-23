using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surprise : MonoBehaviour
{
    public float Direction = 0f;
    public float DirectionVertical = 0f;
    public float RightTrigger = 0f;
    public float RightStickButton = 0f;
    [SerializeField] public Rigidbody2D Fairy;
    public float Geschwindigkeit = 120f;
    public FairyHUDManager yellowFairyHUDManager;
    public FairyHUDManager blueFairyHUDManager;
    public FairyHUDManager redFairyHUDManager;
    public FairyHUDManager greenFairyHUDManager;
    public bool canMoveKeyboard = false;
    public bool canMoveController = false;
    public GameObject[] cameraList;
    private int currentCamera;
    public PlayerController playerController;
    public DashScript dashScript;
    [SerializeField] private Transform speedVFX;
    Vector3 speedVFXSizeNormal;
    [SerializeField] Vector3 increasedSpeedVFX;


    void Start()
    {
        Fairy = GetComponent<Rigidbody2D>();
        currentCamera = 0;
        for (int i = 0; i < cameraList.Length; i++)
        {
        }

        if (cameraList.Length > 0)
        {
            cameraList[0].gameObject.SetActive(true);
        }
        speedVFXSizeNormal = speedVFX.localScale;

    }


    void Update()
    {

        if (canMoveController && playerController.isGrounded || canMoveKeyboard && playerController.isGrounded)
        {
            playerController.Player.velocity = new Vector2(playerController.Player.velocity.x * 0.0001f, playerController.Player.velocity.y * 0.0001f);

            playerController.Player.constraints = RigidbodyConstraints2D.FreezeAll;






        }
        else
        {
            playerController.Player.constraints = RigidbodyConstraints2D.FreezeRotation;
        }




        Direction = Input.GetAxis("Fairy Horizontal");
        DirectionVertical = Input.GetAxis("Fairy Vertical");
        RightTrigger = Input.GetAxis("Right Trigger");


        if (Input.GetAxis("Right Trigger") > 0f || Input.GetKeyDown("g"))
        {
            yellowFairyHUDManager.enabled = true;
            blueFairyHUDManager.enabled = true;
            redFairyHUDManager.enabled = true;
            greenFairyHUDManager.enabled = true;
            canMoveKeyboard = false;
            canMoveController = false;
            playerController.enabled = true;
            dashScript.enabled = true;
            currentCamera++;
            if (currentCamera < cameraList.Length)
            {
                cameraList[currentCamera - 1].gameObject.SetActive(false);
                cameraList[currentCamera].gameObject.SetActive(true);
            }

        }

        MovementController();
        FarCam();

        Movement();

        void Movement()
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button9))
            {
                canMoveController = true;
                canMoveKeyboard = false;
                yellowFairyHUDManager.enabled = false;
                blueFairyHUDManager.enabled = false;
                redFairyHUDManager.enabled = false;
                greenFairyHUDManager.enabled = false;
                playerController.enabled = false;
                dashScript.enabled = false;
            }

            if (Input.GetKeyDown("f"))
            {
                canMoveKeyboard = true;
                canMoveController = false;
                yellowFairyHUDManager.enabled = false;
                blueFairyHUDManager.enabled = false;
                redFairyHUDManager.enabled = false;
                greenFairyHUDManager.enabled = false;
                playerController.enabled = false;
                dashScript.enabled = false;
            }




            if (canMoveKeyboard == true || canMoveController == true)
            {
                if (Direction > 0f)
                {
                    Fairy.velocity = new Vector2(Direction * Geschwindigkeit, Fairy.velocity.y);
                }


                if (Direction < 0f)
                {
                  
                    Fairy.velocity = new Vector2(Direction * Geschwindigkeit, Fairy.velocity.y);

                }
                if (DirectionVertical > 0f)
                {
               
                    Fairy.velocity = new Vector2(Fairy.velocity.x, -DirectionVertical * Geschwindigkeit);
                }


                if (DirectionVertical < 0f)
                {
    
                    Fairy.velocity = new Vector2(Fairy.velocity.x, -DirectionVertical * Geschwindigkeit);

                }
                if (Direction == 0f && DirectionVertical == 0f)
                {
                    Fairy.velocity = new Vector2(0f, 0f);
                }
                if (Input.GetAxis("Left Trigger") > 0f && canMoveController && !canMoveKeyboard || Input.GetKey("q") && canMoveKeyboard && !canMoveController)
                {
                    Debug.Log("Jawohl");
                    speedVFX.localScale = speedVFXSizeNormal;
                    currentCamera = 3;
                    cameraList[currentCamera - 3].gameObject.SetActive(false);
                    cameraList[currentCamera - 2].gameObject.SetActive(false);
                    cameraList[currentCamera - 1].gameObject.SetActive(false);
                    cameraList[currentCamera + 1].gameObject.SetActive(false);
                    cameraList[currentCamera + 2].gameObject.SetActive(false);
                    cameraList[currentCamera].gameObject.SetActive(true);
                }
            }
            if ((Input.GetAxis("Left Trigger") < 1f && canMoveController) && !canMoveKeyboard || !Input.GetKey("q") && canMoveKeyboard && !canMoveController)
            {
                Debug.Log("YES");
                speedVFX.localScale = speedVFXSizeNormal;
                currentCamera = 0;
                cameraList[currentCamera + 1].gameObject.SetActive(false);
                cameraList[currentCamera + 2].gameObject.SetActive(false);
                cameraList[currentCamera + 3].gameObject.SetActive(false);
                cameraList[currentCamera + 4].gameObject.SetActive(false);
                cameraList[currentCamera + 5].gameObject.SetActive(false);
                cameraList[currentCamera].gameObject.SetActive(true);
            }
        }


        void MovementController()
        {
            if (playerController.Geschwindigkeit > 20f && !canMoveController && playerController.Direction > 0f && !canMoveKeyboard)
            {
                speedVFX.localScale = speedVFXSizeNormal;
                currentCamera = 2;
                cameraList[currentCamera + 3].gameObject.SetActive(false);
                cameraList[currentCamera + 2].gameObject.SetActive(false);
                cameraList[currentCamera + 1].gameObject.SetActive(false);
                cameraList[currentCamera - 2].gameObject.SetActive(false);
                cameraList[currentCamera - 1].gameObject.SetActive(false);
                cameraList[currentCamera].gameObject.SetActive(true);

            }
            if (playerController.Geschwindigkeit > 20f && !canMoveController && playerController.Direction < 0f && !canMoveKeyboard)
            {

                speedVFX.localScale = speedVFXSizeNormal;
                currentCamera = 5;
                cameraList[currentCamera - 5].gameObject.SetActive(false);
                cameraList[currentCamera - 4].gameObject.SetActive(false);
                cameraList[currentCamera - 3].gameObject.SetActive(false);
                cameraList[currentCamera - 2].gameObject.SetActive(false);
                cameraList[currentCamera - 1].gameObject.SetActive(false);
                cameraList[currentCamera].gameObject.SetActive(true);
            }

            if (playerController.Geschwindigkeit < 20f && !canMoveController && !canMoveKeyboard && playerController.isGrounded)
            {
                speedVFX.localScale = speedVFXSizeNormal;
                currentCamera = 1;
                cameraList[currentCamera + 4].gameObject.SetActive(false);
                cameraList[currentCamera + 3].gameObject.SetActive(false);
                cameraList[currentCamera + 2].gameObject.SetActive(false);
                cameraList[currentCamera + 1].gameObject.SetActive(false);
                cameraList[currentCamera - 1].gameObject.SetActive(false);
                cameraList[currentCamera].gameObject.SetActive(true);


            }

        }
        void FarCam()
        {
            if (Input.GetAxis("Left Trigger") > 0f && !canMoveController && !canMoveKeyboard || Input.GetKey("q") && !canMoveController && !canMoveKeyboard || !playerController.isGrounded && playerController.Direction == 0f && dashScript.canDash == true)
            {
                speedVFX.localScale = increasedSpeedVFX;
                currentCamera = 4;
                cameraList[currentCamera - 4].gameObject.SetActive(false);
                cameraList[currentCamera - 3].gameObject.SetActive(false);
                cameraList[currentCamera - 2].gameObject.SetActive(false);
                cameraList[currentCamera - 1].gameObject.SetActive(false);
                cameraList[currentCamera + 1].gameObject.SetActive(false);
                cameraList[currentCamera].gameObject.SetActive(true);
            }
        }
    }

}